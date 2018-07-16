using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Contract
{
    public interface IPasswordGenerator
    {
        string GeneratePassword();
    }
}
