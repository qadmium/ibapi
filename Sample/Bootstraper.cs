using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using Caliburn.Micro.Autofac;
using IBApi;
using Sample.Connection;
using Sample.Shell;

namespace Sample
{
    class Bootstraper : AutofacBootstrapper<ConnectionViewModel>
    {
        public Bootstraper()
        {
            LogManager.GetLog = type => new DebugLog(type);
            this.Initialize();
        }

        protected override void ConfigureBootstrapper()
        {
            base.ConfigureBootstrapper();

            this.EnforceNamespaceConvention = false;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterInstance(new WindowManager()).As<IWindowManager>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var windowManager = this.Container.Resolve<IWindowManager>();
            var connectionViewModel = this.Container.Resolve<ConnectionViewModel>();

            connectionViewModel.OnConnected = client => this.GuardCloseAndOpenMain(connectionViewModel, windowManager, client);

            windowManager.ShowWindow(connectionViewModel);
        }

        private void GuardCloseAndOpenMain(ConnectionViewModel connection, IWindowManager windowManager, IClient client)
        {
            this.Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            connection.TryClose();

            this.Application.ShutdownMode = ShutdownMode.OnLastWindowClose;

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(client);
            containerBuilder.Update(this.Container);

            windowManager.ShowWindow(this.Container.Resolve<ShellViewModel>());
        }
    }
}
