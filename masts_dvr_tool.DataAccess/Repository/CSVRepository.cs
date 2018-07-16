using masts_dvr_tool.DataAccess.Contracts;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using masts_dvr_tool.Types;
using System.IO;

namespace masts_dvr_tool.DataAccess.Repository
{
    public class CSVRepository : ICSVRepository
    {
        public async Task WriteZipsToCSVFileAsync(string path, IEnumerable<ZipFile> records)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                CsvWriter csvWriter = new CsvWriter(writer);

                foreach (var record in records)
                {
                    csvWriter.WriteRecord(record);
                    await csvWriter.NextRecordAsync();
                }

               await csvWriter.FlushAsync();
            }

        }

    }
}
