using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using IBApi.Errors;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Operations;
using IBApi.Serialization;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi
{
    public static class ConnectionFactory
    {
        public static IDisposable Connect(ConnectionParams connectionParams, Action<IClient> onSucessCallback,
            Action<Error> onErrorCallback)
        {
            CodeContract.Requires(SynchronizationContext.Current != null,
                "API need SynchronizationContext.Current to be not null");

            CodeContract.Requires(onSucessCallback != null);
            CodeContract.Requires(onErrorCallback != null);

            var stream = SetupConnection(connectionParams.HostName, connectionParams.Port);
            var serializer = new IBSerializer();

            Handshake(connectionParams.ClientId, stream, serializer);

            var connection = new Connection.Connection(stream, serializer);
            
            var connectionOperation = new ConnectOperation(new ApiObjectsFactory(connection), connection, onSucessCallback, onErrorCallback);
            
            connectionOperation.Execute();

            return connectionOperation;
        }

        private static Stream SetupConnection(string hostName, int port)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(hostName, port);

            Trace.TraceInformation("Socket opened");

            return new NetworkStream(socket, true);
        }

        private static void Handshake(int clientId, Stream stream, IBSerializer clientSerializer)
        {
            Trace.TraceInformation("Handshaking");
            
            clientSerializer.Write(new VersionMessage { Version = 46 }, stream);
            clientSerializer.ReadMessageWithoutId<AcknowledgementMessage>(stream);
            clientSerializer.ReadMessageWithoutId<TimeMessage>(stream);
            clientSerializer.Write(new IdMessage { ClientId = clientId }, stream);
        }

    }
}