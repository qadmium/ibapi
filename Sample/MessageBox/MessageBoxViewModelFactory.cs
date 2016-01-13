namespace Sample.MessageBox
{
    internal class MessageBoxViewModelFactory
    {
        public MessageBoxViewModel Create(string text)
        {
            return new MessageBoxViewModel(text);
        }
    }
}
