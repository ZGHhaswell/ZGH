using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.ServiceLocation;

namespace WpfTest.Mef
{
    public class MefService : MefBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();
        }

        protected override void ConfigureAggregateCatalog()
        {
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(MainWindow).Assembly));
        }

        protected override void InitializeShell()
        {
            var windows = ServiceLocator.Current.GetInstance<MainWindow>();
            Application.Current.MainWindow = windows;
            windows.ShowDialog();
        }
    }
}
