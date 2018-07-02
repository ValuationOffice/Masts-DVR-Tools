using masts_dvr_tool.Models;
using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Contract
{
    public interface IDataManager
    {
        IVOAType DatabaseValues { get; set; }
        MainWindow<IVOAType> MainWindow { get; set; }
    }
}
