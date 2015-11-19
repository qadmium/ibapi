namespace IBApi.Messages.Client
{
    internal enum TriggerMethod
    {
        Default = 0,
        DoubleBidAsk = 1,
        Last = 2,
        DoubleLast = 3,
        BidAsk = 4,
        LastOrBidAsk = 7,
        MidPoint = 8
    }
}