using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVRTools.Services
{
    public class FileNameManager : IFileNameManager
    {
        public string GenerateRandomFileName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
