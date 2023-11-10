using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class PasswordComplexityChecker : IChecker
    {
        
        public bool IsComplexityPolicyEnabled { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

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


        public const String ID = "SK-14";
        public SecurityCheck SecurityCheck { get; private set; }

        public PasswordComplexityChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingPasswordComplexityPolicy);

            CheckComplexityPolicy();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (IsComplexityPolicyEnabled)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPasswordComplexityPolicyCompleted);

        }
        private void CheckComplexityPolicy()
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
            catch(Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
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
