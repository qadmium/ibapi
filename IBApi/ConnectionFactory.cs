using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Serialization;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi
{
    public static class ConnectionFactory
    {
        public static async Task<IClient> Connect(ConnectionParams connectionParams, CancellationToken cancellationToken)
        {
            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
            }

            var stream = await SetupConnection(connectionParams.HostName, connectionParams.Port);
            var fieldsStream = new FieldsStream(stream);
            var serializer = new IBSerializer();
            await Handshake(connectionParams.ClientId, fieldsStream, serializer, cancellationToken);

            var connection = new Connection.Connection(fieldsStream, serializer);
            var factory = new ApiObjectsFactory(connection);
            var waitForMarketConnected = factory.CreateWaitForMarketConnectedOperation(cancellationToken);
            var waitForAccountsList = factory.CreateReceiveManagedAccountsListOperation(cancellationToken);

            connection.ReadMessagesAndDispatch();

            await waitForMarketConnected;
            var accountStorage = factory.CreateAccountStorageOperation(await waitForAccountsList, cancellationToken);

            return factory.CreateClient(await accountStorage);
        }

        private static async Task<NetworkStream> SetupConnection(string hostName, int port)
        {
            Trace.TraceInformation("Opening socket");
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, hostName, port, null);
            Trace.TraceInformation("Socket opened");

            return new NetworkStream(socket, true);
        }

        private static async Task Handshake(int clientId, FieldsStream stream, IIbSerializer clientSerializer,
            CancellationToken cancellationToken)
        {
            Trace.TraceInformation("Handshaking");

            await clientSerializer.Write(new VersionMessage {Version = 46}, stream, cancellationToken);
            await clientSerializer.ReadMessageWithoutId<AcknowledgementMessage>(stream, cancellationToken);
            await clientSerializer.ReadMessageWithoutId<TimeMessage>(stream, cancellationToken);
            await clientSerializer.Write(new IdMessage {ClientId = clientId}, stream, cancellationToken);

            Trace.TraceInformation("Handshake done");
        }
    }
}