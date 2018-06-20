using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.DataAccess.Repository;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services
{
    public static class ServiceConfiguration
    {

        public static IKernel Container { get; } = new StandardKernel();

        public static void ConfigureContainer()
        {
            //Bindings here
            Container.Bind<IPDFRepository>().To<PDFRepository>().InSingletonScope();
        }
    }
}
