using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class RdpChecker : IChecker
    {

        public bool IsRdpEnabled { get; private set; } = false;
        public bool IsRdpWeak { get; private set; } = false;

        public void Scan()
        {

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRdpEnabled);

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

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRdpEnabledCompleted);
        }


    }
}
