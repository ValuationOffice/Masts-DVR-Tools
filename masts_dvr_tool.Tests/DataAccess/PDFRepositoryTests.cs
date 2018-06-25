using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.DataAccess.Repository;
using masts_dvr_tool.Models;
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

            mockPDFFilePath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName}/StubForms/mockForm.pdf";
        }

        [TestMethod]
        public void GetPDFFields_ShouldReturn_FieldsFromDocument()
        {
            List<PDFField> pdfActualItems = pdfRepository.GetPDFFields(mockPDFFilePath, PREFIX).ToList();
            Assert.AreEqual(pdfExpectedItems.Count(), pdfActualItems.Count());
            Assert.AreEqual(pdfExpectedItems.Count(), pdfExpectedItems.Select(x => x.Name).Intersect(pdfActualItems.Select(x => x.Name)).Count());
        }

        [TestMethod]
        public void UpdatePDFFields_ShouldCreate_PDFWithDataFromUI()
        {
            List<PDFField> pdfActualItems = pdfRepository.GetPDFFields(mockPDFFilePath, PREFIX).ToList();
            Assert.AreEqual(pdfExpectedItems.Count(), pdfExpectedItems.Select(x => x.Name).Intersect(pdfActualItems.Select(x => x.Name)).Count());
            //All Values should be empty strings
            Assert.AreEqual(pdfExpectedItems.Count(), pdfExpectedItems.Where(x => String.IsNullOrWhiteSpace(x.Value)).Count());

            List<PDFField> pdfUpdatedItems = new List<PDFField>();

            pdfExpectedItems.ToList().ForEach(
                x =>
                {
                    if(String.IsNullOrEmpty(x.Value) || String.IsNullOrWhiteSpace(x.Value))
                    {
                        x.Value = "Hello";
                        pdfUpdatedItems.Add(x);
                    }
                }


                );

            //Implement file name service to generate this from client
            string mockOutputPDFFilePath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName}/StubForms/{Guid.NewGuid()}.pdf";

            pdfRepository.UpdatePDFFields(mockPDFFilePath, PREFIX, mockOutputPDFFilePath, pdfUpdatedItems);

            List<PDFField> pdfOutputActualItems = pdfRepository.GetPDFFields(mockOutputPDFFilePath,String.Empty).ToList();
            //When the new form is created, it randomly creates 1-2 bits of meta data with IText branding.
            Assert.AreEqual(1, pdfOutputActualItems.Where(x=>x.Name.StartsWith("Non")).Count());

            //IO Garbage collection
            if (File.Exists(mockOutputPDFFilePath))
            {
                File.Delete(mockOutputPDFFilePath);
            }

        }

    }
}
