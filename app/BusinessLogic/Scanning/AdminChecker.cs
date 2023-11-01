using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class AdminChecker : IChecker
    {

        public bool IsElevatedUser { get; private set; }
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingElevatedUser);
            IsElevatedUser = IsCurrentUserAdmin();

            ScanResult result = new ScanResult();
            result.ScanType = "User Account";
            result.DetailedDescription = $"Running in an admin account is dangerous because it grants extensive privileges, which can facilitate malware installation and result in unauthorized software installations. This approach makes your system more vulnerable to compromise and exposes sensitive data. To enhance security, it's best to use standard user accounts for everyday tasks and reserve admin privileges for specific, necessary actions.";
            if (IsElevatedUser)
            {
                result.Severity = Severity.High;
                result.ShortDescription = "You are running in a highly privileged account";
            }
            else
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = "You are protected by using a non-privileged account";
                result.DetailedDescription = $"Running in an admin account is dangerous because it grants extensive privileges, which can facilitate malware installation and result in unauthorized software installations. This approach makes your system more vulnerable to compromise and exposes sensitive data. To enhance security, it's best to use standard user accounts for everyday tasks and reserve admin privileges for specific, necessary actions.";
            }

            ScanResults.Add(result);


            EventAggregator.Instance.FireEvent(BlEvents.CheckingElevatedUserCompleted);
        }

        public bool IsCurrentUserAdmin()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "net",
                Arguments = "user " + Environment.UserName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"
            };

            string output = "";
            using (var process = Process.Start(psi))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            return output.Contains("Administrators");
        }


    }
}
