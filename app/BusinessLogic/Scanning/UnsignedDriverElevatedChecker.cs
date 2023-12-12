using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Scanning.Interfaces;


namespace BusinessLogic.Scanning
{
    public class UnsignedDriverElevatedChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool UnsignedDriverElevatedFound { get; private set; } = false;

        /* This approach makes a lot of assumptions but its the only way to check
         * if the current user is not running with elevated privileges
         */

        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-19";
        public UnsignedDriverElevatedChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingUnsignedDriverElevated);

            CheckWmi();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UnsignedDriverElevatedFound)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingUnsignedDriverElevatedCompleted);
        }

        private void CheckWmi()
        {

            try
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
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
            }

        }
    }
}
