namespace IBApi.Errors
{
    public enum ErrorCode
    {
        UnknownError = 0,
        MarketFarmConnected = 2104,
        DataFarmConnected = 2106,
        DataInactiveButAvailable = 2107
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
    }
}
