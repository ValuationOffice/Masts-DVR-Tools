using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.DataAccess.Contracts
{
    public interface ICSVRepository
    {
        void WriteZipsToCSVFile(string path, IEnumerable<ZipFile> records);
    }
}
