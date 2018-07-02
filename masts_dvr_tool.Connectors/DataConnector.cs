using masts_dvr_tool.Connectors.Contracts;
using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace masts_dvr_tool.Connectors
{
    public class DataConnector<T> : IDataConnector<T>
        where T : IVOAType
    {
        public List<PDFField> Connect(IEnumerable<PDFField> fields, T databaseResult)
        {
            List<PDFField> pdfFieldsFromDatabase = GetPropertiesFromDatabaseResult(databaseResult);

            List<PDFField> joinedValues = fields.Join(pdfFieldsFromDatabase, arg => arg.Name, data => data.Name, (arg, data) => new PDFField()
            {
                Name = arg.Name,
                Value = data.Value != null ? data.Value : String.Empty //Prevent values from being Null
            }).ToList();

            joinedValues.AddRange(fields.Where(x => !pdfFieldsFromDatabase.Any(y => y.Name == x.Name)));

            Regex regex = new Regex(@"\.\d");

            List<PDFField> formattedValues = new List<PDFField>();

            joinedValues.ForEach(

                x =>
                {   //When there is a field with a duplicate name, it appends ".1" to the end of it. 
                    //This checks to see if these exist, then grabs the value and maps it to it.
                    //Needs refactoring to allow for checking when a value is changed on the UI.
                    if (regex.IsMatch(x.Name.Substring(x.Name.Length - 2, 2)))
                    {
                        x.Value = joinedValues.Where(y => y.Name == x.Name.Substring(0, x.Name.Length - 2)).FirstOrDefault().Value;
                    }

                    formattedValues.Add(x);

                });

            return formattedValues.Distinct().ToList();
        }

        private List<PDFField> GetPropertiesFromDatabaseResult(T databaseResult)
        {   //Gets the name and value from Struct/Class and maps it to a PDFField.
            List<PDFField> property = new List<PDFField>();
            property = databaseResult
                    .GetType()
                    .GetProperties()
                    .Select(x => new PDFField()
                    {
                        Name = x.Name,
                        Value = (string)x.GetValue(databaseResult)
                    }).ToList();

            return property;
        }

    }
}
