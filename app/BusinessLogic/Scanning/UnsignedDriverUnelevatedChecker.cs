using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class UnsignedDriverUnelevatedChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool UnsignedDriverUnelevatedFound { get; private set; } = false;

        /* This approach makes a lot of assumptions but its the only way to check
         * if the current user is not running with elevated privileges
         */

        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-20";

        public UnsignedDriverUnelevatedChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingUnsignedDriverUnelevated);

            CheckInstallLogs();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UnsignedDriverUnelevatedFound)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingUnsignedDriverUnelevatedCompleted);
        }

        private void CheckInstallLogs()
        {
            try
            {
                string logFilePath = @"C:\Windows\INF\setupapi.dev.log";
                if (File.Exists(logFilePath))
                {
                    string[] lines = File.ReadAllLines(logFilePath);
                    bool foundUnsignedDrivers = false;

                    foreach (string line in lines)
                    {
                        if (line.Contains("Driver not digitally signed"))
                        {
                            UnsignedDriverUnelevatedFound = true;
                        }
                    }

                    if (!foundUnsignedDrivers)
                    {
                        UnsignedDriverUnelevatedFound = false;
                    }
                }
                else
                {
                    UnsignedDriverUnelevatedFound = true;
                }

            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
            }

        }
    }
}
