using BusinessLogic.Scanning.Interfaces;
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
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool IsPageFileEncrypted { get; private set; } = false;
       

        public const String ID = "SK-09";
        public SecurityCheck SecurityCheck { get; private set; }

        public EncryptedPageFileChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicy);

            CheckPagefileEncryption(@"SYSTEM\CurrentControlSet\Control\FileSystem");

            if (IsPageFileEncrypted)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
            }
            else
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPowerShellExecutionPolicyCompleted);
        }

        private void CheckPagefileEncryption(string subKey)
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
