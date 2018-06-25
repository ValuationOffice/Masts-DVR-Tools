using iText.Forms;
using iText.Kernel.Pdf;
using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace masts_dvr_tool.DataAccess.Repository
{
    public class PDFRepository : IPDFRepository
    {
        public IEnumerable<PDFField> GetPDFFields(string filePath, string prefix)
        {

            using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(filePath)))
            {
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, false);

                List<PDFField> pdfFields = new List<PDFField>();

                form.GetFormFields().AsParallel().ForAll(
                    f =>
                    {
                        if (f.Key.StartsWith(prefix))
                        {
                            pdfFields.Add(

                                new PDFField()
                                {
                                    Name = f.Key,
                                    Value = String.Empty
                                }
                                );

                        }
                    }

                    );

                return pdfFields;

            }

        }

        public void UpdatePDFFields(string filePath, string prefix, string outputLocation, IEnumerable<PDFField> pdfFields)
        {
            using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(filePath), new PdfWriter(outputLocation)))
            {

                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
               
                //Async causes crash
                foreach (var value in pdfFields)
                {
                    form.GetField(value.Name).SetValue(value.Value);
                    form.PartialFormFlattening(value.Name);
                }

                //Flattens all fields OR Flattens all fields marked to be flattened
                form.FlattenFields();
            }

        }
    }
}
