using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using NetFwTypeLib;

namespace BusinessLogic.Scanning
{
    public class FirewallChecker : IChecker
    {
        public bool IsFirewallEnabled { get; private set; } = false;

        public void Scan()
        {

            EventAggregator.Instance.FireEvent(BlEvents.CheckingFirewall);
            Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
            INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);
            bool enabled = mgr.LocalPolicy.CurrentProfile.FirewallEnabled;

            if (enabled)
            {
                IsFirewallEnabled = true;
            }
            EventAggregator.Instance.FireEvent(BlEvents.CheckingFirewallCompleted);
        }
    }
}
