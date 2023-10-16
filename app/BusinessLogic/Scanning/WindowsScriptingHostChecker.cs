using System;
using System.Management;
using System.Collections.Generic;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class WindowsScriptingHostChecker : IChecker
    {

        public bool IsWshEnabled { get; private set; } = false;

        public void Scan()
        {

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

            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsScriptingHostCompleted);
        }


    }
}
