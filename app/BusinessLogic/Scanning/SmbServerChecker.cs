using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class SmbServerChecker : IChecker
    {        
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();
        public bool IsServerEnabled { get; private set; } = false;
        
        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-18";
        public SmbServerChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }
        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSmbServerEnabled);
            CheckSMBv1Server(@"SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters");

            if (IsServerEnabled)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
            }
            else
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSmbServerEnabledCompleted);
        }


        private void CheckSMBv1Server(string subKey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    if (key != null)
                    {
                        IsServerEnabled = true;                        
                    }
                    else
                    {
                        IsServerEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;

                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
