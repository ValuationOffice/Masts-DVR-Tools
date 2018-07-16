using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DVRTools.Services;
using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.DataAccess.Repository;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.Services.Implementation;
using masts_dvr_tool.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace masts_dvr_tool.Tests.Services
{
    [TestClass]
    public class CSVManagerTests
    {
        private ICSVManager csvManager;

        private readonly string badFilePath = @"C:/Camera/badfile.txt";
        
        private readonly List<ZipFile> records = new List<ZipFile>()
        {
            new ZipFile()
            {
                FilePath = "C:/Camera",
                Password = "asdjhasdjhkasdjk"
            }
        };

        private readonly List<ZipFile> nullRecords = null;
        private readonly List<ZipFile> emptyRecords = new List<ZipFile>();

        [TestInitialize]
        public void TestSetup()
        {
            IFileNameManager fileNameManager = new FileNameManager();
            IIOManager ioManager = new IOManager(fileNameManager);
            ICSVRepository csvRepository = new CSVRepository();
            csvManager = new CSVManager(csvRepository,ioManager);
            
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CSVManager_ShouldThrowArguementException_WhenNotCSVAsync()
        {
            await csvManager.WriteZipsToCSVFileAsync(badFilePath, records);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task CSVManager_ShouldThrowArguementException_WhenNullRecords()
        {
            string filePath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName}/StubForms/temporaryCSV.csv";
            FileStream fileStream = File.Create(filePath);
            fileStream.Dispose();
            try
            {
                await csvManager.WriteZipsToCSVFileAsync(filePath, nullRecords);
            }
            catch
            {
                throw;
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CSVManager_ShouldThrowArguementException_WhenNoRecords()
        {
            string filePath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName}/StubForms/temporaryCSV.csv";
            FileStream fileStream = File.Create(filePath);
            fileStream.Dispose();
            try
            {
                await csvManager.WriteZipsToCSVFileAsync(filePath, emptyRecords);
            }
            catch
            {
                throw;
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}
