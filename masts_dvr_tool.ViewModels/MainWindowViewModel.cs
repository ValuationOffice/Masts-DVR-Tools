using masts_dvr_tool.Commands;
using masts_dvr_tool.Models;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.Services.Implementation;
using masts_dvr_tool.ViewModels.Helpers;
using System.Collections.Generic;
using System;
#if DEBUG
using System.IO;
#endif
using System.Linq;

namespace masts_dvr_tool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IPDFManager pdfManager;
        private MainWindow mainWindow;
        private bool getPDFEnabled;

        public MainWindowViewModel(IPDFManager pdfManager)
        {
            mainWindow = new MainWindow();
            this.pdfManager = pdfManager;

#if DEBUG

            Prefix = "VOA";
            TemplatePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            TemplatePath = Path.GetFullPath(Path.Combine(TemplatePath, @"..\"));

            TemplatePath += @"masts_dvr_tool.Tests\StubForms\mockForm.pdf";
#endif

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
                this.OnPropertyChanged(nameof(this.GetPDFEnabled));
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
                this.OnPropertyChanged(nameof(this.GetPDFEnabled));
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
                this.OnPropertyChanged(nameof(this.GetPDFEnabled));
            }
        }

        public bool GetPDFEnabled
        {
            get
            {
                getPDFEnabled = (PDFFields == null || !(PDFFields.Count > 0)) && !(String.IsNullOrEmpty(TemplatePath) || String.IsNullOrEmpty(Prefix));
                return this.getPDFEnabled;
            }
            set
            {
                if (value == this.getPDFEnabled)
                {
                    return;
                }

                this.getPDFEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand GetPDFCommand => new RelayCommand(GetPDF);

        private async void GetPDF()
        {
            var result = await pdfManager.GetPDFields(TemplatePath, Prefix);
            PDFFields = result.ToList();
            OnPropertyChanged(nameof(PDFFields));
        }

    }
}
