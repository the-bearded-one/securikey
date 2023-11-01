using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class HeartbleedChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool IsVulnerable { get; private set; } = false;
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingHeartbleed);

            IsVulnerable = CheckVulnerableOpenSsl();

            ScanResult result = new ScanResult();
            result.ScanType = "Application Software";
            result.DetailedDescription = $"Not having your pagefile encrypted poses a risk of sensitive data leakage. The pagefile can store confidential information like passwords or encryption keys, and without encryption, an attacker with physical access can recover this data, compromising your system's security.";
            if ( !IsVulnerable )
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"Vulnerable OpenSSL not found.";
            }
            else 
            {
                result.Severity = Severity.High;
                result.ShortDescription = $"Vulnerable version of OpenSSL found!";
            }
            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingHeartbleedCompleted);

        }

        private bool CheckVulnerableOpenSsl()
        {
            try
            {
                string[] commonPaths = new string[]
                       {
                            @"C:\Program Files\OpenSSL\",
                            @"C:\Program Files (x86)\OpenSSL\",
                            @"C:\Windows\System32\",
                           // Add other common directories where OpenSSL might be installed
                       };

                string[] dllNames = new string[]
                {
                    "libeay32.dll",
                    "ssleay32.dll",
                    "libssl.dll",
                    "libcrypto.dll"
                };

                foreach (var path in commonPaths)
                {
                    foreach (var dll in dllNames)
                    {
                        string fullPath = Path.Combine(path, dll);
                        if (File.Exists(fullPath))
                        {

                            // Read the version information
                            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fullPath);

                            // Check if the version is known to be vulnerable to Heartbleed
                            if (IsVulnerableToHeartbleed(versionInfo.FileVersion))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }

        private bool IsVulnerableToHeartbleed(string version)
        {
            string[] vulnerableVersions = new string[]
            {
            "1.0.1",
            "1.0.1a",
            "1.0.1b",
            "1.0.1c",
            "1.0.1d",
            "1.0.1e",
            "1.0.1f"
            };

            foreach (var vulnerableVersion in vulnerableVersions)
            {
                if (version.StartsWith(vulnerableVersion))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
