using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class BitLockerChecker : IChecker
    {
        public bool IsStatusProtected { get; private set; } = false;
        public bool IsBitLockerSupported { get; private set; } = false;
        public bool IsBitLockerEnabled { get; private set; } = false;

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

            EventAggregator.Instance.FireEvent(BlEvents.CheckingBitLocker);
            
            if ( IsCurrentUserAdmin() == false )
            {
                IsStatusProtected = true;
                return;
            }


            string edition = GetWindowsEdition();
            bool supportsBitLocker = !edition.Contains("Home");

            if ( supportsBitLocker == false )
            {
                IsBitLockerSupported = false;
                IsBitLockerEnabled = false;
                return;
            }
            else
            {
                IsBitLockerSupported = true;
            }

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
            IsBitLockerEnabled = false;

            EventAggregator.Instance.FireEvent(BlEvents.CheckingBitLockerCompleted);
        }

    }


}
