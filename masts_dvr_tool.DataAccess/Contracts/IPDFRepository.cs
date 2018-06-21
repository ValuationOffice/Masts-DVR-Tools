using masts_dvr_tool.Models;
using System.Collections.Generic;

namespace masts_dvr_tool.DataAccess.Contracts
{
    public interface IPDFRepository
    {
        IEnumerable<PDFField> GetPDFFields(string filePath, string prefix);
    }
}
