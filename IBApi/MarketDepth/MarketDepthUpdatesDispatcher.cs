using System;
using IBApi.Messages.Server;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.MarketDepth
{
    internal class MarketDepthUpdatesDispatcher
    {
        public MarketDepthUpdatesDispatcher(IMarketDepthObserver observer)
        {
            CodeContract.Requires(observer != null);
            this.observer = observer;
        }

        public void OnMarketDepth(MarketDepthMessage message)
        {
            switch (message.Operation)
            {
                case MarketDepthOperation.Insert:
                case MarketDepthOperation.Update:
                    ProcessUpdate(message);
                    return;

                case MarketDepthOperation.Delete:
                    ProcessDelete(message);
                    return;
            }

            var errorMessage = string.Format("This kind of operation not supported: {0}", (int)message.Operation);
            throw new ArgumentOutOfRangeException("message", errorMessage);
        }

        private void ProcessUpdate(MarketDepthMessage message)
        {
            var update = new MarketDepthUpdate {Price = message.Price, Quantity = message.Size};
            if (message.BidSide)
            {
                observer.OnBidUpdate(message.Position, update);
                return;
            }

            observer.OnAskUpdate(message.Position, update);
        }

        private void ProcessDelete(MarketDepthMessage message)
        {
            if (message.BidSide)
            {
                observer.OnBidRemove(message.Position);
                return;
            }
            
            observer.OnAskRemove(message.Position);
        }

        private readonly IMarketDepthObserver observer;
    }
}