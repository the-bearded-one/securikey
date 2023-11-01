using BusinessLogic.Scanning.POCOs;
using System.Management;
using System.Security.Principal;

namespace BusinessLogic.Scanning
{
    public class BitLockerChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool IsStatusProtected { get; private set; } = false;
        public bool IsBitLockerSupported { get; private set; } = false;
        public bool IsBitLockerEnabled { get; private set; } = false;

        public bool RequiresElevatedPrivilege { get; } = false;

        public string GetWindowsEdition()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                return queryObj["Caption"].ToString();
            }
            return "Unknown";
        }

        public bool IsCurrentUserAdmin()
        {
            try
            {
                // Get the identity of the current user
                WindowsIdentity identity = WindowsIdentity.GetCurrent();

                // Create a Windows principal object to represent the current user's role
                WindowsPrincipal principal = new WindowsPrincipal(identity);

                // Check if the current user belongs to the admin group
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                // If an exception occurs, it's safer to assume the user is not an admin
                return false;
            }
        }

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingBitLocker);
            
            if ( IsCurrentUserAdmin() == false )
            {
                IsStatusProtected = true;
            }
            else
            {

                string edition = GetWindowsEdition();
                bool supportsBitLocker = !edition.Contains("Home");

                if (supportsBitLocker == false)
                {
                    IsBitLockerSupported = false;
                    IsBitLockerEnabled = false;
                }
                else
                {
                    IsBitLockerSupported = true;
                    IsBitLockerEnabled = false;

                    ManagementScope scope = new ManagementScope(@"\root\cimv2\Security\MicrosoftVolumeEncryption");
                    ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_EncryptableVolume WHERE DriveLetter = 'C:'");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        uint status = (uint)queryObj["ProtectionStatus"];
                        if (status == 1)
                        {
                            IsBitLockerEnabled = true;
                        }
                    }

                    if (IsBitLockerSupported == true && IsBitLockerEnabled == false)
                    {
                        ScanResult result = new ScanResult();
                        result.ScanType = "BitLocker";
                        result.Severity = Severity.High;
                        result.ShortDescription = "BitLocker is not enabled on your computer";
                        result.DetailedDescription = $"BitLocker was not found to be enabled on your computer. It is recommended to user BitLocker to protect your privacy and data on your computer.";
                        ScanResults.Add(result);
                    }
                    else if (IsBitLockerSupported == true && IsBitLockerEnabled == true)
                    {
                        ScanResult result = new ScanResult();
                        result.ScanType = "BitLocker";
                        result.Severity = Severity.Ok;
                        result.ShortDescription = "Your privacy and data is protected by BitLocker";
                        result.DetailedDescription = $"BitLocker is enabled on your computer. BitLocker protects your privacy and data on your computer through encrpytion.";
                        ScanResults.Add(result);
                    }
                    else
                    {
                        // report nothing since bitlocker is not present on this system
                    }
                }
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingBitLockerCompleted);
        }

    }


}
