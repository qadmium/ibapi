using System;
using System.Collections.Generic;

namespace IBApi
{
    internal static class SubscriptionsExtensions
    {
        public static void Unsubscribe(this ICollection<IDisposable> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }

            subscriptions.Clear();
        }
    }
}
