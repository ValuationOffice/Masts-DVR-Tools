using masts_dvr_tool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Contract
{
    interface IPDFManager
    {
        Task<IEnumerable<PDFField>> GetPDFields (string filepath, string prefix);
    }
}
