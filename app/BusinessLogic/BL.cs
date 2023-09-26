using BusinessLogic.Scanning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic
{
    /// <summary>
    /// The one and only Business Logic
    /// </summary>
    public class BL
    {
        static private BL _instance = null;

        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        /// <summary>
        /// The one and only BL. Force singleton pattern by hiding default constructor
        /// </summary>
        private BL()
        {
            backgroundWorker.DoWork += OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
        }

        public EventAggregator EventAggregator { get => EventAggregator.Instance; }
        public CveChecker CveChecker { get; private set; } = new CveChecker();
        public InternetConnectionChecker InternetConnectionChecker { get; private set; } = new InternetConnectionChecker();
        public SecurityProductChecker SecurityProductChecker { get; private set; } = new SecurityProductChecker();
        public string WindowsVersion { get; private set; } = string.Empty;

        static public BL Instance
        {
            get
            {
                if (_instance == null) _instance = new BL();
                return _instance;
            }
        }

        private void OnBackgroundWorkerRunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                EventAggregator.Instance.FireEvent(BlEvents.SystemScanStopped);
            }
            else if (e.Error != null)
            {
                EventAggregator.Instance.FireEvent(BlEvents.SystemScanAbortedError);
                Console.WriteLine($"An error occurred: {e.Error.Message}");
            }
            else
            {
                EventAggregator.Instance.FireEvent(BlEvents.SystemScanCompleted);
            }
        }

        private void OnBackgroundWorkerDoWork(object? sender, DoWorkEventArgs e)
        {
            EventAggregator.Instance.FireEvent(BlEvents.SystemScanStarted);

            // cve check
            CveChecker.Scan();

            // test internet connectivity
            bool isConnected = InternetConnectionChecker.Scan();

            // check for installed security products
            SecurityProductChecker.Scan();

            // testing windows version
            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsVersion);
            Dictionary<string, object> versionInfo = WindowsVersionChecker.GetVersionInfo();
            WindowsVersion = string.Join(", ", versionInfo.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

            EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsVersionCompleted);
        }

        public void StartSystemScan()
        {
            if (backgroundWorker != null && !backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        public void StopSystemScan()
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync(); // Stop the long-running process
            }
        }
    }
}
