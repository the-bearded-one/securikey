using System;
using System.Management;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class NetworkDiscoveryChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool UsingNetworkDiscovery{ get; private set; } = false;

        public const String ID = "SK-39";
        public SecurityCheck SecurityCheck { get; private set; }

        public NetworkDiscoveryChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            

            ProbeNetworkDiscovery();

            if ( SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UsingNetworkDiscovery)
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

        private void ProbeNetworkDiscovery()
        {

            try
            {
                var scope = new ManagementScope("\\\\.\\root\\CIMV2");
                scope.Connect();

                var query = new ObjectQuery("SELECT * FROM Win32_NetworkConnection"); // Corrected WMI class

                using (var searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        try
                        {
                            if (queryObj["NetworkDiscoveryEnabled"] != null &&
                                bool.TryParse(queryObj["NetworkDiscoveryEnabled"].ToString(), out bool isNetworkDiscoveryEnabled) &&
                                isNetworkDiscoveryEnabled)
                            {
                                UsingNetworkDiscovery = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Network Discovery Exception: {queryObj.ToString()} {ex.ToString()}");
                        }
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
