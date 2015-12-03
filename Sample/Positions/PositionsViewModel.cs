using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using IBApi;

namespace Sample.Positions
{
    class PositionsViewModel : Screen
    {
        private IObservableCollection<Position> positions;

        public PositionsViewModel(IClient client)
        {
            this.DisplayName = "Positions";
            this.positions = new BindableCollection<Position>();

            foreach (var position in client.Accounts.SelectMany(accounts => accounts.PositionsStorage.Positions))
            {
                this.positions.Add(new Position(position));
            }

            foreach (var account in client.Accounts)
            {
                account.PositionsStorage.PositionAdded += (sender, eventArgs) => this.positions.Add(new Position(eventArgs.Position));
            }
        }

        public IObservableCollection<Position> Positions
        {
            get { return this.positions; }
            set
            {
                if (Equals(value, this.positions)) return;
                this.positions = value;
                this.NotifyOfPropertyChange(() => this.Positions);
            }
        }
    }
}
