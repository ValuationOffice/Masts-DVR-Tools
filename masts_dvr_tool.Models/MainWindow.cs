using System.Collections.Generic;

namespace masts_dvr_tool.Models
{
    public class MainWindow
    {
        public List<PDFField> PDFFields { get; set; }

        public string TemplatePath {get; set;}
        
        public string Prefix { get; set; }

    }
}
