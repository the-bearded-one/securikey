using BusinessLogic.Scanning.POCOs;
using System.Security.Principal;

namespace BusinessLogic.Scanning
{
    public class UserType : IChecker
    {
        public bool IsElevatedUser { get; private set; }
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingElevatedUser);

            // Get the Windows identity of the user running the application
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            // Create a Windows principal object based on that identity
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            // Check if the user is in the Administrator role
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (isAdmin)
            {
                IsElevatedUser = true;

                ScanResult result = new ScanResult();
                result.ScanType = "User Account";
                result.Severity = Severity.Low;
                result.ShortDescription = "You are running in a highly priviledged account";
                result.DetailedDescription = $"Running in an admin account is dangerous because it grants extensive privileges, which can facilitate malware installation and result in unauthorized software installations. This approach makes your system more vulnerable to compromise and exposes sensitive data. To enhance security, it's best to use standard user accounts for everyday tasks and reserve admin privileges for specific, necessary actions.";
                ScanResults.Add(result);
            }
            else
            {
                ScanResult result = new ScanResult();
                result.ScanType = "User Account";
                result.Severity = Severity.Ok;
                result.ShortDescription = "You are protected by using a non-priviledged account";
                result.DetailedDescription = $"Running in an admin account is dangerous because it grants extensive privileges, which can facilitate malware installation and result in unauthorized software installations. This approach makes your system more vulnerable to compromise and exposes sensitive data. To enhance security, it's best to use standard user accounts for everyday tasks and reserve admin privileges for specific, necessary actions.";
                ScanResults.Add(result);

                IsElevatedUser = false;
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingElevatedUserCompleted);
        }

    }
}
