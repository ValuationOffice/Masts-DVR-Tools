using masts_dvr_tool.Types;
using System.Collections.Generic;

namespace masts_dvr_tool.Models
{
    public class MainWindow<T> where T: IVOAType
    {
        public List<PDFField> PDFFields { get; set; }

        public string TemplatePath {get; set;}

        public string OutputPath { get; set; }
        
        public string Prefix { get; set; }

        public T DataType { get; set; }

        /// <summary>
        /// Files that need to be added.
        /// </summary>
        public string ExternalFilePath { get; set; }
    }
}
