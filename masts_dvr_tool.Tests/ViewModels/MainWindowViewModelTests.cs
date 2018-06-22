﻿using masts_dvr_tool.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.Models;
using System.Collections.Generic;
using System;
using System.Linq;

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

            mainWindowViewModel = new MainWindowViewModel(mockPDFManager.Object);
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
        
    }
}
