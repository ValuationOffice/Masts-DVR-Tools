using masts_dvr_tool.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Services.Implementation
{
    public class PasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword()
        {
            Random random = new Random();

            string values = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!£$%^&*()_+-={}[]:;'@#<>?/,.";

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < 14; i++)
            {
                int index = random.Next(0, values.Length -1);

                stringBuilder.Append(values[index]);
            }
            return stringBuilder.ToString();
        }
    }
}
