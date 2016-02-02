using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using IBApi;
using IBApi.Contracts;

namespace Sample.SymbolSearch
{
    internal sealed class SymbolSearchViewModel : Screen
    {
        private readonly IClient client;
        private int contractsToSearch;
        private string currency;
        private IObservableCollection<SymbolView> results = new BindableCollection<SymbolView>();
        private IObservableCollection<SecurityType> securityTypes;
        private SecurityType selectedSecurityType;
        private string ticker;

        public SymbolSearchViewModel(IClient client)
        {
            this.client = client;
            this.DisplayName = "Symbol search";
            this.ContractsToSearch = 10;

            this.SecurityTypes = new BindableCollection<SecurityType>
            {
                SecurityType.STK,
                SecurityType.FUT,
                SecurityType.OPT,
                SecurityType.FOP,
                SecurityType.CASH,
                SecurityType.CMDTY,
                SecurityType.IND
            };

            this.SelectedSecurityType = SecurityType.STK;
        }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Ticker
        {
            get { return this.ticker; }
            set
            {
                if (value == this.ticker) return;
                this.ticker = value;
                this.NotifyOfPropertyChange(() => this.Ticker);
            }
        }

        public int ContractsToSearch
        {
            get { return this.contractsToSearch; }
            set
            {
                if (value == this.contractsToSearch) return;
                this.contractsToSearch = value;
                this.NotifyOfPropertyChange(() => this.ContractsToSearch);
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

        public SecurityType SelectedSecurityType
        {
            get { return this.selectedSecurityType; }
            set
            {
                if (value == this.selectedSecurityType) return;
                this.selectedSecurityType = value;
                this.NotifyOfPropertyChange(() => this.SelectedSecurityType);
            }
        }

        public IObservableCollection<SecurityType> SecurityTypes
        {
            get { return this.securityTypes; }
            set
            {
                if (Equals(value, this.securityTypes)) return;
                this.securityTypes = value;
                this.NotifyOfPropertyChange(() => this.SecurityTypes);
            }
        }

        public IObservableCollection<SymbolView> Results
        {
            get { return this.results; }
            set
            {
                if (Equals(value, this.results)) return;
                this.results = value;
                this.NotifyOfPropertyChange(() => this.Results);
            }
        }

        public async void Search()
        {
            this.Results.Clear();
            var request = new SearchRequest
            {
                Symbol = this.Ticker, NumberOfResults = this.ContractsToSearch,
                SecurityType = this.SelectedSecurityType
            };

            if (!string.IsNullOrEmpty(this.Currency))
            {
                request.Currency = this.Currency;
            }

            var contracts = await this.client.FindContracts(request, CancellationToken.None);

            this.Results.AddRange(ConvertToResults(contracts));
        }

        private static IEnumerable<SymbolView> ConvertToResults(IEnumerable<Contract> contracts)
        {
            return contracts.Select(contract => new SymbolView
            {
                Exchange = contract.AdditionalContractInfo.Exchange,
                Symbol = contract.Symbol,
                Call = contract.Call,
                Category = contract.AdditionalContractInfo.Category,
                ContractId = contract.ContractId,
                ContractMonth = contract.AdditionalContractInfo.ContractMonth,
                Currency = contract.AdditionalContractInfo.Currency,
                Expiry = contract.Expiry,
                Industry = contract.AdditionalContractInfo.Industry,
                LiquidHours = contract.AdditionalContractInfo.LiquidHours,
                LocalSymbol = contract.LocalSymbol,
                LongName = contract.AdditionalContractInfo.LongName,
                MarketName = contract.AdditionalContractInfo.MarketName,
                Multiplier = contract.AdditionalContractInfo.Multiplier,
                MinTick = contract.AdditionalContractInfo.MinTick,
                OrderTypes = contract.AdditionalContractInfo.OrderTypes,
                PriceMagnifier = contract.AdditionalContractInfo.PriceMagnifier,
                PrimaryExchange = contract.AdditionalContractInfo.PrimaryExchange,
                SecurityType = contract.SecurityType,
                Strike = contract.Strike,
                SubCategory = contract.AdditionalContractInfo.SubCategory,
                TimeZoneId = contract.AdditionalContractInfo.TimeZoneId,
                TradingClass = contract.AdditionalContractInfo.TradingClass,
                TradingHours = contract.AdditionalContractInfo.TradingHours,
                UnderCondId = contract.AdditionalContractInfo.UnderCondId,
                ValidExchanges = contract.AdditionalContractInfo.ValidExchanges
            });
        }
    }
}