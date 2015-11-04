using IBApi.Errors;
using IBApi.Quotes;

namespace IBApi
{
    public interface IQuoteObserver
    {
        void OnQuote(Quote quote);
        void OnError(Error error);
    }
}