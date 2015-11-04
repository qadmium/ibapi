namespace IBApi.Orders
{
    public enum OrderType
    {
        Limit,
        Market,
        Stop,
        StopLimit,
        MarketToLimit,
        MarketIfTouched,
        LimitIfTouched,
        TrailingStop,
        TrailingStopLimit,
        TrailingLimitIfTouched,
        Relative,
        RetailPriceImprovement,
        MarketOnClose,
        LimitOnClose,
        PeggedToBenchmark
    }
}