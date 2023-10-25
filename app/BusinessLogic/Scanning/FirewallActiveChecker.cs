using BusinessLogic.Scanning.POCOs;
using NetFwTypeLib;

namespace BusinessLogic.Scanning
{
    public class FirewallActiveChecker : IChecker
    {
        public bool IsFirewallEnabled { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingFirewall);

            Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
            INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);
            bool enabled = mgr.LocalPolicy.CurrentProfile.FirewallEnabled;

            if (enabled)
            {
                IsFirewallEnabled = true;

                ScanResult result = new ScanResult();
                result.ScanType = "Firewall";
                result.Severity = Severity.Medium;
                result.ShortDescription = "No active Firewall found";
                result.DetailedDescription = $"Firewalls act as a barrier between your network and potential threats from the internet. They can block unauthorized access and protect your internal network from malicious external entities, such as hackers, malware, and viruses.";
                ScanResults.Add(result);
            }
            else
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Firewall";
                result.Severity = Severity.Ok;
                result.ShortDescription = "You network activity is being protected by a firewall";
                result.DetailedDescription = $"Firewalls act as a barrier between your network and potential threats from the internet. They can block unauthorized access and protect your internal network from malicious external entities, such as hackers, malware, and viruses.";
                ScanResults.Add(result);
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingFirewallCompleted);
        }
    }
}
