using System;
using System.Threading.Tasks;
using IBApi.Errors;
using IBApi.Messages.Server;

namespace IBApi.Connection
{
    internal static class ConnectionExtensions
    {
        public static IDisposable SubscribeForRequestErrors(this IConnection connection, int requestId,
            Action<Error> onError)
        {
            return
                connection.Subscribe((ErrorMessage message) => message.RequestId == requestId,
                    errorMessage => onError(new Error
                    {
                        Code = errorMessage.ErrorCode,
                        Message = errorMessage.Message,
                        RequestId = errorMessage.RequestId
                    }));
        }

        public static IDisposable SubscribeForRequestErrors(this IConnection connection, int requestId,
            Action<Error> onError, TaskScheduler scheduler)
        {
            return
                connection.Subscribe((ErrorMessage message) => message.RequestId == requestId,
                    errorMessage => onError(new Error
                    {
                        Code = errorMessage.ErrorCode,
                        Message = errorMessage.Message,
                        RequestId = errorMessage.RequestId
                    }), scheduler);
        }

        public static IDisposable SubscribeForErrors(this IConnection connection, Func<Error, bool> whatErrors,
            Action<Error> onError)
        {
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
