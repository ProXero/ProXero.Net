using ProXero.Net.DataClasses;
using ProXero.Net.Interfaces;
using ProXero.Net.Messages;
using LightInject;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceContainer container = new ServiceContainer();

        private static IClient<IMessage> client;
        private static MainWindowViewModel vm;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            client = container.GetInstance<IClient<IMessage>>();

            vm = new MainWindowViewModel(client);

            client.Inbox.Subscribe(ReceiveMessage);

            client.ConnectAsync(new ServerAddress("localhost", 8090));


            var win = new MainWindow();
            win.DataContext = vm;

            Application.Current.MainWindow = win;

            win.Show();
        }

        private void ReceiveMessage(MessageInfo<IMessage> obj)
        {
            //client.SendMessage(new HeartBeat() { ID = 20 });
        }
    }
}
