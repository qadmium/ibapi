using IBApi.Messages.Server;

namespace IBApi.Accounts
{
    internal struct AccountValue
    {
        private AccountValue(string accountName, string key, string currency, string value)
        {
            AccountName = accountName;
            Key = key;
            Currency = currency;
            Value = value;
        }

        public readonly string AccountName;
        public readonly string Key;
        public readonly string Currency;
        public readonly string Value;

        public static AccountValue FromMessage(AccountValueMessage message)
        {
            return new AccountValue(message.AccountName, message.Key, message.Currency, message.Value);
        }
    }
}
