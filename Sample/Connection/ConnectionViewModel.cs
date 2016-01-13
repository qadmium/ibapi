using System;
using System.Net.Sockets;
using System.Threading;
using Caliburn.Micro;
using IBApi;

namespace Sample.Connection
{
    internal sealed class ConnectionViewModel : Screen
    {
        private string connectButtonCaption;
        private IClient client;
        private string status;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private bool connecting;
        private bool inputControlsEnabled;
        private string hostname;
        private string port;
        private string clientId;
        private string reason;

        public System.Action<IClient> OnConnected { get; set; }

        public ConnectionViewModel()
        {
            this.DisplayName = "Connect to TWS";
            this.Connecting = false;
            this.ClientId = "0";
            this.Hostname = "localhost";
            this.Port = "7496";
        }

        public string Status
        {
            get { return this.status; }
            set
            {
                if (value == this.status) return;
                this.status = value;
                this.NotifyOfPropertyChange(() => this.Status);
            }
        }

        public string Hostname
        {
            get { return this.hostname; }
            set
            {
                if (value == this.hostname) return;
                this.hostname = value;
                this.NotifyOfPropertyChange(() => this.Hostname);
            }
        }

        public string Port
        {
            get { return this.port; }
            set
            {
                if (value == this.port) return;
                this.port = value;
                this.NotifyOfPropertyChange(() => this.Port);
            }
        }

        public string ClientId
        {
            get { return this.clientId; }
            set
            {
                if (value == this.clientId) return;
                this.clientId = value;
                this.NotifyOfPropertyChange(() => this.ClientId);
            }
        }

        public string ConnectButtonCaption
        {
            get { return this.connectButtonCaption; }
            set
            {
                if (value == this.connectButtonCaption) return;
                this.connectButtonCaption = value;
                this.NotifyOfPropertyChange(() => this.ConnectButtonCaption);
            }
        }

        public string Reason
        {
            get { return reason; }
            set
            {
                if (value == reason) return;
                reason = value;
                NotifyOfPropertyChange(() => Reason);
            }
        }

        public bool InputControlsEnabled
        {
            get { return this.inputControlsEnabled; }
            set
            {
                if (value == this.inputControlsEnabled) return;
                this.inputControlsEnabled = value;
                this.NotifyOfPropertyChange(() => this.InputControlsEnabled);
            }
        }

        private bool Connecting
        {
            get { return this.connecting; }
            set
            {
                this.connecting = value;

                if (value)
                {
                    this.Status = "Connecting...";
                    this.InputControlsEnabled = false;
                    this.ConnectButtonCaption = "Cancel";
                    return;
                }

                this.Status = "Disconnected";
                this.InputControlsEnabled = true;
                this.ConnectButtonCaption = "Connect";
            }
        }

        public async void Connect()
        {
            if (this.Connecting)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = new CancellationTokenSource();
                this.Connecting = false;
                return;
            }

            this.Connecting = true;

            try
            {
                this.client = await ConnectionFactory.Connect(new ConnectionParams
                {
                    ClientId = int.Parse(this.ClientId),
                    HostName = this.Hostname,
                    Port = int.Parse(this.Port)
                }, this.cancellationTokenSource.Token);
            }
            catch (SocketException e)
            {
                this.Connecting = false;
                Reason = e.Message;
                return;
            }

            this.OnConnected(this.client);
        }
    }
}