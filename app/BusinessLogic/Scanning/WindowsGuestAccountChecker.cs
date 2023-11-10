using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BusinessLogic.Scanning
{
    public class WindowsGuestAccountChecker : IChecker
    {

        public bool HasWeakExecutionPolicy { get; private set; } = false;
        public bool HasExecutionPolicyDefined { get; private set; } = true;
        public bool UnableToQuery { get; private set; } = false;

        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-04";
        public WindowsGuestAccountChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {

            ScanResults.Clear();
            
            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsGuestAccountEnabled);

            bool enabled = IsGuestAccountEnabled();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (enabled)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsGuestAccountEnabledCompleted);
        }

        private bool IsGuestAccountEnabled()
        {
            try
            {
                SelectQuery query = new SelectQuery("Win32_UserAccount");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject envVar in searcher.Get())
                {
                    string name = envVar["Name"].ToString();
                    string status = envVar["Status"].ToString();
                    bool disabled = (bool)envVar["Disabled"];

                    if (name.Equals("Guest", StringComparison.OrdinalIgnoreCase))
                    {
                        // Account is found and status is OK means it's not disabled.
                        return status.Equals("OK", StringComparison.OrdinalIgnoreCase) && !disabled;
                    }
                }
            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                // Handle exceptions, possibly logging the error somewhere.
                // Depending on your error handling strategy, you might want to return a default value here.
            }

            // If we reach here, the Guest account was not found, or the check could not be performed.
            return false;
        }

  
    }
}
