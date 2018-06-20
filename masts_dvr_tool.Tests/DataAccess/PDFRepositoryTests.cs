using System.Collections.Generic;
using System.IO;
using System.Linq;
using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.DataAccess.Repository;
using masts_dvr_tool.Models.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace masts_dvr_tool.Tests.DataAccess
{
    [TestClass]
    public class PDFRepositoryTests
    {

        private readonly IPDFRepository pdfRepository = new PDFRepository();
        private IEnumerable<PDFField> pdfExpectedItems;
        private const string PREFIX = "VOA";
        private string mockPDFFilePath;

        [TestInitialize]
        public void TestSetup()
        {
            pdfExpectedItems = new List<PDFField>()
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

            mockPDFFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            mockPDFFilePath = Path.GetFullPath(Path.Combine(mockPDFFilePath, @"..\"));

            mockPDFFilePath += @"masts_dvr.tool.Stubs\StubForms\mockForm.pdf";
        }

        [TestMethod]
        public void GetPDFFields_ShouldReturn_FieldsFromDocument()
        {
            List<PDFField> pdfActualItems = pdfRepository.GetPDFFields(mockPDFFilePath, PREFIX).ToList();
            Assert.AreEqual(pdfExpectedItems.Count(), pdfActualItems.Count());
            Assert.AreEqual(pdfExpectedItems.Count(), pdfExpectedItems.Select(x => x.Name).Intersect(pdfActualItems.Select(x=>x.Name)).Count());
        }

    }
}
