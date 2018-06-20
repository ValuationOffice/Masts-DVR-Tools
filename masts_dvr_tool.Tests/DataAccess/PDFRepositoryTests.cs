using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.DataAccess.Repository;
using masts_dvr_tool.Models.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


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
            List <PDFField> pdfActualItems = pdfRepository.GetPDFFields(mockPDFFilePath, PREFIX).ToList();
            Assert.AreEqual(pdfExpectedItems.Count(), pdfActualItems.Count());

            bool[] isTrue = new bool[3];

            for(int i =0; i<pdfExpectedItems.Count();i++)
            {
                foreach (var item in pdfActualItems)
                {
                    if (item.Name == pdfExpectedItems.ToList()[0].Name)
                        isTrue[i] = true;
                }
            }

            foreach(var value in isTrue)
            {
                if (value != true)
                    Assert.Fail();
            }
            
        }

    }
}
