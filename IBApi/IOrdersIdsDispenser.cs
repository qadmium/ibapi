using System;
using System.Threading;
using System.Threading.Tasks;

namespace IBApi
{
    internal interface IOrdersIdsDispenser : IDisposable
    {
        Task<int> NextOrderId(CancellationToken cancellationToken);
    }
}