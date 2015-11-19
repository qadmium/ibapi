using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using IBApi;
using Sample.Accounts;
using Sample.Orders;
using Sample.Positions;

namespace Sample.Shell
{
    class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        public ShellViewModel(IClient client, AccountsViewModel accounts, PositionsViewModel positions, OrdersViewModel orders)
        {
            this.Items.Add(accounts);
            this.Items.Add(positions);
            this.Items.Add(orders);
        }
    }
}
