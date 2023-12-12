using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using System.Management;

namespace BusinessLogic.Scanning
{
    public class AntivirusInstalledChecker : IChecker
    {
        public List<string> AntivirusProducts { get; private set; } = new List<string>();
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public SecurityCheck SecurityCheck { get; private set; }
        
        public const String ID = "SK-24";
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public AntivirusInstalledChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecurityProductsAntivirus);

            AntivirusProducts = QuerySecurityProduct("AntiVirusProduct");

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (AntivirusProducts.Count() == 0)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecurityProductsAntivirusCompleted);
        }

        public List<string> QuerySecurityProduct(string className)
        {
            List<string> detectedProducts = new List<string>();

            try
            {
                string wmiNamespace = @"\\.\root\SecurityCenter2";
                string query = $"SELECT * FROM {className}";

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiNamespace, query);

                foreach (ManagementObject item in searcher.Get())
                {
                    string displayName = item["displayName"].ToString();
                    detectedProducts.Add($"{className} Detected: {displayName}");
                }

            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
            }
            return detectedProducts;
        }
    }
}
