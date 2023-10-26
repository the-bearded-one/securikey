using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BusinessLogic.Scanning
{
    public class EncryptedPageFileChecker : IChecker
    {

        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool IsPageFileEncrypted { get; private set; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicy);

            CheckPagefileEncryption(@"SYSTEM\CurrentControlSet\Control\FileSystem");

            ScanResult result = new ScanResult();
            result.ScanType = "Secure Defaults";
            result.DetailedDescription = $"Having a Heartbleed-vulnerable version of OpenSSL exposes sensitive data like private keys and user credentials, making your system highly susceptible to unauthorized access and data breaches. It's a critical vulnerability that can lead to catastrophic security incidents.";
            if ( IsPageFileEncrypted )
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"Windows PageFile is encrypted.";
            }
            else 
            {
                result.Severity = Severity.Low;
                result.ShortDescription = $"Windows PageFile is not encrypted";
            }
            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicyCompleted);
        }

        public void CheckPagefileEncryption(string subKey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    var isPagefileEncrypted = key?.GetValue("NtfsEncryptPagingFile");

                    if (isPagefileEncrypted != null && isPagefileEncrypted.ToString() == "1")
                    {
                        IsPageFileEncrypted = true;
                    }
                    else
                    {
                        IsPageFileEncrypted = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
