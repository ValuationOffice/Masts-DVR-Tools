using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.DataAccess.Repository;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.Services.Implementation;
using masts_dvr_tool.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace masts_dvr_tool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();

        }

        private IKernel Container { get; } = new StandardKernel();

        private void ConfigureContainer()
        {
            //Bindings here
            Container.Bind<IPDFRepository>().To<PDFRepository>().InSingletonScope();
            Container.Bind<IPDFManager>().To<PDFManager>().InSingletonScope();
        }

        private void ComposeObjects()
        {
            //View binding here
            Current.MainWindow = this.Container.Get<MainWindow>();
            Current.MainWindow.DataContext = this.Container.Get<MainWindowViewModel>();
            Current.MainWindow.Show();

        }

    }
}
