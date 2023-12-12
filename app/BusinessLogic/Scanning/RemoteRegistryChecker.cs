using System;
using System.ServiceProcess;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class RemoteRegistryChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool UsesRemoteRegistry{ get; private set; } = false;

        public const String ID = "SK-29";
        public SecurityCheck SecurityCheck { get; private set; }

        public RemoteRegistryChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

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
            ServiceController remoteRegistryService = new ServiceController("RemoteRegistry");
            try
            {
                // Check if the service is installed
                var status = remoteRegistryService.Status;

                Console.WriteLine("Remote Registry Service is installed.");
                Console.WriteLine($"Service status: {status}");

                // Check if the service is enabled
                if (remoteRegistryService.StartType == ServiceStartMode.Disabled)
                {
                    Console.WriteLine("Remote Registry Service is disabled.");
                }
                else
                {
                    UsesRemoteRegistry = true;
                    Console.WriteLine("Remote Registry Service is enabled.");
                }
            }
            catch (InvalidOperationException ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                Console.WriteLine("Remote Registry Service is not installed.");
            }
        }


    }
}
