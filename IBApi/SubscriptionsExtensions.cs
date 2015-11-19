using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace IBApi
{
    internal static class SubscriptionsExtensions
    {
        public static void Unsubscribe(this ICollection<IDisposable> subscriptions)
        {
            Contract.Requires(subscriptions != null);

            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }

            subscriptions.Clear();
        }
    }
}
