using BusinessLogic.Scanning;
using System.Runtime.InteropServices;

namespace BusinessLogic
{
    public class InternetConnectionChecker : IChecker
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public bool IsConnected { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingInternetStatus);

            int description = 0;
            IsConnected = InternetGetConnectedState(out description, 0);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingInternetStatusCompleted);
        }
    }
}