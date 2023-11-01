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
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicy);

            CheckPagefileEncryption(@"SYSTEM\CurrentControlSet\Control\FileSystem");

            ScanResult result = new ScanResult();
            result.ScanType = "Secure Defaults";
            result.DetailedDescription = $"Encrypting the page file enhances data security by making it more difficult for attackers to harvest sensitive information that might be written to disk from RAM. Without encryption, data remnants like passwords or encryption keys stored in the page file could be accessible to unauthorized users, creating a potential attack vector for data exfiltration or system compromise.";
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
