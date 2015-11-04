using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using IBApi;
using IBApi.Contracts;
using IBApi.Errors;

namespace IBPlugin
{
    public partial class SymbolLookupPage : Form, IObserver<Contract>
    {
        public SymbolLookupPage()
        {
            InitializeComponent();
            
            InitInstrumentTypeCombo();
            InitContractsView();

            ErrorLabel.Visible = false;
            Call.CheckState = CheckState.Indeterminate;
        }

        private void SymbolLookupPage_Shown(object sender, EventArgs e)
        {
            What.Focus();
            DisableAll();

            Connect();
        }

        private void SymbolLookupPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (client != null)
            {
                client.Dispose();
            }
            else if (connectionOperation != null)
            {
                connectionOperation.Dispose();
            }
        }

        private void Connect()
        {
            var connectionParams = new ConnectionParams
            {
                ClientId = 0,
                HostName = "localhost",
                Port = 7496
            };

            try
            {
                connectionOperation = ConnectionFactory.Connect(connectionParams, OnSucessCallback, OnErrorCallback);
            }
            catch (Exception e)
            {
                ErrorLabel.Text = e.Message;
                ErrorLabel.Visible = true;
                return;
            }
            
            ErrorLabel.Visible = true;
            ErrorLabel.Text = "Connecting...";
        }

        private void OnErrorCallback(Error error)
        {
            ErrorLabel.Text = error.Message;
            connectionOperation = null;
        }

        private void OnSucessCallback(IClient client)
        {
            EnableAll();
            this.client = client;
            connectionOperation = null;
            ErrorLabel.Visible = false;
        }

        private void DisableAll()
        {
            What.Enabled = false;
            InstrumentTypeBox.Enabled = false;
            Search.Enabled = false;
            ContractsView.Enabled = false;
            ContractId.Enabled = false;
            Currency.Enabled = false;
            Exchange.Enabled = false;
            Expiry.Enabled = false;
            IncludeExpired.Enabled = false;
            LocalSymbol.Enabled = false;
            Multiplier.Enabled = false;
            Call.Enabled = false;
            Strike.Enabled = false;
            SecId.Enabled = false;
            SecIdType.Enabled = false;
        }

        private void EnableAll()
        {
            What.Enabled = true;
            InstrumentTypeBox.Enabled = true;
            Search.Enabled = true;
            ContractsView.Enabled = true;
            Currency.Enabled = true;
            Currency.Enabled = true;
            Exchange.Enabled = true;
            Expiry.Enabled = true;
            IncludeExpired.Enabled = true;
            LocalSymbol.Enabled = true;
            Multiplier.Enabled = true;
            Call.Enabled = true;
            Strike.Enabled = true;
            SecId.Enabled = true;
            SecIdType.Enabled = true;
        }

        private void InitInstrumentTypeCombo()
        {
            var choices = new Dictionary<string, SecurityType>
                {
                    { "Stock", SecurityType.STK },
                    { "Stock Option", SecurityType.OPT },
                    { "Future", SecurityType.FUT },
                    { "Future Option", SecurityType.FOP },
                    { "Index", SecurityType.IND },
                    { "Forex", SecurityType.CASH },
                    { "Commodity", SecurityType.CMDTY },
                    { "None", SecurityType.None }
                };

            InstrumentTypeBox.DataSource = new BindingSource(choices, null);
            InstrumentTypeBox.DisplayMember = "Key";
            InstrumentTypeBox.ValueMember = "Value";
        }

        private void InitContractsView()
        {
            contractList = new BindingList<ContractRow>();
            ContractsView.DataSource = contractList;
        }

        public bool OnApply()
        {
            return ContractsView.SelectedRows.Count == 1;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (Searching())
            {                
                StopSearch();
            }
            else
            {   
                StartSearch();
            }
        }

