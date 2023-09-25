using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class SecurityProductChecker
    {
        public List<string> AntivirusProducts {  get; private set; } = new List<string>();
        public List<string> AntispywareProducts { get; private set; } = new List<string>();
        public List<string> FirewallProducts { get; private set; } = new List<string>();

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecurityProducts);

            AntivirusProducts = SecurityProductInfo.QuerySecurityProduct("AntiVirusProduct");
            AntispywareProducts = SecurityProductInfo.QuerySecurityProduct("AntiSpywareProduct");
            FirewallProducts = SecurityProductInfo.QuerySecurityProduct("FirewallProduct");

            if (AntivirusProducts.Count() == 0)
            {
                Console.WriteLine("AntiVirus Detected: None!");
            }
            foreach (string product in AntivirusProducts)
            {
                Console.WriteLine(product);
            }


            if (AntispywareProducts.Count() == 0)
            {
                Console.WriteLine("AntiSpyware Detected: None!");
            }
            foreach (string product in AntispywareProducts)
            {
                Console.WriteLine(product);
            }


            if (FirewallProducts.Count() == 0)
            {
                Console.WriteLine("Firewall Detected: None!");
            }
            foreach (string product in FirewallProducts)
            {
                Console.WriteLine(product);
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSecurityProductsCompleted);
        }
    }
}
