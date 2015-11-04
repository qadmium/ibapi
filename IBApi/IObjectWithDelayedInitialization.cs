namespace IBApi
{
    internal delegate void InitializedEventHandler();

    internal interface IObjectWithDelayedInitialization
    {
        event InitializedEventHandler Initialized;

        bool IsInitialized { get; }
    }
}