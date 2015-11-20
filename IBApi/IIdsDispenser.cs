using System;
using System.Threading;
using System.Threading.Tasks;

namespace IBApi
{
    internal interface IIdsDispenser : IDisposable
    {
        Task<int> NextId(CancellationToken cancellationToken);
    }
}