        private void What_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) Keys.Return || Searching())
            {
                return;
            }

            e.Handled = true;
            StartSearch();
        }

        private void StartSearch()
        {
            contractList.Clear();

            UpdateUIForSearch();

            var securityType = (SecurityType)InstrumentTypeBox.SelectedValue;

            var request = new SearchRequest
            {
                Symbol = What.Text,
                SecurityType = securityType,
                ContractId = (long)ContractId.Value,
                Currency = Currency.Text,
                Exchange = Exchange.Text,
                Expiry = Expiry.Text,
                IncludeExpired = IncludeExpired.Checked,
                LocalSymbol = LocalSymbol.Text,
                Multiplier = Multiplier.Value == 0.0m || string.IsNullOrEmpty(Multiplier.Text) ? null : (double?)Multiplier.Value,
                Call = Call.CheckState == CheckState.Indeterminate ? (bool?)null : Call.CheckState == CheckState.Checked,
                Strike = Strike.Value == 0.0m || string.IsNullOrEmpty(Strike.Text) ? null : (double?)Strike.Value,
                SecId = SecId.Text,
                SecIdType = SecIdType.Text
            };

            subscription = client.FindContracts(this, request);
        }

        private void StopSearch()
        {
            UpdateUIForInput();

            if (subscription == null)
            {
                return;
            }

            subscription.Dispose();
            subscription = null;
        }

        private void UpdateUIForInput()
        {
            Search.Text = "Search";

            What.Enabled = true;
            InstrumentTypeBox.Enabled = true;

            ProgressBar.Visible = false;
        }

        private void UpdateUIForSearch()
        {
            Search.Text = "Cancel";

            What.Enabled = false;
            InstrumentTypeBox.Enabled = false;

            ProgressBar.Visible = true;
            ErrorLabel.Visible = false;
        }

        private bool Searching()
        {
            return subscription != null;
        }

        public void OnNext(Contract contract)
        {
            var newRow = new ContractRow
            {
                Symbol = contract.Symbol,
                LocalSymbol = contract.LocalSymbol,
                ID = contract.ContractId,
                Type = contract.FormatContractType(),
                Exchange = contract.AdditionalContractInfo.Exchange,
                PrimaryExchange = contract.AdditionalContractInfo.PrimaryExchange,
                Description = contract.AdditionalContractInfo.LongName,
                Currency = contract.AdditionalContractInfo.Currency,
                ContractMonth = contract.FormatContractMonth(),
                Expiration = contract.Expiry.HasValue ? contract.Expiry.Value.ToShortDateString() : string.Empty,
                Strike = contract.FormatStrike(),
                PutCall = contract.FormatPutCall(),
                Multiplier = contract.AdditionalContractInfo.Multiplier,
                MinTick = contract.AdditionalContractInfo.MinTick
            };

            contractList.Add(newRow);
        }

        public void OnError(Exception error)
        {
            StopSearch();

            ErrorLabel.Text = error.Message;
            ErrorLabel.Visible = true;
        }

        public void OnCompleted()
        {
            StopSearch();
        }

        private BindingList<ContractRow> contractList;

        private IDisposable subscription;

        private IClient client;
        private IDisposable connectionOperation;
    }

    internal class ContractRow
    {
        public string Symbol { get; set; }
        public string LocalSymbol { get; set; }
        public long ID { get; set; }
        public string Type { get; set; }
        public string PrimaryExchange { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string ContractMonth { get; set; }
        public string Expiration { get; set; }
        public string Strike { get; set; }
        public string PutCall { get; set; }
        public string Exchange { get; set; }
        public double? Multiplier { get; set; }
        public double MinTick { get; set; }
    }

    internal static class ContractFormatters
    {
        public static string FormatStrike(this Contract contract)
        {
            if (contract.SecurityType != SecurityType.FOP && contract.SecurityType != SecurityType.OPT)
            {
                return string.Empty;
            }

            return contract.Strike.Value.ToString("0.00#", CultureInfo.InvariantCulture);
        }

        public static string FormatPutCall(this Contract contract)
        {
            if (contract.Call == null)
            {
                return string.Empty;
            }

            if (contract.Call.Value)
            {
                return "Call";
            }

            return "Put";
        }

        public static string FormatContractMonth(this Contract contract)
        {
            if (contract.AdditionalContractInfo.ContractMonth == null)
            {
                return string.Empty;
            }

            return contract.AdditionalContractInfo.ContractMonth.Value.ToString("Y");
        }

        public static string FormatContractType(this Contract contract)
        {
            switch (contract.SecurityType)
            {
                case SecurityType.STK:
                    return "Stock";

                case SecurityType.OPT:
                    return "Stock Option";

                case SecurityType.FOP:
                    return "Future Option";

                case SecurityType.FUT:
                    return "Future";

                case SecurityType.IND:
                    return "Index";

                case SecurityType.CASH:
                    return "Forex";

                case SecurityType.CMDTY:
                    return "Commodity";

                default:
                    return "";
            }
        }
    }
}
