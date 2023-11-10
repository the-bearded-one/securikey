using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using NetFwTypeLib;

namespace BusinessLogic.Scanning
{
    public class FirewallActiveChecker : IChecker
    {
        public bool IsFirewallEnabled { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public const String ID = "SK-27";
        public SecurityCheck SecurityCheck { get; private set; }

        public FirewallActiveChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }
        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingFirewall);

            bool isFirewallEnabled = ProbeFirewall();

            if (isFirewallEnabled)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
            }
            else
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
            }


            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingFirewallCompleted);
        }

        private bool ProbeFirewall()
        {
            try
            {
                Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
                INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);
                return mgr.LocalPolicy.CurrentProfile.FirewallEnabled;
            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
            }
            return false;
        }
    }
}
