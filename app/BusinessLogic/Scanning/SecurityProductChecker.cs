using BusinessLogic.Scanning.POCOs;

namespace BusinessLogic.Scanning
{
    public class SecurityProductChecker : IChecker
    {
        public List<string> AntivirusProducts { get; private set; } = new List<string>();
        public List<string> AntispywareProducts { get; private set; } = new List<string>();
        public List<string> FirewallProducts { get; private set; } = new List<string>();
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecurityProducts);

            AntivirusProducts = SecurityProductInfo.QuerySecurityProduct("AntiVirusProduct");
            AntispywareProducts = SecurityProductInfo.QuerySecurityProduct("AntiSpywareProduct");
            FirewallProducts = SecurityProductInfo.QuerySecurityProduct("FirewallProduct");

            if (AntivirusProducts.Count() == 0)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Security Software";
                result.Severity = Severity.Medium;
                result.ShortDescription = "You are not protected by Antivirus";
                result.DetailedDescription = $"Antivirus software is important for a variety of reasons, primarily related to the protection of computer systems and data from malicious software and cyber threats.";
                ScanResults.Add(result);

                Console.WriteLine("AntiVirus Detected: None!");
            }
            foreach (string product in AntivirusProducts)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Security Software";
                result.Severity = Severity.Ok;
                result.ShortDescription = $"You are protected by Antivirus software, {product}";
                result.DetailedDescription = $"Antivirus software is important for a variety of reasons, primarily related to the protection of computer systems and data from malicious software and cyber threats.";
                ScanResults.Add(result);

                Console.WriteLine(product);
            }


            if (AntispywareProducts.Count() == 0)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Security Software";
                result.Severity = Severity.Medium;
                result.ShortDescription = "You are not protected by Antispyware software";
                result.DetailedDescription = $"Antispyware software is important for several reasons, as it plays a crucial role in protecting computers and networks from spyware, a specific category of malicious software designed to gather information about a user or organization without their knowledge or consent.";
                ScanResults.Add(result);

                Console.WriteLine("AntiSpyware Detected: None!");
            }
            foreach (string product in AntispywareProducts)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Security Software";
                result.Severity = Severity.Ok;
                result.ShortDescription = $"You are protected by Antispyware software, {product}";
                result.DetailedDescription = $"Antispyware software is important for several reasons, as it plays a crucial role in protecting computers and networks from spyware, a specific category of malicious software designed to gather information about a user or organization without their knowledge or consent.";
                ScanResults.Add(result);

                Console.WriteLine(product);
            }


            if (FirewallProducts.Count() == 0)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Security Software";
                result.Severity = Severity.Medium;
                result.ShortDescription = $"You are not protected by firewall software";
                result.DetailedDescription = $"Firewalls are critically important for network and computer security for several reasons. They act as a barrier between a trusted internal network and untrusted external networks, such as the internet.";
                ScanResults.Add(result);

                Console.WriteLine("Firewall Detected: None!");
            }
            foreach (string product in FirewallProducts)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Security Software";
                result.Severity = Severity.Ok;
                result.ShortDescription = $"You are protected by firewall software, {product}";
                result.DetailedDescription = $"Firewalls are critically important for network and computer security for several reasons. They act as a barrier between a trusted internal network and untrusted external networks, such as the internet.";
                ScanResults.Add(result);

                Console.WriteLine(product);
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecurityProductsCompleted);
        }
    }
}
