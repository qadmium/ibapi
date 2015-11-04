using IBApi.Errors;

namespace IBApi
{
    public interface IMarketDepthObserver
    {
        void OnError(Error error);
        void OnAskUpdate(int row, MarketDepthUpdate update);
        void OnBidUpdate(int row, MarketDepthUpdate update);
        void OnAskRemove(int row);
        void OnBidRemove(int row);
    }
}