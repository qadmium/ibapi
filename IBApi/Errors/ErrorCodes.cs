namespace IBApi.Errors
{
    public enum ErrorCode
    {
        UnknownError = 0,
        OrderDoesNotMatch = 105,
        OrderRejected = 201,
        OrderCanceled = 202,
        OrderWarning = 399,
        NewAccountDataRequested = 2100,
        MarketFarmConnected = 2104,
        DataFarmConnected = 2106,
        DataInactiveButAvailable = 2107,
        ClosingOrderQuantity = 2137,
        CrossCurrencyComboError = 10000
    }

    static class ErrorCodeExtensions
    {
        public static bool IsConnected(this ErrorCode ec)
        {
            return ec == ErrorCode.MarketFarmConnected ||
                   ec == ErrorCode.DataFarmConnected ||
                   ec == ErrorCode.DataInactiveButAvailable;
        }

        public static bool ConnectionStatusRelated(this ErrorCode ec)
        {
            return ec == ErrorCode.MarketFarmConnected ||
                   ec == ErrorCode.DataFarmConnected ||
                   ec == ErrorCode.DataInactiveButAvailable;
        }

        public static bool IsGeneralError(this ErrorCode ec)
        {
            return !ec.ConnectionStatusRelated();
        }

        public static bool IsWarning(this ErrorCode ec)
        {
            return (ec >= ErrorCode.NewAccountDataRequested && ec < ErrorCode.CrossCurrencyComboError)
                || ec == ErrorCode.OrderWarning;
        }
    }
}
