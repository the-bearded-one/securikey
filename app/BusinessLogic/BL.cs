using BusinessLogic.Scanning;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics;

namespace BusinessLogic
{
    /// <summary>
    /// The one and only Business Logic
    /// </summary>
    public class BL
    {
        static private BL _instance = null;
        private bool isInternetConnectionAuthorized = false;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private List<IChecker> checkers = new List<IChecker>();
        private ReportGenerator reportGenerator = new ReportGenerator();
        private int scanChecksCompleted = 0; // tracks number of completed checks in current scan

        private PostureGrader postureGrader = new PostureGrader();
        #region Constructor

        /// <summary>
        /// The one and only BL. Force singleton pattern by hiding default constructor
        /// </summary>
        private BL()
        {
            // Create a list of scanners
            checkers.Add(this.CveChecker);
            checkers.Add(this.InternetConnectionChecker);
            checkers.Add(this.SecurityProductChecker);
            checkers.Add(this.WindowsVersionChecker);
            checkers.Add(this.AppScanner);
            checkers.Add(this.UserType);
            checkers.Add(this.WindowsScriptingHostChecker);
            checkers.Add(this.RdpChecker);
            checkers.Add(this.SecureBootChecker);
            checkers.Add(this.WindowsUpdateChecker);
            checkers.Add(this.FirewallChecker);
            checkers.Add(this.BitLockerChecker);
            checkers.Add(this.UacChecker);
            checkers.Add(this.PowerShellChecker);
            checkers.Add(this.NtlmChecker);
            checkers.Add(this.EncryptedPageFileChecker);


            // subscribe to events
            backgroundWorker.DoWork += OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
        }

        static public BL Instance
        {
            get
            {
                _instance ??= new BL();
                return _instance;
            }
        }

        #endregion

        #region Properties

        public bool IsInternetConnectionAuthorized
        {
            get => isInternetConnectionAuthorized;
            set
            {
                isInternetConnectionAuthorized = value;
                WindowsVersionChecker.IsInternetAccessAuthorized = value;
            }
        }
        public EventAggregator EventAggregator { get => EventAggregator.Instance; }
        public CveChecker CveChecker { get; private set; } = new CveChecker();
        public InternetConnectionChecker InternetConnectionChecker { get; private set; } = new InternetConnectionChecker();
        public SecurityProductChecker SecurityProductChecker { get; private set; } = new SecurityProductChecker();
        public WindowsVersionChecker WindowsVersionChecker { get; private set; } = new WindowsVersionChecker();
        public AppScanner AppScanner { get; private set; } = new AppScanner();
        public UserType UserType { get; private set; } = new UserType();
        public WindowsScriptingHostChecker WindowsScriptingHostChecker { get; private set; } = new WindowsScriptingHostChecker();
        public RdpChecker RdpChecker { get; private set; } = new RdpChecker();
        public SecureBootChecker SecureBootChecker { get; private set; } = new SecureBootChecker();
        public FirewallChecker FirewallChecker { get; private set; } = new FirewallChecker();
        public WindowsUpdateChecker WindowsUpdateChecker { get; private set; } = new WindowsUpdateChecker();
        public BitLockerChecker BitLockerChecker { get; private set; } = new BitLockerChecker();
        public UacChecker UacChecker { get; private set; } = new UacChecker();
        public PowerShellChecker PowerShellChecker { get; private set; } = new PowerShellChecker();
        public NtlmChecker NtlmChecker { get; private set; } = new NtlmChecker();
        public EncryptedPageFileChecker EncryptedPageFileChecker { get; private set; } = new EncryptedPageFileChecker();
        public int ScanPercentCompleted { get => (int)((double)scanChecksCompleted / (double)checkers.Count * 100D); }
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        #endregion

        #region Event Handlers

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
            ScanResults.Clear();
            scanChecksCompleted = 0;

            EventAggregator.Instance.FireEvent(BlEvents.SystemScanStarted);

            // run all checks in parallel
            Parallel.ForEach(checkers, checker =>
            {
                checker.Scan();
                scanChecksCompleted++;
            });

            // now compile a universal list of results
            foreach(IChecker checker in checkers)
            {
                ScanResults.AddRange(checker.ScanResults);
            }

            // still in work
            Debug.WriteLine("Posture grade: ", postureGrader.Grader());
        }

        #endregion

        # region Public Methods

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

        public void GenerateReport(string filePath, string content, string password)
        {
            reportGenerator.CreatePdf(filePath, content, password);
        }

        #endregion
    }
}
