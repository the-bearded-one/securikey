using BusinessLogic;
using Microsoft.Win32;
using System;
using System.Text.Json;
using System.IO;
using BusinessLogic.Scanning.POCOs;
using Newtonsoft.Json;
using System.Linq;

namespace BusinessLogic.Scanning
{
    public class AppScanner : IChecker
    {

        public bool IsChromeVulnerable { get; private set; } = false;

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

            string chromeVersion = ChromeChecker.GetChromeVersion();
            Console.WriteLine("Checking for Chrome", chromeVersion);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "applications.json");

            // Read the file content
            string json = File.ReadAllText(path);

            // Deserialize the JSON content to a list of ApplicationInfo objects
            List<BusinessLogic.Scanning.POCOs.ApplicationInfo> apps = JsonConvert.DeserializeObject<List<BusinessLogic.Scanning.POCOs.ApplicationInfo>>(json);

            BusinessLogic.Scanning.POCOs.ApplicationInfo chromeInfo = apps.FirstOrDefault(app => app.Application == "Chrome");

            if (chromeInfo != null)
            {
                // Iterate through all the vulnerabilities for Chrome
                foreach (BusinessLogic.Scanning.POCOs.Vulnerability vulnerability in chromeInfo.Vulnerabilities)
                {
                    if (IsChromeVersionLower(chromeVersion, vulnerability.Version))
                    {
                        IsChromeVulnerable = true;
                    }
                }
            }

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
