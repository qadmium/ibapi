using System;
using System.Diagnostics.Contracts;
using IBApi.Errors;
using IBApi.Messages.Server;

namespace IBApi.Connection
{
    internal static class ConnectionExtensions
    {
        public static IDisposable SubscribeForRequestErrors(this IConnection connection, int requestId,
            Action<Error> onError)
        {
            Contract.Requires(connection != null);

            return
                connection.Subscribe((ErrorMessage message) => message.RequestId == requestId,
                    errorMessage => onError(new Error
                    {
                        Code = errorMessage.ErrorCode,
                        Message = errorMessage.Message,
                        RequestId = errorMessage.RequestId
                    }));
        }

        public static IDisposable SubscribeForErrors(this IConnection connection, Func<Error, bool> whatErrors,
            Action<Error> onError)
        {
            Contract.Requires(connection != null);

            return
                connection.Subscribe<ErrorMessage>(
                    errorMessage =>
                    {
                        var error = new Error
                        {
                            Code = errorMessage.ErrorCode,
                            Message = errorMessage.Message,
                            RequestId = errorMessage.RequestId
                        };

                        if (whatErrors(error))
                        {
                            onError(error);
                        }
                    });
        }
    }
}