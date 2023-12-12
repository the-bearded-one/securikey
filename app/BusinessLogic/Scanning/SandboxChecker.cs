using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class SandboxChecker : IChecker
    {
        
        public bool IsSandboxEnabled { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public const String ID = "SK-34";
        public SecurityCheck SecurityCheck { get; private set; }

        public SandboxChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            CheckSandbox();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (IsSandboxEnabled)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
            }

            SecurityResults.Add(SecurityCheck);

        }
        private void CheckSandbox()
        {
           
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OptionalFeature WHERE Name = 'Containers-DisposableClientVM'");
                foreach (var obj in searcher.Get())
                {
                    var installState = obj["InstallState"];
                    if (installState != null)
                    {
                        // InstallState == 1 means the feature is enabled
                        if (Convert.ToInt32(installState) == 1)
                        {
                            IsSandboxEnabled = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
            }
        }
    }
}
