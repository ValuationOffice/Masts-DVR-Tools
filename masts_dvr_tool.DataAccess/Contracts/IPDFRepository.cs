using masts_dvr_tool.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.DataAccess.Contracts
{
    public interface IPDFRepository
    {
        IEnumerable<PDFField> GetPDFFields(string filePath, string prefix);
    }
}
