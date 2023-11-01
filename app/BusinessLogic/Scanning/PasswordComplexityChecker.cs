using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class PasswordComplexityChecker : IChecker
    {
        
        public bool IsComplexityPolicyEnabled { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool RequiresElevatedPrivilege { get; } = false;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct USER_MODALS_INFO_0
        {
            public uint usrmod0_min_passwd_len;
            public uint usrmod0_max_passwd_age;
            public uint usrmod0_min_passwd_age;
            public uint usrmod0_force_logoff;
            public uint usrmod0_password_hist_len;
        }

        [DllImport("Netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int NetUserModalsGet(
            [MarshalAs(UnmanagedType.LPWStr)] string server,
            int level,
            out IntPtr bufptr);

        [DllImport("Netapi32.dll", SetLastError = true)]
        public static extern int NetApiBufferFree(IntPtr buffer);
        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingPasswordComplexityPolicy);

            CheckComplexityPolicy();

            ScanResult result = new ScanResult();
            result.ScanType = "Windows Policy";
            result.DetailedDescription = $"Enforcing a password complexity policy makes it more difficult for attackers to guess or brute-force passwords, thereby enhancing account security. Complex passwords, which include a mix of characters, numbers, and symbols, are less susceptible to common attack techniques like dictionary attacks, significantly reducing the risk of unauthorized access.";
            if (!IsComplexityPolicyEnabled)
            {
                result.Severity = Severity.Medium;
                result.ShortDescription = $"Password complexity policy is not enabled!";
            }
            else
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"Password complexity policy is enabled!";
            }
            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPasswordComplexityPolicyCompleted);

        }
        public void CheckComplexityPolicy()
        {
            IntPtr pBuffer = IntPtr.Zero;

            try
            {
                int result = NetUserModalsGet(null, 0, out pBuffer);
                if (result == 0 && pBuffer != IntPtr.Zero)
                {
                    USER_MODALS_INFO_0 modals = (USER_MODALS_INFO_0)Marshal.PtrToStructure(pBuffer, typeof(USER_MODALS_INFO_0));

                    if (modals.usrmod0_min_passwd_len > 0)
                    {
                        IsComplexityPolicyEnabled = true;
                        Console.WriteLine($"Password complexity is enabled. Minimum password length is {modals.usrmod0_min_passwd_len} characters.");
                    }
                    else
                    {
                        IsComplexityPolicyEnabled = false;                        
                    }
                }
                else
                {
                    IsComplexityPolicyEnabled = false;
                    Console.WriteLine($"Failed to get user modal information. Error code: {result}");
                }
            }
            finally
            {
                if (pBuffer != IntPtr.Zero)
                {
                    NetApiBufferFree(pBuffer);
                }
            }
        }
    }
}
