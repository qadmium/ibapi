using IBApi.Contracts;
using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IBSerializable(9)]
    internal struct RequestContractDetailsMessage : IClientMessage
    {
        public int Version;

        public int RequestId;

        public long ContractId;

        public string Symbol;

        public string SecurityType;

        public string Expiry;

        public double Strike;

        public string Right;

        public double? Multiplier;

        public string Exchange;

        public string Currency;

        public string LocalSymbol;

        public bool IncludeExpired;

        public string SecIdType;

        public string SecId;

        public static RequestContractDetailsMessage Create(SearchRequest request)
        {
            
            return new RequestContractDetailsMessage
            {
                Version = 6,
                Currency = request.Currency,
                ContractId = request.ContractId,
                Exchange = request.Exchange,
                Expiry = request.Expiry,
                IncludeExpired = request.IncludeExpired,
                LocalSymbol = request.LocalSymbol,
                Multiplier = request.Multiplier,
                Right = !request.Call.HasValue ? string.Empty : request.Call.Value ? "C" : "P",
                Symbol = request.Symbol,
                SecurityType = request.SecurityType.ToString(),
                SecId = request.SecId,
                SecIdType = request.SecIdType,
                Strike = request.Strike ?? 0.0
            };
        }

    }
}
