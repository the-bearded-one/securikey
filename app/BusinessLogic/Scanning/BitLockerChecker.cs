using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using System.Management;
using System.Security.Principal;

namespace BusinessLogic.Scanning
{
    public class BitLockerChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool IsStatusProtected { get; private set; } = false;
        public bool IsBitLockerSupported { get; private set; } = false;
        public bool IsBitLockerEnabled { get; private set; } = false;

        public const String ID = "SK-07";
        public SecurityCheck SecurityCheck { get; private set; }

        public BitLockerChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingBitLocker);

            ProbeBitLocker();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if ( !IsStatusProtected )
                {
                    if (IsBitLockerSupported && IsBitLockerEnabled)
                    {
                        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                    }
                    else
                    {
                        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                    }
                }
            }


            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingBitLockerCompleted);
        }

        private void ProbeBitLocker()
        {
            try
            {
                if (IsCurrentUserAdmin() == false)
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

                    }
                }

            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
            }


        }

        private string GetWindowsEdition()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                return queryObj["Caption"].ToString();
            }
            return "Unknown";
        }

        private bool IsCurrentUserAdmin()
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

    }


}
