using BusinessLogic.Reports;
using BusinessLogic.Scanning;
using BusinessLogic.Scanning.Interfaces;
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
        private bool isInternetConnectionAuthorized = true;
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
            checkers.Add(this.WindowsVersionChecker);
            checkers.Add(this.WindowsScriptingHostChecker);
            checkers.Add(this.RdpChecker);
            checkers.Add(this.SecureBootChecker);
            checkers.Add(this.WindowsUpdateChecker);
            checkers.Add(this.FirewallActiveChecker);
            checkers.Add(this.BitLockerChecker);
            checkers.Add(this.UacChecker);
            checkers.Add(this.PowerShellChecker);
            checkers.Add(this.NtlmChecker);
            checkers.Add(this.EncryptedPageFileChecker);
            checkers.Add(this.AutoRunEnabledChecker);
            checkers.Add(this.SmbClientChecker);
            checkers.Add(this.SmbServerChecker);
            checkers.Add(this.HeartbleedChecker);
            checkers.Add(this.NoPasswordExpiryChecker);
            checkers.Add(this.IEChecker);
            checkers.Add(this.WindowsSubsystemLinuxChecker);
            checkers.Add(this.UnsignedDriverUnelevatedChecker);
            checkers.Add(this.UnsignedDriverElevatedChecker);
            checkers.Add(this.AdminChecker);
            checkers.Add(this.PasswordComplexityChecker);
            checkers.Add(this.WifiAutoConnectChecker);
            checkers.Add(this.WindowsGuestAccountChecker);
            checkers.Add(this.AntivirusInstalledChecker);
            checkers.Add(this.FirewallInstalledChecker);
            checkers.Add(this.AntispywareInstalledChecker);
            checkers.Add(this.TlsChecker);
            checkers.Add(this.RemoteRegistryChecker);
            checkers.Add(this.TelnetChecker);
            checkers.Add(this.SpoolerChecker);
            checkers.Add(this.RemoteManagementChecker);
            checkers.Add(this.FaxChecker);
            checkers.Add(this.SandboxChecker);
            checkers.Add(this.ExpiredCertificateChecker);            
            checkers.Add(this.LlmnrChecker);
            checkers.Add(this.NetBiosChecker);
            checkers.Add(this.UsbAuditingChecker);
            checkers.Add(this.NetworkDiscoveryChecker);

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
        public WindowsVersionChecker WindowsVersionChecker { get; private set; } = new WindowsVersionChecker();
        public WindowsScriptingHostChecker WindowsScriptingHostChecker { get; private set; } = new WindowsScriptingHostChecker();
        public RdpChecker RdpChecker { get; private set; } = new RdpChecker();
        public SecureBootChecker SecureBootChecker { get; private set; } = new SecureBootChecker();
        public FirewallActiveChecker FirewallActiveChecker { get; private set; } = new FirewallActiveChecker();
        public WindowsUpdateChecker WindowsUpdateChecker { get; private set; } = new WindowsUpdateChecker();
        public BitLockerChecker BitLockerChecker { get; private set; } = new BitLockerChecker();
        public UacChecker UacChecker { get; private set; } = new UacChecker();
        public PowerShellChecker PowerShellChecker { get; private set; } = new PowerShellChecker();
        public NtlmChecker NtlmChecker { get; private set; } = new NtlmChecker();
        public EncryptedPageFileChecker EncryptedPageFileChecker { get; private set; } = new EncryptedPageFileChecker();
        public AutoRunEnabledChecker AutoRunEnabledChecker { get; private set; } = new AutoRunEnabledChecker();
        public SmbClientChecker SmbClientChecker { get; private set; } = new SmbClientChecker();
        public SmbServerChecker SmbServerChecker { get; private set; } = new SmbServerChecker();
        public HeartbleedChecker HeartbleedChecker { get; private set; } = new HeartbleedChecker();
        public NoPasswordExpiryChecker NoPasswordExpiryChecker  { get; private set; } = new NoPasswordExpiryChecker();
        public IEChecker IEChecker  { get; private set; } = new IEChecker();
        public WindowsSubsystemLinuxChecker WindowsSubsystemLinuxChecker { get; private set; } = new WindowsSubsystemLinuxChecker();
        public UnsignedDriverUnelevatedChecker UnsignedDriverUnelevatedChecker { get; private set; } = new UnsignedDriverUnelevatedChecker();
        public UnsignedDriverElevatedChecker UnsignedDriverElevatedChecker { get; private set; } = new UnsignedDriverElevatedChecker();
        public UserElevatedPrivilegesChecker AdminChecker { get; private set; } = new UserElevatedPrivilegesChecker();
        public PasswordComplexityChecker PasswordComplexityChecker{ get; private set; } = new PasswordComplexityChecker();
        public WifiAutoConnectChecker WifiAutoConnectChecker { get; private set; } = new WifiAutoConnectChecker();
        public WindowsGuestAccountChecker WindowsGuestAccountChecker { get; private set; } = new WindowsGuestAccountChecker();
        public AntivirusInstalledChecker AntivirusInstalledChecker { get; private set; } = new AntivirusInstalledChecker();
        public FirewallInstalledChecker FirewallInstalledChecker { get; private set; } = new FirewallInstalledChecker();        
        public AntispywareInstalledChecker AntispywareInstalledChecker  { get; private set; } = new AntispywareInstalledChecker();
        public RemoteRegistryChecker RemoteRegistryChecker { get; private set; } = new RemoteRegistryChecker();
        public TlsChecker TlsChecker { get; private set; } = new TlsChecker();
        public TelnetChecker TelnetChecker { get; private set; } = new TelnetChecker();
        public SpoolerChecker SpoolerChecker { get; private set; } = new SpoolerChecker();
        public RemoteManagementChecker RemoteManagementChecker { get; private set; } = new RemoteManagementChecker();
        public FaxChecker FaxChecker { get; private set; } = new FaxChecker();
        public SandboxChecker SandboxChecker { get; private set; } = new SandboxChecker();
        public ExpiredCertificateChecker ExpiredCertificateChecker { get; private set; } = new ExpiredCertificateChecker();
        public LlmnrChecker LlmnrChecker { get; private set; } = new LlmnrChecker();
        public NetBiosChecker NetBiosChecker { get; private set; } = new NetBiosChecker();
        public UsbAuditingChecker UsbAuditingChecker { get; private set; } = new UsbAuditingChecker();
        public NetworkDiscoveryChecker NetworkDiscoveryChecker { get; private set; } = new NetworkDiscoveryChecker();



        public int ScanPercentCompleted { get => (int)((double)scanChecksCompleted / (double)checkers.Count * 100D); }
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityChecks { get; private set; } = new List<SecurityCheck>();



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
                try
                {
                    Debug.WriteLine($"Running Checker Object: {checker.ToString()}");
                    checker.Scan();
                    scanChecksCompleted++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR Performing Check: {checker.ToString()} {ex.Message}");
                }
            });

            // now compile a universal list of results
            foreach(IChecker checker in checkers)
            {
                ScanResults.AddRange(checker.ScanResults);
            }

            // now compile a universal list of security checks
            foreach (IChecker checker in checkers)
            {
                SecurityChecks.AddRange(checker.SecurityResults);
            }

            // remove duplicate entries
            // TODO: this is a kludge to deal with duplicates over multile scans. need to fix for realz in the future
            // Remove duplicates based on CriteriaProperty
            ScanResults = ScanResults.GroupBy(x => x.GetType())
                           .Select(g => g.First())
                           .ToList();

            SecurityChecks = SecurityChecks.GroupBy(x => x.ID)
                                       .Select(g => g.First())
                                       .ToList();

            // finally, sort the scan results by severity
            SecurityChecks.Sort(new SeverityComparer());

            ScanResults.Sort((a, b) =>
            {
                return b.Severity.CompareTo(a.Severity);
            });

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

        public void GenerateReport(string filePath, string password, bool shallOpenAfterSaving)
        {
            bool isSuccess = reportGenerator.CreatePdf(filePath, ScanResults, SecurityChecks,  password);

            try
            {
                if (shallOpenAfterSaving && isSuccess)
                {
                    // open pdf to view
                    Process.Start("explorer.exe", filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to open PDF Report");
            }
        }

        #endregion
    }
}
