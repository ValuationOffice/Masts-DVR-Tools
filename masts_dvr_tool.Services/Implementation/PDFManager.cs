﻿using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.Models.DTO;
using masts_dvr_tool.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Implementation
{
    public class PDFManager : IPDFManager
    {
        private readonly IPDFRepository pdfRepository;

        public PDFManager(IPDFRepository pdfRepository)
        {
            this.pdfRepository = pdfRepository;
        }

        public async Task<IEnumerable<PDFField>> GetPDFields(string filepath, string prefix)
        {
            var task = Task.Run(() => pdfRepository.GetPDFFields(filepath, prefix));

            return await task;
        }
    }
}
