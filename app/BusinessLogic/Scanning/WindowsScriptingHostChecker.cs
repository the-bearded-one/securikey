using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class WindowsScriptingHostChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();
        public bool IsWshEnabled { get; private set; } = false;

        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-02";
        public WindowsScriptingHostChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsScriptingHost);

            ProbeWindowsScriptingHost();

            if ( IsWshEnabled )
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
            }
            else
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
            }


            SecurityResults.Add(SecurityCheck);
            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsScriptingHostCompleted);
        }

        private void ProbeWindowsScriptingHost()
        {

            try
            {

                // Define the registry key path and value name
                string keyPath = @"Software\Microsoft\Windows Script Host\Settings";
                string valueName = "Enabled";

                // Open the registry key
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(keyPath);

                // Check if the key exists
                if (registryKey != null)
                {
                    // Try to get the value
                    object value = registryKey.GetValue(valueName);

                    // Check if the value exists and is set to 0 (indicating WSH is disabled)
                    if (value != null && value is int && (int)value == 0)
                    {
                        IsWshEnabled = false;
                    }
                    else
                    {
                        IsWshEnabled = true;
                    }
                }
                else
                {
                    IsWshEnabled = true;
                }
            } catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
            }
        }

    }
}
