using System;
using System.DirectoryServices;

namespace BusinessLogic.Scanning
{
    public class NoPasswordExpiryChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool IsPasswordVulnerable { get; private set; } = false;
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingPasswordExpiry);

            IsPasswordVulnerable = CheckPasswordExpiry();

            ScanResult result = new ScanResult();
            result.ScanType = "Secure Defaults";
            result.DetailedDescription = $"Having a non-expiring password is a security risk because it increases the likelihood of unauthorized access over time. It gives attackers an indefinite window to crack the password, and it negates the benefits of regular password changes as a preventive measure against potential compromises.";
            if (IsPasswordVulnerable)
            {
                result.Severity = Severity.Medium;
                result.ShortDescription = $"Non-expiring password is enabled!";
            }
            else
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"Password is not set to be non-expiring";
            }
            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPasswordExpiryCompleted);

        }

        private bool CheckPasswordExpiry()
        {
            try
            {
                string userName = Environment.UserName;
                string domainName = Environment.UserDomainName;
                string path = (domainName == Environment.MachineName)
                    ? $"WinNT://{Environment.MachineName}/{userName}, user"  // Local account
                    : $"WinNT://{domainName}/{userName}, user";  // Domain account

                using (DirectoryEntry userEntry = new DirectoryEntry(path))
                {
                    int flags = (int)userEntry.Properties["UserFlags"].Value;

                    // 0x10000 is the ADS_UF_DONT_EXPIRE_PASSWD flag.
                    IsPasswordVulnerable = (flags & 0x10000) != 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return true;
        }
        
    }
}
