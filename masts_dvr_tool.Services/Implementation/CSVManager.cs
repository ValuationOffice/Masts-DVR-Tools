using DVRTools.Services;
using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Implementation
{
    public class CSVManager : ICSVManager
    {
        private readonly ICSVRepository csvRepository;
        private readonly IIOManager ioManager;

        public CSVManager(ICSVRepository csvRepository, IIOManager ioManager)
        {
            this.csvRepository = csvRepository;
            this.ioManager = ioManager;
        }

        public async Task WriteZipsToCSVFileAsync(string path, IEnumerable<ZipFile> records)
        {
            try
            {
                if (!path.ToLower().EndsWith(".csv"))
                    throw new ArgumentException("Path is not a CSV");
            }

            catch (ArgumentException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"File is not a CSV. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
                throw ex;
            }

            if (!ioManager.FileExists(path))
                ioManager.CreateFile(path);
            try
            {
                if (records == null)
                    throw new ArgumentNullException("Records was null");
                if (!(records.Count() >= 1))
                    throw new ArgumentException("No records");
            }
            catch (ArgumentNullException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"No records found. Records was null. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
                throw;
            }
            catch (ArgumentException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($@"No records found. /n Stack trace {ex}", EventLogEntryType.FailureAudit, 101, 1);
                }
                throw;
            }

            await csvRepository.WriteZipsToCSVFileAsync(path, records);
        }
    }
}
