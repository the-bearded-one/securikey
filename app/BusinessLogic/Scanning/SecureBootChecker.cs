using BusinessLogic.Scanning.POCOs;
using System.Runtime.InteropServices;

namespace BusinessLogic.Scanning
{
    public class SecureBootChecker : IChecker
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint GetFirmwareEnvironmentVariable(string lpName, ref Guid lpGuid, IntPtr pBuffer, uint nSize);

        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool IsSecureBootEnabled { get; private set; } = false;
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecureBootEnabled);

            Guid EFI_GLOBAL_VARIABLE = new Guid("8BE4DF61-93CA-11d2-AA0D-00E098032B8C");

            try
            {
                uint res = GetFirmwareEnvironmentVariable("SecureBoot", ref EFI_GLOBAL_VARIABLE, IntPtr.Zero, 0);
                int errorCode = Marshal.GetLastWin32Error();

                // The function will fail but last error will be ERROR_INVALID_FUNCTION (1) if Secure Boot is not enabled
                if (errorCode == 1)
                {
                    IsSecureBootEnabled = false;

                    ScanResult result = new ScanResult();
                    result.ScanType = "Secure Boot";
                    result.Severity = Severity.High;
                    result.ShortDescription = "You system is not protected by Secure Boot";
                    result.DetailedDescription = $"Secure Boot is an important security feature designed to enhance the integrity and security of the boot process on modern computer systems, particularly those running operating systems like Windows 8 and later";
                    ScanResults.Add(result);
                }
                else
                {
                    IsSecureBootEnabled = true;

                    ScanResult result = new ScanResult();
                    result.ScanType = "Secure Boot";
                    result.Severity = Severity.Ok;
                    result.ShortDescription = "You system is protected by Secure Boot";
                    result.DetailedDescription = $"Secure Boot is an important security feature designed to enhance the integrity and security of the boot process on modern computer systems, particularly those running operating systems like Windows 8 and later";
                    ScanResults.Add(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecureBootEnabledCompleted);
        }

    }
}
