using masts_dvr_tool.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Contract
{
    interface IPDFManager
    {
        Task<IEnumerable<PDFField>> GetPDFields (string filepath, string prefix);
    }
}
