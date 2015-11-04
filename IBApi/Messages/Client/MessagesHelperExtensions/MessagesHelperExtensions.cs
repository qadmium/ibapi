namespace IBApi.Messages.Client.MessagesHelperExtensions
{
    internal static class MessagesHelperExtensions
    {
        public static string ToRightString(this bool? call)
        {
            if (!call.HasValue)
            {
                return string.Empty;
            }

            if (call.Value)
            {
                return "C";
            }

            return "P";
        }
    }
}
