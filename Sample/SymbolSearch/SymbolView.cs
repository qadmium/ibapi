using System;
using Caliburn.Micro;
using IBApi.Contracts;

namespace Sample.SymbolSearch
{
    internal class SymbolView : PropertyChangedBase
    {
        private string symbol;
        private SecurityType securityType;
        private DateTime? expiry;
        private double? strike;
        private bool? call;
        private string localSymbol;
        private int contractId;
        private string multiplier;
        private string exchange;
        private string primaryExchange;
        private string currency;
        private string marketName;
        private string tradingClass;
        private double minTick;
        private string orderTypes;
        private string validExchanges;
        private int priceMagnifier;
        private int underCondId;
        private string longName;
        private DateTime? contractMonth;
        private string industry;
        private string category;
        private string subCategory;
        private string timeZoneId;
        private string tradingHours;
        private string liquidHours;

        public string Symbol
        {
            get { return this.symbol; }
            set
            {
                if (value == this.symbol) return;
                this.symbol = value;
                this.NotifyOfPropertyChange(() => this.Symbol);
            }
        }

        public SecurityType SecurityType
        {
            get { return this.securityType; }
            set
            {
                if (value == this.securityType) return;
                this.securityType = value;
                this.NotifyOfPropertyChange(() => this.SecurityType);
            }
        }

        public DateTime? Expiry
        {
            get { return this.expiry; }
            set
            {
                if (value.Equals(this.expiry)) return;
                this.expiry = value;
                this.NotifyOfPropertyChange(() => this.Expiry);
            }
        }

        public double? Strike
        {
            get { return this.strike; }
            set
            {
                if (value.Equals(this.strike)) return;
                this.strike = value;
                this.NotifyOfPropertyChange(() => this.Strike);
            }
        }

        public bool? Call
        {
            get { return this.call; }
            set
            {
                if (value == this.call) return;
                this.call = value;
                this.NotifyOfPropertyChange(() => this.Call);
            }
        }

        public string LocalSymbol
        {
            get { return this.localSymbol; }
            set
            {
                if (value == this.localSymbol) return;
                this.localSymbol = value;
                this.NotifyOfPropertyChange(() => this.LocalSymbol);
            }
        }

        public int ContractId
        {
            get { return this.contractId; }
            set
            {
                if (value == this.contractId) return;
                this.contractId = value;
                this.NotifyOfPropertyChange(() => this.ContractId);
            }
        }

        public string Multiplier
        {
            get { return this.multiplier; }
            set
            {
                if (value == this.multiplier) return;
                this.multiplier = value;
                this.NotifyOfPropertyChange(() => this.Multiplier);
            }
        }

        public string Exchange
        {
            get { return this.exchange; }
            set
            {
                if (value == this.exchange) return;
                this.exchange = value;
                this.NotifyOfPropertyChange(() => this.Exchange);
            }
        }

        public string PrimaryExchange
        {
            get { return this.primaryExchange; }
            set
            {
                if (value == this.primaryExchange) return;
                this.primaryExchange = value;
                this.NotifyOfPropertyChange(() => this.PrimaryExchange);
            }
        }

        public string Currency
        {
            get { return this.currency; }
            set
            {
                if (value == this.currency) return;
                this.currency = value;
                this.NotifyOfPropertyChange(() => this.Currency);
            }
        }

        public string MarketName
        {
            get { return this.marketName; }
            set
            {
                if (value == this.marketName) return;
                this.marketName = value;
                this.NotifyOfPropertyChange(() => this.MarketName);
            }
        }

        public string TradingClass
        {
            get { return this.tradingClass; }
            set
            {
                if (value == this.tradingClass) return;
                this.tradingClass = value;
                this.NotifyOfPropertyChange(() => this.TradingClass);
            }
        }

        public double MinTick
        {
            get { return this.minTick; }
            set
            {
                if (value.Equals(this.minTick)) return;
                this.minTick = value;
                this.NotifyOfPropertyChange(() => this.MinTick);
            }
        }

        public string OrderTypes
        {
            get { return this.orderTypes; }
            set
            {
                if (value == this.orderTypes) return;
                this.orderTypes = value;
                this.NotifyOfPropertyChange(() => this.OrderTypes);
            }
        }

        public string ValidExchanges
        {
            get { return this.validExchanges; }
            set
            {
                if (value == this.validExchanges) return;
                this.validExchanges = value;
                this.NotifyOfPropertyChange(() => this.ValidExchanges);
            }
        }

        public int PriceMagnifier
        {
            get { return this.priceMagnifier; }
            set
            {
                if (value == this.priceMagnifier) return;
                this.priceMagnifier = value;
                this.NotifyOfPropertyChange(() => this.PriceMagnifier);
            }
        }

        public int UnderCondId
        {
            get { return this.underCondId; }
            set
            {
                if (value == this.underCondId) return;
                this.underCondId = value;
                this.NotifyOfPropertyChange(() => this.UnderCondId);
            }
        }

        public string LongName
        {
            get { return this.longName; }
            set
            {
                if (value == this.longName) return;
                this.longName = value;
                this.NotifyOfPropertyChange(() => this.LongName);
            }
        }

        public DateTime? ContractMonth
        {
            get { return this.contractMonth; }
            set
            {
                if (value.Equals(this.contractMonth)) return;
                this.contractMonth = value;
                this.NotifyOfPropertyChange(() => this.ContractMonth);
            }
        }

        public string Industry
        {
            get { return this.industry; }
            set
            {
                if (value == this.industry) return;
                this.industry = value;
                this.NotifyOfPropertyChange(() => this.Industry);
            }
        }

        public string Category
        {
            get { return this.category; }
            set
            {
                if (value == this.category) return;
                this.category = value;
                this.NotifyOfPropertyChange(() => this.Category);
            }
        }

        public string SubCategory
        {
            get { return this.subCategory; }
            set
            {
                if (value == this.subCategory) return;
                this.subCategory = value;
                this.NotifyOfPropertyChange(() => this.SubCategory);
            }
        }

        public string TimeZoneId
        {
            get { return this.timeZoneId; }
            set
            {
                if (value == this.timeZoneId) return;
                this.timeZoneId = value;
                this.NotifyOfPropertyChange(() => this.TimeZoneId);
            }
        }

        public string TradingHours
        {
            get { return this.tradingHours; }
            set
            {
                if (value == this.tradingHours) return;
                this.tradingHours = value;
                this.NotifyOfPropertyChange(() => this.TradingHours);
            }
        }

        public string LiquidHours
        {
            get { return this.liquidHours; }
            set
            {
                if (value == this.liquidHours) return;
                this.liquidHours = value;
                this.NotifyOfPropertyChange(() => this.LiquidHours);
            }
        }
    }
}