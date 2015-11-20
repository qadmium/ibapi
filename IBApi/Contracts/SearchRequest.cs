namespace IBApi.Contracts
{
    public struct SearchRequest
    {
        public int? NumberOfResults { get; set; }
        public long ContractId { get; set; }
        public string Currency { get; set; }
        public string Exchange { get; set; }
        public string Expiry { get; set; }
        public bool IncludeExpired { get; set; }
        public string LocalSymbol { get; set; }
        public double? Multiplier { get; set; }
        public bool? Call { get; set; }
        public double? Strike { get; set; }
        public string Symbol { get; set; }
        public SecurityType? SecurityType { get; set; }
        public string SecId { get; set; }
        public string SecIdType { get; set; }
    }
}