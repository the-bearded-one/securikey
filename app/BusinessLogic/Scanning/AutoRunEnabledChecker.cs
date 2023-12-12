using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class AutoRunEnabledChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool IsAutoRunEnabled { get; private set; } = false;

        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-08";
        public AutoRunEnabledChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingAutoRunEnabled);

            CheckAutoRun(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (IsAutoRunEnabled)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingNtlmV1EnabledComplete);

        }

        private void CheckAutoRun(string subKey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    var noDriveTypeAutoRun = key?.GetValue("NoDriveTypeAutoRun");

                    if (noDriveTypeAutoRun != null)
                    {
                        int value = Convert.ToInt32(noDriveTypeAutoRun);
                        // Checking if the 4th bit is 0 (which would mean AutoRun is enabled for removable drives)
                        if ((value & 0x08) == 0)
                        {
                            IsAutoRunEnabled = true;
                        }
                        else
                        {
                            IsAutoRunEnabled = false;
                        }
                    }
                    else
                    {
                        // no explicit registry key was defined which means it defaults to standard behavior.
                        // on windows 10/11, that means AutoRun is off
                        IsAutoRunEnabled = false;                        
                    }
                }
            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;

                Console.WriteLine($"An error occurred: {ex.Message}");
            };

        }


    }
}
