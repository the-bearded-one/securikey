using System;
using System.ServiceProcess;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class RemoteManagementChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool UsesRemoteRegistry{ get; private set; } = false;

        public const String ID = "SK-31";
        public SecurityCheck SecurityCheck { get; private set; }

        public RemoteManagementChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingRemoteRegistry);

            ProbeRemoteRegistry();

            if ( SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UsesRemoteRegistry)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);
            EventAggregator.Instance.FireEvent(BlEvents.CheckingRemoteRegistryCompleted);

        }

        private void ProbeRemoteRegistry()
        {
            ServiceController winrmService = new ServiceController("WinRM");
            try
            {
                // Check if the service is installed
                var status = winrmService.Status;

                // Check if the service is enabled
                if (winrmService.StartType == ServiceStartMode.Disabled)
                {
                    Console.WriteLine("Windows Remote Management (WinRM) Service is disabled.");
                }
                else
                {
                    Console.WriteLine("Windows Remote Management (WinRM) Service is enabled.");
                }
            }
            catch (InvalidOperationException ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                // If the service is not installed, an exception will be thrown
                Console.WriteLine("Windows Remote Management (WinRM) Service is not installed on this machine.");
            }

        }


    }
}
