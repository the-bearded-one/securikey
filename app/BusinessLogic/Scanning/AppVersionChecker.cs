using Microsoft.Win32;
using System;

namespace AppVersionChecker
{
    public static class Scanner
    {
        public static class Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingApplicationVersions);

            string chromeVersion = ChromeChecker.GetChromeVersion();
            Console.WriteLine("Checking for Chrome", chromeVersion);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingApplicationVersionsCompleted);

        }
    }
    public static class ChromeChecker
    {
        public static string GetChromeVersion()
        {
            // Registry path where Chrome details are stored
            string registryKey = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            // Open the key
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                // Loop through all subkeys
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                    {
                        // Check if the DisplayName is Google Chrome
                        if ((string)subkey.GetValue("DisplayName") == "Google Chrome")
                        {
                            // Return the version
                            return (string)subkey.GetValue("DisplayVersion");
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}
