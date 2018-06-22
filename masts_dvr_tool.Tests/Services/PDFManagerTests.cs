using System;
using System.Collections.Generic;
using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using masts_dvr_tool.Models;
using masts_dvr_tool.Services.Contract;

namespace masts_dvr_tool.Tests.Services
{
    [TestClass]
    public class PDFManagerTests
    {
        private IEnumerable<PDFField> expectedItems;
        private IPDFManager pdfManager;

        [TestInitialize]
        public void TestSetup()
        {
            expectedItems = new List<PDFField>()
            {

                new PDFField()
                {
                    Name = "VOATestField"
                },

                 new PDFField()
                {
                     Name = "VOAMockField"
                },

                  new PDFField()
                {
                      Name = "VOASillyField"
                }

            };


            var mockPDFRepository = new Mock<IPDFRepository>();

            mockPDFRepository.Setup(x => x.GetPDFFields(String.Empty, String.Empty)).Returns(expectedItems);

            pdfManager = new PDFManager(mockPDFRepository.Object);
        }

        [TestMethod]
        public void GetPDFFields_ShouldReturn_FieldsFromRepositoryAsync()
        {
            List<PDFField> actualFields = pdfManager.GetPDFields(String.Empty, String.Empty).Result.ToList();
            Assert.AreEqual(expectedItems.Count(), actualFields.Count());
            Assert.AreEqual(expectedItems.Count(), expectedItems.Select(x => x.Name).Intersect(actualFields.Select(x => x.Name)).Count());
        }

    }
}
