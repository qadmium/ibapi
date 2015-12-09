using System;

namespace IBApi
{
    internal interface IIdsDispenser : IDisposable
    {
        int NextOrderId();
        int NextRequestId();
    }
}