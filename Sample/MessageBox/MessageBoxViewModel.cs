using Caliburn.Micro;

namespace Sample.MessageBox
{
    internal sealed class MessageBoxViewModel : Screen
    {
        private string text;

        public MessageBoxViewModel(string text)
        {
            this.Text = text;
            this.DisplayName = string.Empty;
        }

        public string Text
        {
            get { return this.text; }
            set
            {
                if (value == this.text) return;
                this.text = value;
                this.NotifyOfPropertyChange(() => this.Text);
            }
        }

        public void Ok()
        {
            this.TryClose();
        }
    }
}
