using System;
using System.DirectoryServices;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class NoPasswordExpiryChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool IsPasswordVulnerable { get; private set; } = false;

        public const String ID = "SK-12";
        public SecurityCheck SecurityCheck { get; private set; }

        public NoPasswordExpiryChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingPasswordExpiry);

            IsPasswordVulnerable = CheckPasswordExpiry();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (IsPasswordVulnerable)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

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
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return true;
        }
        
    }
}
