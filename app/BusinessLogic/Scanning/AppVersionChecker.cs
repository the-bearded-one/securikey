using BusinessLogic;
using Microsoft.Win32;
using System;
using System.Text.Json;
using System.IO;
using BusinessLogic.Scanning.POCOs;
using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics;

namespace BusinessLogic.Scanning
{
    public class AppScanner : IChecker
    {
        
        public List<BusinessLogic.Scanning.POCOs.Vulnerability> VulnerabiltiesSeen { get; private set; } = new List<BusinessLogic.Scanning.POCOs.Vulnerability>();

        public bool IsChromeVersionLower(string version1, string version2)
        {
            string[] parts1 = version1.Split('.');
            string[] parts2 = version2.Split('.');

            // Assuming that both versions have the same number of parts
            for (int i = 0; i < parts1.Length; i++)
            {
                int num1 = int.Parse(parts1[i]);
                int num2 = int.Parse(parts2[i]);

                if (num1 < num2)
                {
                    return true;
                }
                else if (num1 > num2)
                {
                    return false;
                }
                // If they are equal, continue to the next part
            }

            // If you reach this point, the versions are equal
            return false;
        }

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingApplicationVersions);


            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "applications.json");

            // Read the file content
            string json = File.ReadAllText(path);

            // Deserialize the JSON content to a list of ApplicationInfo objects
            List<BusinessLogic.Scanning.POCOs.ApplicationInfo> apps = JsonConvert.DeserializeObject<List<BusinessLogic.Scanning.POCOs.ApplicationInfo>>(json);

            string appName = "Google Chrome";

            # region Chrome Checker
            string appVersion = AppVersionChecker.CheckVersion(appName);
            if (appVersion != string.Empty)
            {
                BusinessLogic.Scanning.POCOs.ApplicationInfo appInfo = apps.FirstOrDefault(app => app.Application == appName);

                if (appInfo != null)
                {
                    // Iterate through all the vulnerabilities for Chrome
                    foreach (BusinessLogic.Scanning.POCOs.Vulnerability vulnerability in appInfo.Vulnerabilities)
                    {
                        if (IsChromeVersionLower(appVersion, vulnerability.Version))
                        {
                            VulnerabiltiesSeen.Add(vulnerability);

                        }
                    }
                }
            }
            # endregion

            # region Spotify Checker, not working yet
            appName = "Spotify";
            appVersion = AppVersionChecker.CheckVersion(appName);

            if (appVersion != string.Empty)
            {
                var appInfo = apps.FirstOrDefault(app => app.Application == appName);

                if (appVersion != String.Empty && appInfo != null)
                {
                    // Iterate through all the vulnerabilities for Chrome
                    foreach (BusinessLogic.Scanning.POCOs.Vulnerability vulnerability in appInfo.Vulnerabilities)
                    {
                        if (IsChromeVersionLower(appVersion, vulnerability.Version))
                        {
                            VulnerabiltiesSeen.Add(vulnerability);
                        }
                    }
                }
            }
            #endregion

            #region Firefox Checker
            appName = "Mozilla Firefox";
            appVersion = AppVersionChecker.GetFirefoxVersion();

            if (appVersion != string.Empty)
            {
                var appInfo = apps.FirstOrDefault(app => app.Application == appName);

                if (appVersion != String.Empty && appInfo != null)
                {
                    // Iterate through all the vulnerabilities for Chrome
                    foreach (BusinessLogic.Scanning.POCOs.Vulnerability vulnerability in appInfo.Vulnerabilities)
                    {
                        if (IsChromeVersionLower(appVersion, vulnerability.Version))
                        {
                            VulnerabiltiesSeen.Add(vulnerability);
                        }
                    }
                }
            }
            #endregion

            EventAggregator.Instance.FireEvent(BlEvents.CheckingApplicationVersionsCompleted);

        }
    }

    public static class AppVersionChecker
    {
        public static string CheckVersion(String appName)
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
                        Debug.WriteLine((string)subkey.GetValue("DisplayName"));
                        // Check if the DisplayName is Google Chrome
                        if ((string)subkey.GetValue("DisplayName") == appName)
                        {
                            // Return the version
                            return (string)subkey.GetValue("DisplayVersion");
                        }
                    }
                }
            }

            return string.Empty;
        }


        // Firefox doesn't store its version info in the same place as other apps
        public static string GetFirefoxVersion()
        {
            // Registry path where Firefox details are typically stored
            string registryKey = @"SOFTWARE\Mozilla\Mozilla Firefox";

            // Open the key
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    // Get the current version from the key's default value
                    string currentVersion = key.GetValue(null) as string;
                    if (!string.IsNullOrEmpty(currentVersion))
                    {
                        return currentVersion;
                    }
                }
            }

            return string.Empty;
        }


    }

}
