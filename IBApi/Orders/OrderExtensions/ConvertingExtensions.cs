using System;

namespace IBApi.Orders.OrderExtensions
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

        public static string ToStringAction(this OrderAction action)
        {
            switch (action)
            {
                case OrderAction.Buy:
                    return "BUY";

                case OrderAction.Sell:
                    return "SELL";
                default:
                    throw new ArgumentException("Unsupported action", action.ToString());
            }
        }

        public static OrderType ToOrderType(this string type)
        {
            switch (type)
            {
                case "LMT":
                    return OrderType.Limit;
                case "MKT PRT":
                    return OrderType.MarketWithProtection;
                case "MKT":
                    return OrderType.Market;
                case "QUOTE":
                    return OrderType.RequestToQuote;
                case "STP":
                    return OrderType.Stop;
                case "MTL":
                    return OrderType.MarketToLimit;
                case "STP LMT":
                    return OrderType.StopLimit;
                case "MIT":
                    return OrderType.MarketIfTouched;
                case "LIT":
                    return OrderType.LimitIfTouched;
                case "TRAIL":
                    return OrderType.TrailingStop;
                case "TRAIL LIMIT":
                    return OrderType.TrailingStopLimit;
                case "TRAIL LIT":
                    return OrderType.TrailingLimitIfTouched;
                case "TRAIL MIT":
                    return OrderType.TrailingMarketIfTouched;
                case "REL":
                    return OrderType.Relative;
                case "RPI":
                    return OrderType.RetailPriceImprovement;
                case "MOC":
                    return OrderType.MarketOnClose;
                case "MOO":
                    return OrderType.MarketOnOpen;
                case "LOC":
                    return OrderType.LimitOnClose;
                case "LOO":
                    return OrderType.LimitOnOpen;
                case "PEG BENCH":
                    return OrderType.PeggedToBenchmark;
                case "PEG MKT":
                    return OrderType.PeggedToMarket;
                case "PEG MID":
                    return OrderType.PeggedToMidpoint;
                case "BOX TOP":
                    return OrderType.BoxTop;
                case "VWAP":
                    return OrderType.Vwap;
                case "GAT":
                    return OrderType.GoodAfterTimeDate;
                case "GTD":
                    return OrderType.GoodTillDateTime;
                case "GTC":
                    return OrderType.GoodTillCanceled;
                case "IOC":
                    return OrderType.ImmediateOrCancel;
                case "OCA":
                    return OrderType.OneCancelsAll;
                case "VOL":
                    return OrderType.Volatility;

                default:
                    throw new ArgumentException("Unsupported type", type);
            }
        }

        public static string ToOrderString(this OrderType type)
        {
            switch (type)
            {
                case OrderType.Limit:
                    return "LMT";
                case OrderType.MarketWithProtection:
                    return "MKT PRT";
                case OrderType.Market:
                    return "MKT";
                case OrderType.RequestToQuote:
                    return "QUOTE";
                case OrderType.Stop:
                    return "STP";
                case OrderType.MarketToLimit:
                    return "MTL";
                case OrderType.StopLimit:
                    return "STP LMT";
                case OrderType.MarketIfTouched:
                    return "MIT";
                case OrderType.LimitIfTouched:
                    return "LIT";
                case OrderType.TrailingStop:
                    return "TRAIL";
                case OrderType.TrailingStopLimit:
                    return "TRAIL LIMIT";
                case OrderType.TrailingLimitIfTouched:
                    return "TRAIL LIT";
                case OrderType.TrailingMarketIfTouched:
                    return "TRAIL MIT";
                case OrderType.Relative:
                    return "REL";
                case OrderType.RetailPriceImprovement:
                    return "RPI";
                case OrderType.MarketOnClose:
                    return "MOC";
                case OrderType.MarketOnOpen:
                    return "MOO";
                case OrderType.LimitOnClose:
                    return "LOC";
                case OrderType.LimitOnOpen:
                    return "LOO";
                case OrderType.PeggedToBenchmark:
                    return "PEG BENCH";
                case OrderType.PeggedToMarket:
                    return "PEG MKT";
                case OrderType.PeggedToMidpoint:
                    return "PEG MID";
                case OrderType.BoxTop:
                    return "BOX TOP";
                case OrderType.Vwap:
                    return "VWAP";
                case OrderType.GoodAfterTimeDate:
                    return "GAT";
                case OrderType.GoodTillDateTime:
                    return "GTD";
                case OrderType.GoodTillCanceled:
                    return "GTC";
                case OrderType.ImmediateOrCancel:
                    return "IOC";
                case OrderType.OneCancelsAll:
                    return "OCA";
                case OrderType.Volatility:
                    return "VOL";
                default:
                    throw new ArgumentException("Unsupported type", type.ToString());
            }
        }
    }
}