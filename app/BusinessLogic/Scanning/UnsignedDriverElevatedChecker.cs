using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class UnsignedDriverElevatedChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool UnsignedDriverElevatedFound { get; private set; } = false;

        /* This approach makes a lot of assumptions but its the only way to check
         * if the current user is not running with elevated privileges
         */

        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingUnsignedDriverElevated);

            CheckWmi();

            ScanResult result = new ScanResult();
            result.ScanType = "System Drivers (Elevated)";
            result.DetailedDescription = $"Unsigned drivers pose a significant security risk because they haven't undergone the verification process by Microsoft or another trusted entity. This lack of verification opens the door for malicious drivers that can compromise system integrity, expose sensitive data, or create a foothold for further attacks.";
            if (UnsignedDriverElevatedFound)
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

            EventAggregator.Instance.FireEvent(BlEvents.CheckingUnsignedDriverElevatedCompleted);
        }

        public void CheckWmi()
        {

            // Initialize WMI query to get information about installed drivers
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMv2", "SELECT * FROM Win32_PnPSignedDriver");

            bool foundUnsigned = false;

            // Iterate through each object (driver) returned by WMI query
            foreach (ManagementObject queryObj in searcher.Get())
            {
                // Check if the driver is unsigned
                if (queryObj["IsSigned"] != null && queryObj["IsSigned"].ToString().ToLower() == "false")
                {
                    UnsignedDriverElevatedFound = true;
                    foundUnsigned = true;
                    Console.WriteLine($"Found unsigned driver: {queryObj["DeviceName"]}, Manufacturer: {queryObj["Manufacturer"]}");
                }
            }

            if (!foundUnsigned)
            {
                UnsignedDriverElevatedFound = false;                
            }

        }
    }
}
