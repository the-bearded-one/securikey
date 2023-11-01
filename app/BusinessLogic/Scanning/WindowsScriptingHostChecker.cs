using BusinessLogic.Scanning.POCOs;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class WindowsScriptingHostChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool IsWshEnabled { get; private set; } = false;
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsScriptingHost);

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

                    ScanResult result = new ScanResult();
                    result.ScanType = "Windows Script Host";
                    result.Severity = Severity.Ok;
                    result.ShortDescription = "Improved system security by disabling Windows Script Host";
                    result.DetailedDescription = $"Windows Script Host (WSH) can pose security risks when used improperly or when scripts are executed without adequate security measures in place. WSH is allows users to run scripts written in scripting languages like VBScript and JScript.";
                    ScanResults.Add(result);
                }
                else
                {
                    IsWshEnabled = true;

                    ScanResult result = new ScanResult();
                    result.ScanType = "Windows Script Host";
                    result.Severity = Severity.Medium;
                    result.ShortDescription = "Windows Script Host is Enabled";
                    result.DetailedDescription = $"Windows Script Host (WSH) can pose security risks when used improperly or when scripts are executed without adequate security measures in place. WSH is allows users to run scripts written in scripting languages like VBScript and JScript.";
                    ScanResults.Add(result);
                }
            }
            else
            {
                IsWshEnabled = true;
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsScriptingHostCompleted);
        }


    }
}
