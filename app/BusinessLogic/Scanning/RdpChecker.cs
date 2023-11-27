using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class RdpChecker : IChecker
    {

        public bool IsRdpEnabled { get; private set; } = false;
        public bool IsRdpWeak { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-15";
        public RdpChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();

            try
            {

                EventAggregator.Instance.FireEvent(BlEvents.CheckingRdpEnabled);

                ProbeRdp();

                if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
                {
                    if (IsRdpEnabled || IsRdpWeak)
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
            catch (Exception ex)
            {
                Console.WriteLine("Error: Exception when running RDP Checker");
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRdpEnabledCompleted);
        }

        private void ProbeRdp()
        {
            try
            {

                // Check if RDP is enabled
                string rdpKeyPath = @"SYSTEM\CurrentControlSet\Control\Terminal Server";
                string rdpValueName = "fDenyTSConnections";
                RegistryKey rdpKey = Registry.LocalMachine.OpenSubKey(rdpKeyPath);

                if (rdpKey != null)
                {
                    object rdpValue = rdpKey.GetValue(rdpValueName);
                    if (rdpValue != null && rdpValue is int)
                    {
                        if ((int)rdpValue == 0)
                        {
                            IsRdpEnabled = true;
                        }
                        else
                        {
                            IsRdpEnabled = false;


                            return;  // No point checking security if RDP is disabled
                        }
                    }
                }

                // Check RDP security level
                string securityKeyPath = @"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp";
                string securityValueName = "UserAuthentication";
                RegistryKey securityKey = Registry.LocalMachine.OpenSubKey(securityKeyPath);

                if (securityKey != null)
                {
                    object securityValue = securityKey.GetValue(securityValueName);
                    if (securityValue != null && securityValue is int)
                    {
                        if ((int)securityValue == 1)
                        {
                            IsRdpWeak = false;

                        }
                        else
                        {
                            IsRdpWeak = true;

                        }
                    }
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
