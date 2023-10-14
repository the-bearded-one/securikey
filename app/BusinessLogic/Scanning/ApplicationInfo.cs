using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace BusinessLogic.Scanning
{
    public class ApplicationInfo
    {
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public string Publisher { get; set; }
        public override string ToString()
        {
            string theString = $"DisplayName: {DisplayName}    DisplayVersion: {DisplayVersion}    Publisher: {Publisher}";
            return theString ;
        }
    }
}
