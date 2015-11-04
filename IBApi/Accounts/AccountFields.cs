namespace IBApi.Accounts
{
    public class AccountFields
    {
        public bool AccountIsTradeable { get; internal set; }
        public double BuyingPower { get; internal set; }
        public double TotalCashBalance { get; internal set; }
        public double StockMarketValue { get; internal set; }
        public double OptionMarketValue { get; internal set; }
        public double FutureMarketValue { get; internal set; }
        public double AccruedCash { get; internal set; }
        public double NetLiquidation { get; internal set; }
        public string Currency { get; internal set; }
        public double RealizedPnL { get; internal set; }
        public double UnrealizedPnL { get; internal set; }
        public double ExchangeRate { get; internal set; }
        public double FullInitMarginReq { get; internal set; }
        public double FullMaintMarginReq { get; internal set; }
        public double FullAvailableFunds { get; internal set; }
    }
}