
namespace IBApi.Contracts
{
    public enum SecurityType
    {
        None,

        /// <summary>
        /// Stock
        /// </summary>
        STK,

        /// <summary>
        /// Option
        /// </summary>
        OPT,

        /// <summary>
        /// FutureOption
        /// </summary>
        FOP,

        /// <summary>
        /// Future
        /// </summary>
        FUT,

        /// <summary>
        /// Index
        /// </summary>
        IND,

        /// <summary>
        /// Forex
        /// </summary>
        CASH,

        /// <summary>
        /// Commodity
        /// </summary>
        CMDTY
    }


}
