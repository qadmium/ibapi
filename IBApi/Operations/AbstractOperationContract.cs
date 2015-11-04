using System;
using System.Diagnostics.Contracts;
using IBApi.Connection;

namespace IBApi.Operations
{
    [ContractClassFor(typeof(AbstractOperation))]
    internal abstract class AbstractOperationContract : AbstractOperation
    {
        public override void Execute(IConnection connection)
        {
            Contract.Requires<ArgumentNullException>(connection != null);
        }
    }
}