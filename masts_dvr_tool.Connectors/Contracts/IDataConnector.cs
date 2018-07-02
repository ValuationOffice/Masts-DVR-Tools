using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Connectors.Contracts
{
    public interface IDataConnector<T> where T: IVOAType
    {
        List<PDFField> Connect(IEnumerable<PDFField> fields, T databaseResult);
    }
}
