using System;
using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(59)]
    class CommissionsReportMessage : IServerMessage
    {
        public int Version;
        public string ExecutionId;
        public double Commission;
        public string Currency;
        public double RealizedPnL;
        public double Yield;
        public DateTime YieldRedemptionDate;

    }
}
