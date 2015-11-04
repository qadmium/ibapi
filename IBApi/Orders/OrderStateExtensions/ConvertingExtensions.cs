using System;

namespace IBApi.Orders.OrderStateExtensions
{
    internal static class ConvertingExtensions
    {
        public static OrderState ToOrderState(this string state)
        {
            OrderState result;
            
            if (Enum.TryParse(state, out result))
            {
                return result;
            }

            return OrderState.Invalid;
        }

        public static OrderAction ToOrderAction(this string action)
        {
            if (action == "BUY")
            {
                return OrderAction.Buy;    
            }
            
            if (action == "SELL")
            {
                return OrderAction.Sell;
            }

            throw new ArgumentException("Unsupported action", action);
        }

        public static OrderType ToOrderType(this string type)
        {
            if (type == "LMT")
            {
                return OrderType.Limit;
            }

            if (type == "MKT")
            {
                return OrderType.Market;
            }

            if (type == "STP")
            {
                return OrderType.Stop;
            }

            if (type == "MTL")
            {
                return OrderType.MarketToLimit;
            }

            if (type == "STP LMT")
            {
                return OrderType.StopLimit;
            }

            if (type == "MIT")
            {
                return OrderType.MarketIfTouched;
            }

            if (type == "LIT")
            {
                return OrderType.LimitIfTouched;
            }

            if (type == "TRAIL")
            {
                return OrderType.TrailingStop;
            }

            if (type == "TRAIL LIMIT")
            {
                return OrderType.TrailingStopLimit;
            }

            if (type == "TRAIL LIT")
            {
                return OrderType.TrailingLimitIfTouched;
            }

            if (type == "REL")
            {
                return OrderType.Relative;
            }

            if (type == "RPI")
            {
                return OrderType.RetailPriceImprovement;
            }

            if (type == "MOC")
            {
                return OrderType.MarketOnClose;
            }

            if (type == "LOC")
            {
                return OrderType.LimitOnClose;
            }

            if (type == "PEG BENCH")
            {
                return OrderType.PeggedToBenchmark;
            }
            
            throw new ArgumentException("Unsupported type", type);
        }
    }
}
