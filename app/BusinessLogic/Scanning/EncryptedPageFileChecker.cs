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

        public List<ScanResult> ScanResults => new List<ScanResult>();
        public bool IsPageFileEncrypted { get; private set; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicy);

            CheckPagefileEncryption(@"SYSTEM\CurrentControlSet\Control\FileSystem");

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
