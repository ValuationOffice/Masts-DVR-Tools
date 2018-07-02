using masts_dvr_tool.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.Types;
using System.Collections.Generic;
using System;
using System.Linq;
using DVRTools.Services;
using masts_dvr_tool.Services.Implementation;
using masts_dvr_tool.Connectors;

namespace masts_dvr_tool.Tests.ViewModels
{
    [TestClass]
    public class MainWindowViewModelTests
    {

        private MainWindowViewModel mainWindowViewModel;
        private IEnumerable<PDFField> getPDFFieldsResponse;

        [TestInitialize]
        public void TestSetup()
        {

            var mockPDFManager = new Mock<IPDFManager>();

            getPDFFieldsResponse = new List<PDFField>()
            {
                new PDFField()
                {
                    Name = "VOAFieldOne"
                },

                 new PDFField()
                {
                    Name = "VOAFieldTwo"
                },

                  new PDFField()
                {
                    Name = "VOAFieldThree"
                }
            };

            mockPDFManager.Setup(x => x.GetPDFields(String.Empty, String.Empty)).ReturnsAsync(getPDFFieldsResponse);

            FileNameManager fileNameManager = new FileNameManager();
            DataManager dataManager = new DataManager(null, null);
            DataConnector<IVOAType> dataConnector = new DataConnector<IVOAType>();

            mainWindowViewModel = new MainWindowViewModel(mockPDFManager.Object, new IOManager(fileNameManager), fileNameManager, dataManager, dataConnector);
            mainWindowViewModel.Prefix = "";
            mainWindowViewModel.TemplatePath = "";
        }

        [TestMethod]
        public void GetPDFButton_ShouldBeDisabled_WhenListIsNotEmpty()
        {
            mainWindowViewModel.Prefix = "Foo";
            mainWindowViewModel.TemplatePath = "Foo";
            Assert.AreEqual(true, mainWindowViewModel.GetPDFEnabled);
            mainWindowViewModel.PDFFields = getPDFFieldsResponse.ToList();
            Assert.AreEqual(false, mainWindowViewModel.GetPDFEnabled);
            mainWindowViewModel.PDFFields = new List<PDFField>();
            Assert.AreEqual(true, mainWindowViewModel.GetPDFEnabled);
        }

        [TestMethod]
        public void GetPDFButton_ShouldBeDisabled_WhenPrefixAndTemplatePathNullOrEmpty()
        {
            Assert.AreEqual(false, mainWindowViewModel.GetPDFEnabled);
            mainWindowViewModel.Prefix = "Foo";
            Assert.AreEqual(false, mainWindowViewModel.GetPDFEnabled);
            mainWindowViewModel.TemplatePath = "Foo";
            Assert.AreEqual(true, mainWindowViewModel.GetPDFEnabled);
            mainWindowViewModel.TemplatePath = "";
            Assert.AreEqual(false, mainWindowViewModel.GetPDFEnabled);

        }

        [TestMethod]
        public void UpdatePDFButton_ShouldBeDisabled_WhenNoValuesWithinPDFField()
        {
            Assert.AreEqual(false, mainWindowViewModel.UpdatePDFEnabled);
            mainWindowViewModel.Prefix = "Foo";
            mainWindowViewModel.TemplatePath = "Foo";
            Assert.AreEqual(false, mainWindowViewModel.UpdatePDFEnabled);
            mainWindowViewModel.OutputPath = "asdasd";
            Assert.AreEqual(false, mainWindowViewModel.UpdatePDFEnabled);
            mainWindowViewModel.PDFFields = new List<PDFField>();
            Assert.AreEqual(false, mainWindowViewModel.UpdatePDFEnabled);
            mainWindowViewModel.PDFFields = new List<PDFField>()
            {
                new PDFField()
                {
                    Name="Foo",
                    Value ="Foo"
                }
            };
            Assert.AreEqual(true, mainWindowViewModel.UpdatePDFEnabled);
        }

        [TestMethod]
        public void UpdatePDFButton_ShouldBeDisabled_WhenNoOutputPath()
        {
            Assert.AreEqual(false, mainWindowViewModel.UpdatePDFEnabled);
            mainWindowViewModel.PDFFields = new List<PDFField>()
            {
                new PDFField()
                {
                    Name="Foo",
                    Value ="Foo"
                }
            };

            Assert.AreEqual(false, mainWindowViewModel.UpdatePDFEnabled);
            mainWindowViewModel.OutputPath = @"C:/";
            Assert.AreEqual(true, mainWindowViewModel.UpdatePDFEnabled);
        }

    }
}
