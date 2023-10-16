using System.Runtime.InteropServices;

namespace BusinessLogic.Scanning
{
    public class SecureBootChecker : IChecker
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint GetFirmwareEnvironmentVariable(string lpName, ref Guid lpGuid, IntPtr pBuffer, uint nSize);

        public bool IsSecureBootEnabled { get; private set; } = false;

        public void Scan()
        {

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecureBootEnabled);

            Guid EFI_GLOBAL_VARIABLE = new Guid("8BE4DF61-93CA-11d2-AA0D-00E098032B8C");

            try
            {
                uint result = GetFirmwareEnvironmentVariable("SecureBoot", ref EFI_GLOBAL_VARIABLE, IntPtr.Zero, 0);
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
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecureBootEnabledCompleted);
        }

    }
}
