using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace IBApi.Accounts
{
    internal class AccountCurrenciesFields
    {
        private const string baseCurrencyId = "BASE";
        private static readonly PropertyInfo[] accountProperties = typeof(AccountFields).GetProperties();

        private readonly IDictionary<string, AccountFields> accountFields = new Dictionary<string, AccountFields>();

        public AccountFields AccountFields
        {
            get { return this.accountFields[baseCurrencyId]; }
        }

        public string[] Currencies
        {
            get { return this.accountFields.Keys.ToArray(); }
        }

        public AccountFields this[string currency]
        {
            get { return this.accountFields[currency]; }
        }

        public void Update(AccountValue accountValue)
        {
            var currencyKey = CurrencyKey(accountValue.Currency);

            //Trace.TraceInformation("{0} {1} {2}", accountValue.Currency, accountValue.Key, accountValue.Value);

            var property = accountProperties.SingleOrDefault(prop => prop.Name == accountValue.Key);

            if (property == null)
            {
                return;
            }

            var accountFieldsInstance = this.GetAccountFieldsInstance(currencyKey);
            property.SetValue(accountFieldsInstance,
                Convert.ChangeType(accountValue.Value, property.PropertyType, CultureInfo.InvariantCulture), null);
        }

        private static string CurrencyKey(string currency)
        {
            if (string.IsNullOrEmpty(currency) || currency == baseCurrencyId)
            {
                return baseCurrencyId;
            }

            return currency;
        }

        private AccountFields GetAccountFieldsInstance(string forCurrency)
        {
            AccountFields accountFieldsInstance;

            if (this.accountFields.TryGetValue(forCurrency, out accountFieldsInstance))
            {
                return accountFieldsInstance;
            }

            accountFieldsInstance = new AccountFields();
            this.accountFields[forCurrency] = accountFieldsInstance;
            return accountFieldsInstance;
        }
    }
}
