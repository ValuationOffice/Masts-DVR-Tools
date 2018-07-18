using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Contract
{
    public interface ICSVManager
    {
        Task WriteZipsToCSVFileAsync(string path, IEnumerable<ZipFile> records);
    }
}
