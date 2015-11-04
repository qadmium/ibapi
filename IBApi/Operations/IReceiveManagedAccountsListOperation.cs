namespace IBApi.Operations
{
    internal interface IReceiveManagedAccountsListOperation : IOperation
    {
        string[] AccountsList { get; }
    }
}