using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using System.Runtime.InteropServices;

namespace BusinessLogic.Scanning
{
    public class SecureBootChecker : IChecker
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint GetFirmwareEnvironmentVariable(string lpName, ref Guid lpGuid, IntPtr pBuffer, uint nSize);

        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool IsSecureBootEnabled { get; private set; } = false;
        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-16"; // 16
        public SecureBootChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecureBootEnabled);

            ProbeSecureBoot();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (IsSecureBootEnabled)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecureBootEnabledCompleted);
        }

        private void ProbeSecureBoot()
        {
            Guid EFI_GLOBAL_VARIABLE = new Guid("8BE4DF61-93CA-11d2-AA0D-00E098032B8C");

            try
            {
                uint res = GetFirmwareEnvironmentVariable("SecureBoot", ref EFI_GLOBAL_VARIABLE, IntPtr.Zero, 0);
                int errorCode = Marshal.GetLastWin32Error();

                // The function will fail but last error will be ERROR_INVALID_FUNCTION (1) if Secure Boot is not enabled
                if (errorCode == 1)
                {
                    IsSecureBootEnabled = false;

                }
                else
                {
                    IsSecureBootEnabled = true;

                }
            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

    }
}
