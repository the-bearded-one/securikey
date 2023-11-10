using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using System.Management;

namespace BusinessLogic.Scanning
{
    public class AntispywareInstalledChecker : IChecker
    {
        public List<string> Products { get; private set; } = new List<string>();
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public const String ID = "SK-26";
        public SecurityCheck SecurityCheck { get; private set; }

        public AntispywareInstalledChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }
        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecurityProductsSpyware);

            Products = QuerySecurityProduct("AntiSpywareProduct");

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (Products.Count() == 0)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecurityProductsSpywareCompleted);

        }

        private List<string> QuerySecurityProduct(string className)
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
