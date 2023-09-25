using System;
using System.Runtime.InteropServices;

namespace BusinessLogic
{
    public class InternetConnectionChecker
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public bool IsConnected { get; private set; } = false;

        public bool Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingInternetStatus);

            int description = 0;
            IsConnected = InternetGetConnectedState(out description, 0);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingInternetStatusCompleted);
            return IsConnected;
        }
    }
}