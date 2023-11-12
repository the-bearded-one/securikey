using System;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class NetBiosChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool UsingNetBios { get; private set; } = false;

        public const String ID = "SK-36";
        public SecurityCheck SecurityCheck { get; private set; }

        public NetBiosChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            

            ProbeNetBios();

            if ( SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UsingNetBios)
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

        private void ProbeNetBios()
        {

            try
            {
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    if (properties.WinsServersAddresses.Count > 0)
                    {
                        UsingNetBios = true;
                    }
                    else
                    {
                        UsingNetBios = false;
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
