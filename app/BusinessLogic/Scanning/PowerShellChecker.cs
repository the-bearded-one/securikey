using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BusinessLogic.Scanning
{
    public class PowerShellChecker : IChecker
    {

        public bool HasWeakExecutionPolicy { get; private set; } = false;
        public bool HasExecutionPolicyDefined { get; private set; } = true;
        public bool UnableToQuery { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-01";
        public PowerShellChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {

            ScanResults.Clear();
            
            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicy);
            
            CheckExecutionPolicy(@"SOFTWARE\Policies\Microsoft\Windows\PowerShell", "Local Machine");
            CheckExecutionPolicy(@"SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "Local Machine (Legacy)");
            CheckExecutionPolicy(@"SOFTWARE\Policies\Microsoft\PowerShell", "Current User");
             
            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (ScanResults.Count == 0)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicyCompleted);
        }

        public void CheckExecutionPolicy(string subKey, string scope)
        {
            try
            {
                using (RegistryKey psKey = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    var execPolicy = psKey?.GetValue("ExecutionPolicy");
                    if (execPolicy != null)
                    {
                        string policyString = execPolicy.ToString();

                        if (policyString.Equals("Unrestricted", StringComparison.OrdinalIgnoreCase) ||
                            policyString.Equals("Bypass", StringComparison.OrdinalIgnoreCase))
                        {
                            HasWeakExecutionPolicy = true;

                            ScanResult result = new ScanResult();
                            result.ScanType = "Power Shell";
                            result.Severity = Severity.Medium;
                            result.ShortDescription = $"Power Shell has a weak {scope} execution policy";
                            result.DetailedDescription = $"PowerShell's execution policy is a security feature designed to prevent the execution of potentially malicious scripts. It's advisable to use a more restrictive execution policy to enhance security while considering specific use cases for temporarily relaxing the policy as needed.";
                            ScanResults.Add(result);
                        }
                    }
                    else
                    {
                        HasExecutionPolicyDefined = false;                        
                    }
                }
            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                UnableToQuery = true;
            }
            
        }

    }
}
