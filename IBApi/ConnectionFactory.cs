using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Infrastructure;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Serialization;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi
{
    public static class ConnectionFactory
    {
        public static Task<IClient> Connect(ConnectionParams connectionParams, CancellationToken cancellationToken)
        {
            var apiCancellationTokenSource = new CancellationTokenSource();
            var dispatcher = SetupDispatcher(apiCancellationTokenSource);
            return dispatcher.Dispatch(() => ConnectInternal(connectionParams, cancellationToken, dispatcher, apiCancellationTokenSource));
        }

        private static Dispatcher SetupDispatcher(CancellationTokenSource apiCancellationTokenSource)
        {
            var ctx = new SingleThreadSynchronizationContext(apiCancellationTokenSource.Token);
            var dispatcherThread = new Thread(() => { ctx.Run(); });
            dispatcherThread.Start();

            var taskScheduler = GetTaskScheduler(ctx);
            
            var dispatcher = new Dispatcher(taskScheduler, apiCancellationTokenSource.Token, dispatcherThread);
            return dispatcher;
        }

        private static TaskScheduler GetTaskScheduler(SynchronizationContext ctx)
        {
            var old = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(ctx);
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(old);
            return scheduler;
        }

        private static async Task<IClient> ConnectInternal(ConnectionParams connectionParams, CancellationToken cancellationToken, Dispatcher dispatcher, CancellationTokenSource apiCancellationTokenSource)
        {
            var stream = await SetupConnection(connectionParams.HostName, connectionParams.Port);
            var fieldsStream = new FieldsStream(stream);
            var serializer = new IBSerializer();
            await Handshake(connectionParams.ClientId, fieldsStream, serializer, cancellationToken);

            var connection = new Connection.Connection(fieldsStream, serializer);
            var factory = new ApiObjectsFactory(connection, new IdsDispenser(connection), dispatcher, apiCancellationTokenSource);
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
            await Task.Factory.FromAsync(socket.BeginConnect(hostName, port, null, null), socket.EndConnect);
            Trace.TraceInformation("Socket opened");

            return new NetworkStream(socket, true);
        }

        private static async Task Handshake(int clientId, FieldsStream stream, IIbSerializer clientSerializer,
            CancellationToken cancellationToken)
        {
            Trace.TraceInformation("Handshaking");

            await clientSerializer.Write(new VersionMessage {Version = 63}, stream, cancellationToken);
            await clientSerializer.ReadMessageWithoutId<AcknowledgementMessage>(stream, cancellationToken);
            await clientSerializer.ReadMessageWithoutId<TimeMessage>(stream, cancellationToken);
            await clientSerializer.Write(new StartApiMessage {ClientId = clientId, Version = 1}, stream, cancellationToken);

            Trace.TraceInformation("Handshake done");
        }
    }
}