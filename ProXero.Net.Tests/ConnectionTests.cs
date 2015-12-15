using Moq;
using NUnit.Framework;
using ProXero.Net.DataClasses;
using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProXero.Net.Tests
{
    [TestFixture]
    public class ConnectionTests
    {
        const string host = "hostname";
        const int port = 12345;

        Mock<IStreamClientInbox<int>> clientInboxMock;
        Mock<IMessageSender<int>> messageSenderMock;
        Mock<IServerAddressConverter> addressConverter;
        Mock<IStreamClient> streamClient;
        Subject<int> inbox;

        Connection<int> connection;

        [SetUp]
        public void Setup()
        {
            streamClient = new Mock<IStreamClient>();
            clientInboxMock = new Mock<IStreamClientInbox<int>>();
            messageSenderMock = new Mock<IMessageSender<int>>();
            addressConverter = new Mock<IServerAddressConverter>();
            inbox = new Subject<int>();

            clientInboxMock.Setup(i => i.Inbox).Returns(inbox);

            connection = new Connection<int>(messageSenderMock.Object, clientInboxMock.Object, addressConverter.Object);
        }

        [Test]
        public void ListenStream_Perfect()
        {
            var results = new List<bool>();
            clientInboxMock.Setup(i => i.Listen(streamClient.Object)).Verifiable();

            connection.IsConnected.Subscribe(results.Add);
            connection.Listen(streamClient.Object);

            Assert.That(results.Count, Is.EqualTo(3));
            Assert.That(results, Is.EquivalentTo(new []{false, true, false}));
        }

        [Test]
        public void ListenAddress_Perfect()
        {
            List<bool> results = new List<bool>();
            ServerAddress address = new ServerAddress(host, port);
            addressConverter.Setup(add => add.Convert(address)).Returns(streamClient.Object).Verifiable();

            clientInboxMock.Setup(i => i.Listen(streamClient.Object)).Verifiable();

            connection.IsConnected.Subscribe(results.Add);
            connection.Listen(address);

            Assert.That(results.Count, Is.EqualTo(3));
            Assert.That(results, Is.EquivalentTo(new[] { false, true, false }));
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void SendMessage_MessageSenderAnswer(bool messageSenderReply, bool sendMessageReply)
        {
            const int message = 10;
            var exit = new AutoResetEvent(false);
            var listen = new AutoResetEvent(false);

            streamClient.Setup(s => s.GetStream()).Verifiable();
            clientInboxMock.Setup(i => i.Listen(streamClient.Object)).Callback(() =>
            {
                listen.Set();
                exit.WaitOne();
            }).Verifiable();
            messageSenderMock.Setup(i => i.SendMessage(It.IsAny<NetworkStream>(), message)).Returns(messageSenderReply).Verifiable();

            Task.Run(() => connection.Listen(streamClient.Object));
            listen.WaitOne();

            Assert.That(connection.SendMessage(message), Is.EqualTo(sendMessageReply));
            Assert.That(connection.IsConnected.Value, Is.EqualTo(sendMessageReply));

            exit.Set();
        }

        [Test]
        public void SendMessage_NoConnection()
        {
            Assert.That(connection.SendMessage(10), Is.False);
        }

        [Test]
        public void Inbox_Perfect()
        {
            MessageInfo<int> message = null;

            connection.Inbox.Subscribe(i => message = i);

            inbox.OnNext(20);

            Assert.That(message.Message, Is.EqualTo(20));
        }
    }
}
