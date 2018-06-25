using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVRTools.Services
{
    public interface IFileNameManager
    {
        string GenerateRandomFileName();
    }
}
