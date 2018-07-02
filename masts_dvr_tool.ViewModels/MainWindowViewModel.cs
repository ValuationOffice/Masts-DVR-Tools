using masts_dvr_tool.Commands;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.ViewModels.Helpers;
using System.Collections.Generic;
using System;
using DVRTools.Services;
using masts_dvr_tool.Models;
using masts_dvr_tool.Types;
using masts_dvr_tool.Connectors.Contracts;
#if DEBUG
using System.IO;
#endif
using System.Linq;

namespace masts_dvr_tool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IPDFManager pdfManager;
        private readonly IIOManager ioManager;
        private readonly IFileNameManager fileNameManager;
        private readonly IDataManager dataManager;
        private readonly IDataConnector<IVOAType> dataConnector;
        private MainWindow<IVOAType> mainWindow;
        private bool getPDFEnabled;
        private bool updatePDFEnabled;

        public MainWindowViewModel(IPDFManager pdfManager, IIOManager ioManager, IFileNameManager fileNameManager, IDataManager dataManager, IDataConnector<IVOAType> dataConnector)
        {
            mainWindow = dataManager.MainWindow;
            this.pdfManager = pdfManager;
            this.ioManager = ioManager;
            this.fileNameManager = fileNameManager;
            this.dataManager = dataManager;
            this.dataConnector = dataConnector;

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
                this.OnPropertyChanged(nameof(this.UpdatePDFEnabled));
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

        public string OutputPath
        {
            get => mainWindow.OutputPath;
            set
            {
                if (value == mainWindow.OutputPath)
                {
                    return;
                }

                mainWindow.OutputPath = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.UpdatePDFEnabled));
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

        public bool UpdatePDFEnabled
        {
            get
            {
                if (PDFFields == null)
                    updatePDFEnabled = false;
                else if (PDFFields.Count() >= 1 && !(String.IsNullOrEmpty(OutputPath) || String.IsNullOrWhiteSpace(OutputPath)))
                    updatePDFEnabled = true;

                return this.updatePDFEnabled;
            }
            set
            {
                if (value == this.updatePDFEnabled)
                {
                    return;
                }

                this.updatePDFEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand GetPDFCommand => new RelayCommand(GetPDF);

        public RelayCommand OutputPathCommand => new RelayCommand(ChooseOutputPath);

        public RelayCommand TemplatePathCommand => new RelayCommand(ChooseTemplatePath);

        public RelayCommand UpdatePDFCommand => new RelayCommand(UpdatePDF);

        private async void GetPDF()
        {
            var result = await pdfManager.GetPDFields(TemplatePath, Prefix);
            PDFFields = result.ToList();

            //Only execute if database is in use.
            if (mainWindow.DataType != null)
                PDFFields = dataConnector.Connect(PDFFields, mainWindow.DataType);

            OnPropertyChanged(nameof(PDFFields));
        }

        public void ChooseTemplatePath()
        {
            TemplatePath = ioManager.OpenFileDialog();
        }

        private void ChooseOutputPath()
        {
            OutputPath = ioManager.OpenFolder();
        }


        public async void UpdatePDF()
        {
            string filePath = $"{OutputPath}/{fileNameManager.GenerateRandomFileName()}";

            ioManager.CreateDirectory(filePath);

            await pdfManager.UpdatePDFFields(TemplatePath, "VOA", $"{filePath}/{fileNameManager.GenerateRandomFileName()}.pdf", PDFFields);

            //Only execute if there is an external file.
            if (!(String.IsNullOrEmpty(mainWindow.ExternalFilePath) || String.IsNullOrWhiteSpace(mainWindow.ExternalFilePath)))
                ioManager.CopyFileToDirectory(mainWindow.ExternalFilePath, filePath);

            ioManager.ZipDirectory(filePath, OutputPath);

            ioManager.DeleteDirectory(filePath);
        }

    }
}
