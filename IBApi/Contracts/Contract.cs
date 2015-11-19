using System;
using IBApi.Messages.Server;

namespace IBApi.Contracts
{
    public struct Contract : IEquatable<Contract>
    {
        public bool Equals(Contract other)
        {
            return string.Equals(Symbol, other.Symbol) && SecurityType == other.SecurityType &&
                   Expiry.Equals(other.Expiry) && Strike.Equals(other.Strike) && Call.Equals(other.Call) &&
                   string.Equals(LocalSymbol, other.LocalSymbol) && ContractId == other.ContractId;
        }

        public override bool Equals(object obj)
        {
            return obj is Contract && Equals((Contract) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Symbol != null ? Symbol.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) SecurityType;
                hashCode = (hashCode * 397) ^ Expiry.GetHashCode();
                hashCode = (hashCode * 397) ^ Strike.GetHashCode();
                hashCode = (hashCode * 397) ^ Call.GetHashCode();
                hashCode = (hashCode * 397) ^ (LocalSymbol != null ? LocalSymbol.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ContractId.GetHashCode();
                return hashCode;
            }
        }

        public string Symbol { get; set; }
        public SecurityType SecurityType { get; set; }
        public DateTime? Expiry { get; set; }
        public double? Strike { get; set; }
        public bool? Call { get; set; }
        public string LocalSymbol { get; set; }
        public int ContractId { get; set; }

        public AdditionalContractInfo AdditionalContractInfo;

        internal static Contract FromContractDataMessage(ContractDataMessage message)
        {
            var result = new Contract
            {
                Symbol = message.Symbol,
                SecurityType = (SecurityType)Enum.Parse(typeof(SecurityType), message.SecurityType),
                Expiry = message.Expiry,
                Strike = ParseStrike(message.Strike),
                Call = ParseCall(message.Right),
                LocalSymbol = message.LocalSymbol,
                ContractId = message.ContractId,

                AdditionalContractInfo = new AdditionalContractInfo
                {
                    Exchange = message.Exchange,
                    Currency = message.Currency,
                    Multiplier = message.Multiplier,
                    PrimaryExchange = message.PrimaryExchange,

                    MarketName = message.MarketName,
                    TradingClass = message.TradingClass,

                    MinTick = message.MinTick,

                    OrderTypes = message.OrderTypes,
                    ValidExchanges = message.ValidExchanges,
                    PriceMagnifier = message.PriceMagnifier,
                    UnderCondId = message.UnderCondId,
                    LongName = message.LongName,

                    ContractMonth = ParseContractMonth(message.ContractMonth),
                    Industry = message.Industry,
                    Category = message.Category,
                    SubCategory = message.SubCategory,
                    TimeZoneId = message.TimeZoneId,
                    TradingHours = message.TradingHours,
                    LiquidHours = message.LiquidHours
                }
            };

            return result;
        }

        internal static Contract FromExecutionDataMessage(ExecutionDataMessage message)
        {
            var result = new Contract
            {
                Symbol = message.Symbol,
                SecurityType = (SecurityType)Enum.Parse(typeof(SecurityType), message.SecurityType),
                Expiry = message.Expiry,
                Strike = ParseStrike(message.Strike),
                Call = ParseCall(message.Right),
                LocalSymbol = message.LocalSymbol,
                ContractId = message.ContractId,

                AdditionalContractInfo = new AdditionalContractInfo
                {
                    Exchange = message.Exchange,
                    Currency = message.Currency,    
                }
            };
            return result;
        }

        internal static Contract FromPortfolioValueMessage(PortfolioValueMessage message)
        {
            var result = new Contract
            {
                Symbol = message.Symbol,
                SecurityType = (SecurityType)Enum.Parse(typeof(SecurityType), message.SecurityType),
                Expiry = message.Expiry,
                Strike = ParseStrike(message.Strike),
                Call = ParseCall(message.Right),
                LocalSymbol = message.LocalSymbol,
                ContractId = message.ContractId,

                AdditionalContractInfo = new AdditionalContractInfo
                {
                    Multiplier = message.Multiplier,
                    PrimaryExchange = message.PrimaryExchange,    
                }
            };

            return result;
        }

        internal static Contract FromOpenOrderMessage(OpenOrderMessage message)
        {
            var result = new Contract
            {
                Symbol = message.Symbol,
                SecurityType = (SecurityType)Enum.Parse(typeof(SecurityType), message.SecurityType),
                Expiry = message.Expiry,
                Strike = ParseStrike(message.Strike),
                Call = ParseCall(message.Right),
                LocalSymbol = message.LocalSymbol,
                ContractId = message.ContractId,

                AdditionalContractInfo = new AdditionalContractInfo
                {
                    Exchange = message.Exchange,
                    Currency = message.Currency,
                }
            };

            return result;
        }

        private static double? ParseStrike(double strike)
        {
            if (Math.Abs(strike) < 0.0000000001)
            {
                return null;
            }

            return strike;
        }

        private static DateTime? ParseContractMonth(string contractMonth)
        {
            if (string.IsNullOrEmpty(contractMonth) || contractMonth.Length != 6)
            {
                return null;
            }

            try
            {
                var year = int.Parse(contractMonth.Substring(0, 4));
                var month = int.Parse(contractMonth.Substring(4, 2));

                return new DateTime(year, month, 1);
            }
            catch (FormatException)
            {
                return null;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        private static bool? ParseCall(string rightString)
        {
            if (string.IsNullOrEmpty(rightString))
            {
                return null;
            }

            if (rightString == "C")
            {
                return true;
            }

            return false;
        }
    }

    public struct AdditionalContractInfo
    {
        public string Multiplier { get; set; }
        public string Exchange { get; set; }
        public string PrimaryExchange { get; set; }
        public string Currency { get; set; }

        public string MarketName { get; set; }
        public string TradingClass { get; set; }
        public double MinTick { get; set; }
        public string OrderTypes { get; set; }
        public string ValidExchanges { get; set; }
        public int PriceMagnifier { get; set; }
        public int UnderCondId { get; set; }
        public string LongName { get; set; }
        public DateTime? ContractMonth { get; set; }
        public string Industry { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string TimeZoneId { get; set; }
        public string TradingHours { get; set; }
        public string LiquidHours { get; set; }
    }
}