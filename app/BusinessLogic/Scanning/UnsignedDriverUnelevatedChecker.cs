using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class UnsignedDriverUnelevatedChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool UnsignedDriverUnelevatedFound { get; private set; } = false;

        /* This approach makes a lot of assumptions but its the only way to check
         * if the current user is not running with elevated privileges
         */

        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingUnsignedDriverUnelevated);

            CheckInstallLogs();

            ScanResult result = new ScanResult();
            result.ScanType = "System Drivers (Unelevated)";
            result.DetailedDescription = $"Unsigned drivers pose a significant security risk because they haven't undergone the verification process by Microsoft or another trusted entity. This lack of verification opens the door for malicious drivers that can compromise system integrity, expose sensitive data, or create a foothold for further attacks.";
            if (UnsignedDriverUnelevatedFound)
            {
                result.Severity = Severity.Medium;
                result.ShortDescription = $"Possible unsigned Windows driver(s) found!";
            }
            else
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"No unsigned Windows driver(s) found.";

            }

            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingUnsignedDriverUnelevatedCompleted);
        }

        public void CheckInstallLogs()
        {

            string logFilePath = @"C:\Windows\INF\setupapi.dev.log";
            if (File.Exists(logFilePath))
            {
                string[] lines = File.ReadAllLines(logFilePath);
                bool foundUnsignedDrivers = false;

                foreach (string line in lines)
                {
                    if (line.Contains("Driver not digitally signed"))
                    {
                        UnsignedDriverUnelevatedFound = true;
                    }
                }

                if (!foundUnsignedDrivers)
                {
                    UnsignedDriverUnelevatedFound = false;
                }
            }
            else
            {
                UnsignedDriverUnelevatedFound = true;
            }

        }
    }
}
