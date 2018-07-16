using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using masts_dvr_tool.DataAccess.Repository;
using masts_dvr_tool.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace masts_dvr_tool.Tests.DataAccess
{
    [TestClass]
    public class CSVRepositoryTests
    {
        [TestMethod]
        public async System.Threading.Tasks.Task WriteZipsToCSVFile_ShouldWriteToCSVAsync()
        {
            CSVRepository csvRepository = new CSVRepository();

            string filepath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName}/StubForms/mockCSV.csv";

            //if (File.Exists(filepath))
            //    File.Delete(filepath);

            //Will overwrite existing file
            //Not reusing filestream as implementation handles IO
            var file = File.Create(filepath);
            file.Close();
          
            List<ZipFile> records = new List<ZipFile>()
            {
                new ZipFile
                {
                    FilePath = "C:/Camera",
                    Password = "Foo"
                },

                 new ZipFile
                {
                    FilePath = "C:/Camera/asd",
                    Password = "FooBar"
                },

                  new ZipFile
                {
                    FilePath = "C:/Camera/adasd",
                    Password = "FooBag"
                },
            };

            await csvRepository.WriteZipsToCSVFileAsync(filepath, records);

            List<string[]> csvResult = ReadCSV(filepath);

            Assert.AreEqual(3, csvResult.Count);

            for (int i = 0; i < records.Count; i++)
            {
                for (int y = 0; y < csvResult.Count; y++)
                {
                    Assert.AreEqual(records[i].FilePath, csvResult[y][0]);
                    Assert.AreEqual(records[i].Password, csvResult[y][1]);
                    i++;
                }
            }

            if (File.Exists(filepath))
                File.Delete(filepath);
        }

        private List<string[]> ReadCSV(string filepath)
        {
            List<string[]> result = new List<string[]>();
            using (StreamReader reader = new StreamReader(filepath))
            {
                char[] separator = { ',' };

                string data;

                while ((data = reader.ReadLine()) != null)
                {
                    result.Add(data.Split(separator, StringSplitOptions.RemoveEmptyEntries));
                }
            }

            return result;
        }

    }
}
