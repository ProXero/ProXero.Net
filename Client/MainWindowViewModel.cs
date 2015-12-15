using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProXero.Net.Interfaces;
using ProXero.Net.Messages;

namespace Client
{
    class MainWindowViewModel : ViewModelBase
    {
        IClient<IMessage> client;

        public MainWindowViewModel(IClient<IMessage> client)
        {
            this.client = client;

            client.IsConnected.Subscribe(isOnline => IsOnline = isOnline);
        }

        private RelayCommand sendMessageCommand;

        public RelayCommand SendMessageCommand
        {
            get {
                if (sendMessageCommand == null)
                {
                    sendMessageCommand = new RelayCommand(() => client.SendMessage(new HeartBeat() { ID = 13 }));
                }
                return sendMessageCommand; 
            }
        }
        

        #region IsOnline

        private bool m_IsOnline;

        public bool IsOnline
        {
            get { return m_IsOnline; }
            set
            {
                m_IsOnline = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}
