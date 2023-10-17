using BusinessLogic.Scanning.POCOs;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class RdpChecker : IChecker
    {

        public bool IsRdpEnabled { get; private set; } = false;
        public bool IsRdpWeak { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public void Scan()
        {
            ScanResults.Clear();

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

                        ScanResult result = new ScanResult();
                        result.ScanType = "Remote Desktop Protocol";
                        result.Severity = Severity.Ok;
                        result.ShortDescription = "Remote Desktop Protocol (RDP) is disabled";
                        result.DetailedDescription = $"Disabling Remote Desktop Protocol (RDP) when not used can protect against security risks";
                        ScanResults.Add(result);

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

                        ScanResult result = new ScanResult();
                        result.ScanType = "Remote Desktop Protocol";
                        result.Severity = Severity.Low;
                        result.ShortDescription = "Remote Desktop Protocol (RDP) is enabled";
                        result.DetailedDescription = $"Remote Desktop Protocol (RDP) can be vulnerable to security risks if not configured and used properly.";
                        ScanResults.Add(result);
                    }
                    else
                    {
                        IsRdpWeak = true;

                        ScanResult result = new ScanResult();
                        result.ScanType = "Remote Desktop Protocol";
                        result.Severity = Severity.High;
                        result.ShortDescription = "Remote Desktop Protocol (RDP) is enabled";
                        result.DetailedDescription = $"Remote Desktop Protocol (RDP) can be vulnerable to security risks if not configured and used properly. To make RDP safer, you should follow best practices, including using strong, unique passwords, implementing MFA, restricting access, keeping the software up to date, and using VPNs and firewalls to limit exposure to the internet. Additionally, consider alternative remote access solutions if security is a primary concern, especially for critical systems or sensitive data.";
                        ScanResults.Add(result);
                    }
                }
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRdpEnabledCompleted);
        }


    }
}
