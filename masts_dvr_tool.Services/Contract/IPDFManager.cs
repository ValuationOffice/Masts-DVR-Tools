using masts_dvr_tool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Contract
{
    public interface IPDFManager
    {
        Task<IEnumerable<PDFField>> GetPDFields (string filepath, string prefix);
        Task UpdatePDFFields(string filepath, string prefix, string outputLocation, IEnumerable<PDFField> pdfFields);
    }
}
