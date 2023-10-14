﻿using AppVersionChecker;
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
        private bool isInternetConnectionAuthorized = false;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        #region Constructor

        /// <summary>
        /// The one and only BL. Force singleton pattern by hiding default constructor
        /// </summary>
        private BL()
        {
            backgroundWorker.DoWork += OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
        }

        static public BL Instance
        {
            get
            {
                if (_instance == null) _instance = new BL();
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
        public SecureBootChecker SecureBootChecker{ get; private set; } = new SecureBootChecker();
        public FirewallChecker FirewallChecker { get; private set; } = new FirewallChecker();

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
            EventAggregator.Instance.FireEvent(BlEvents.SystemScanStarted);

            // cve check
            CveChecker.Scan();

            // test internet connectivity
            InternetConnectionChecker.Scan();

            // check for installed security products
            SecurityProductChecker.Scan();

            // testing windows version
            WindowsVersionChecker.Scan();

            // check app versions
            AppScanner.Scan();

            // check user elevation status
            UserType.Scan();

            // windows scripting host enabled?
            WindowsScriptingHostChecker.Scan();

            // Does user have remote desktop protocol enabled and if it is, is it using weak security
            RdpChecker.Scan();

            // Does user have secure boot enabled
            SecureBootChecker.Scan();

            FirewallChecker.Scan();

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

        #endregion
    }
}
