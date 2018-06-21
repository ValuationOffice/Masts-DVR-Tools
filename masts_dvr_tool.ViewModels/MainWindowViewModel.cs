using masts_dvr_tool.Commands;
using masts_dvr_tool.Models;
using masts_dvr_tool.Models.DTO;
using masts_dvr_tool.Services.Implementation;
using masts_dvr_tool.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.ViewModels
{
    public class MainWindowViewModel:BindableBase
    {
        private readonly PDFManager pdfManager;
        private MainWindow mainWindow;

        public MainWindowViewModel(PDFManager pdfManager)
        {
            mainWindow = new MainWindow();
            this.pdfManager = pdfManager;
        }
        
        public List<PDFField> PDFFields
        {
            get => mainWindow.PDFFields;
            set
            {
                if (ReferenceEquals(value, mainWindow.PDFFields))
                {
                    return;
                }

                mainWindow.PDFFields = value;
                this.OnPropertyChanged();
            }
        }

        public string TemplatePath
        {
            get => mainWindow.TemplatePath;
            set
            {
                if (value == mainWindow.TemplatePath)
                {
                    return;
                }

                mainWindow.TemplatePath = value;
                this.OnPropertyChanged();
            }
        }

        public string Prefix
        {
            get => mainWindow.Prefix;
            set
            {
                if (value == mainWindow.Prefix)
                {
                    return;
                }

                mainWindow.Prefix = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand GetPDFCommand => new RelayCommand(GetPDF);

        private void GetPDF()
        {
            PDFFields = pdfManager.GetPDFields(TemplatePath, Prefix).Result.ToList();
        }


    }
}
