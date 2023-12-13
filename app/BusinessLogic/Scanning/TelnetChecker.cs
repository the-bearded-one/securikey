using System;
using System.ServiceProcess;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class TelnetChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();
        public bool UsesTelnet { get; private set; } = false;

        public const String ID = "SK-30";
        public SecurityCheck SecurityCheck { get; private set; }

        public TelnetChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingTelnet);

            ProbeTelnet();

            if ( SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UsesTelnet)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);
            EventAggregator.Instance.FireEvent(BlEvents.CheckingTelnetCompleted);

        }

        private void ProbeTelnet()
        {
            ServiceController telnetService = new ServiceController("TlntSvr");
            try
            {
                // Check if the service is installed
                var status = telnetService.Status;

                // Check if the service is enabled
                if (telnetService.StartType == ServiceStartMode.Disabled)
                {
                    Console.WriteLine("Telnet Service is disabled.");
                }
                else
                {
                    UsesTelnet = true;
                    Console.WriteLine("Telnet Service is enabled.");
                }
            }
            catch (InvalidOperationException ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                Console.WriteLine("Telnet Service is not installed.");
            }
        }


    }
}
