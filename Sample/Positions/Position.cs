using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using IBApi.Positions;

namespace Sample.Positions
{
    class Position : PropertyChangedBase
    {
        private string accountName;
        private double averagePrice;
        private double marketPrice;
        private double marketValue;
        private int quantity;
        private double realizedPl;
        private double openPl;
        private string symbol;

        public Position(IPosition position)
        {
            position.PositionChanged += this.OnPositionChanged;
            this.AccountName = position.AccountName;
            this.Symbol = position.Contract.Symbol;

            this.OnPositionChanged(position);
        }

        public string AccountName
        {
            get { return this.accountName; }
            set
            {
                if (value == this.accountName) return;
                this.accountName = value;
                this.NotifyOfPropertyChange(() => this.AccountName);
            }
        }

        private void OnPositionChanged(IPosition account)
        {
            this.AveragePrice = account.AveragePrice;
            this.MarketPrice = account.MarketPrice;
            this.MarketValue = account.MarketValue;
            this.Quantity = account.Quantity;
            this.RealizedPL = account.RealizedPL;
            this.OpenPL = account.OpenPL;
        }

        public double AveragePrice
        {
            get { return this.averagePrice; }
            set
            {
                if (value.Equals(this.averagePrice)) return;
                this.averagePrice = value;
                this.NotifyOfPropertyChange(() => this.AveragePrice);
            }
        }

        public double MarketPrice
        {
            get { return this.marketPrice; }
            private set
            {
                if (value.Equals(this.marketPrice)) return;
                this.marketPrice = value;
                this.NotifyOfPropertyChange(() => this.MarketPrice);
            }
        }

        public double MarketValue
        {
            get { return this.marketValue; }
            private set
            {
                if (value.Equals(this.marketValue)) return;
                this.marketValue = value;
                this.NotifyOfPropertyChange(() => this.MarketValue);
            }
        }

        public int Quantity
        {
            get { return this.quantity; }
            private set
            {
                if (value == this.quantity) return;
                this.quantity = value;
                this.NotifyOfPropertyChange(() => this.Quantity);
            }
        }

        public double RealizedPL
        {
            get { return this.realizedPl; }
            private set
            {
                if (value.Equals(this.realizedPl)) return;
                this.realizedPl = value;
                this.NotifyOfPropertyChange(() => this.RealizedPL);
            }
        }

        public double OpenPL
        {
            get { return this.openPl; }
            private set
            {
                if (value.Equals(this.openPl)) return;
                this.openPl = value;
                this.NotifyOfPropertyChange(() => this.OpenPL);
            }
        }

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
    }
}
