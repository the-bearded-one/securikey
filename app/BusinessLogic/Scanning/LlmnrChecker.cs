using System;
using System.ServiceProcess;
using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class LlmnrChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool UsingLlmnr{ get; private set; } = false;

        public const String ID = "SK-37";
        public SecurityCheck SecurityCheck { get; private set; }

        public LlmnrChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            ProbeLlmrn();

            if ( SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UsingLlmnr)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);
            

        }

        private void ProbeLlmrn()
        {

            const string keyPath = @"SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";
            const string valueName = "EnableMulticast";

            try
            {                
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(valueName);
                        if (value != null && value is int && (int)value == 0)
                        {
                            UsingLlmnr = false;
                        }
                        else
                        {
                            UsingLlmnr = true;                            
                        }
                    }
                    else
                    {
                        UsingLlmnr = true;
                        Console.WriteLine("LLMNR setting not explicitly set (default: enabled).");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                // If the service is not installed, an exception will be thrown
                
            }

        }


    }
}
