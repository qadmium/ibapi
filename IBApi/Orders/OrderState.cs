namespace IBApi.Orders
{
    public enum OrderState
    {
        Invalid,
        New,
        Filled,
        Cancelled,
        Submitted,
        PreSubmitted,
        Ignored,
        Inactive,
        PendingSubmit,
        PendingCancel,
        Restored,
        PendingUpdate,
        Rejected
    }
}