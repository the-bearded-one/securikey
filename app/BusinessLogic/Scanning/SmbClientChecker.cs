using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class SmbClientChecker : IChecker
    {        
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();
        public bool IsServerEnabled { get; private set; } = false;
        public bool IsClientEnabled { get; private set; } = false;
        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-17";
        public SmbClientChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }
        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSmbServerEnabled);
            CheckSMBv1Client(@"SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters");

            if (IsClientEnabled)
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

        private void CheckSMBv1Client(string subKey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    var dependOnService = key?.GetValue("DependOnService") as string[];
                    if (dependOnService != null && Array.Exists(dependOnService, element => element == "MRxSmb10"))
                    {
                        IsClientEnabled = true;
                    }
                    else
                    {
                        IsClientEnabled = true;
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
