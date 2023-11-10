using System;
using System.ServiceProcess;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class SpoolerChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();
        public bool UsesSpooler { get; private set; } = false;

        public const String ID = "SK-06";
        public SecurityCheck SecurityCheck { get; private set; }

        public SpoolerChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingTelnet);

            ProbeSpooler();

            if ( SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UsesSpooler)
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

        private void ProbeSpooler()
        {
            ServiceController spoolerService = new ServiceController("Spooler");
            try
            {
                // Check if the service is installed
                var status = spoolerService.Status;

                // Check if the service is enabled
                if (spoolerService.StartType == ServiceStartMode.Disabled)
                {
                    Console.WriteLine("Telnet Service is disabled.");
                }
                else
                {
                    UsesSpooler = true;
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
