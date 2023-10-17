using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class PowerShellChecker : IChecker
    {

        public bool HasWeakExecutionPolicy { get; private set; } = false;
        public bool HasExecutionPolicyDefined { get; private set; } = true;
        public bool UnableToQuery { get; private set; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicy);
            CheckExecutionPolicy(@"SOFTWARE\Policies\Microsoft\Windows\PowerShell", "Local Machine");
            CheckExecutionPolicy(@"SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "Local Machine (Legacy)");
            CheckExecutionPolicy(@"SOFTWARE\Policies\Microsoft\PowerShell", "Current User");
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
                UnableToQuery = true;
            }
            
        }

    }
}
