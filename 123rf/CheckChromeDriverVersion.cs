using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _123rf
{
    public class CheckChromeDriverVersion
    {
        public async Task<string> CheckVersion()
        {          
            string path = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Google\Chrome\BLBeacon", "version", String.Empty).ToString();
            return path;

        }
            

    }
}
