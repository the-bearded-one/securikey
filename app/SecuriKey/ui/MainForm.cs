using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Security;

using BusinessLogic;
using BusinessLogic.Scanning;

public class MainForm : Form
    {
        private Button startButton;
        private Button stopButton;
        private Label myLabel;
        private Button exitButton;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem helpMenu;
        private ToolStripMenuItem aboutMenuItem;
        private BackgroundWorker backgroundWorker;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken;
        private PortChecker portScan;
        public MainForm()
        {

            
            startButton = new Button();
            stopButton = new Button();
            myLabel = new Label();
            exitButton = new Button();            
            InitializeUI();
            
        }
        


        // Event Handler for Exit menu item
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BackgroundWorker_PerformScans(object sender, DoWorkEventArgs e)
        {
            // cve check
            CveChecker cveScanner = new CveChecker();
            {
                cveScanner.Scan();
            }

        // test internet connectivity
        bool isConnected = InternetConnectionChecker.IsConnectedToInternet();
            Console.WriteLine(isConnected ? "Connected to the Internet" : "Not connected to the Internet");

            // check for installed security products
            CheckSecurityProducts();

            // testing windows version
            Dictionary<string, object> versionInfo = WindowsVersionChecker.GetVersionInfo();
            string versionInfoString = string.Join(", ", versionInfo.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
            Console.WriteLine(versionInfoString);



/*
            // test the ports            
            cancellationToken = cancellationTokenSource.Token;
            portScan = new PortChecker();
            portScan.OnCompletion += result =>
            {
                // event gets raised if the scan of the ports successfully completes
                PortScan_Complete(result);
            };            
            var task = portScan.CheckPortsAsync("localhost", 20, 100, cancellationToken);
            //CheckPorts();

*/

        }

        private void CheckSecurityProducts()
        {
            List<string> antivirusProducts = SecurityProductInfo.SecurityScanner.QuerySecurityProduct("AntiVirusProduct");
            List<string> antispywareProducts = SecurityProductInfo.SecurityScanner.QuerySecurityProduct("AntiSpywareProduct");
            List<string> firewallProducts = SecurityProductInfo.SecurityScanner.QuerySecurityProduct("FirewallProduct");
            
            if ( antivirusProducts.Count() == 0 ) 
            {
                Console.WriteLine("AntiVirus Detected: None!");
            }
            foreach (string product in antivirusProducts)
            {
                Console.WriteLine(product);
            }
            
            
            if ( antispywareProducts.Count() == 0 ) 
            {
                Console.WriteLine("AntiSpyware Detected: None!");
            }
            foreach (string product in antispywareProducts)
            {
                Console.WriteLine(product);
            }
            
            
            if ( firewallProducts.Count() == 0 ) 
            {
                Console.WriteLine("Firewall Detected: None!");
            }
            foreach (string product in firewallProducts)
            {
                Console.WriteLine(product);
            }            
        }

        private void InitializeUI()
        {

            // Initialize Form
            this.Text = "SecuriKey";
            //this.BackgroundImage = Image.FromFile("path/to/your/image.jpg");
            //this.BackgroundImageLayout = ImageLayout.Stretch;

            // Initialize MenuStrip and add File and Help menus
            menuStrip = new MenuStrip();

            // Initialize and add the File menu
            fileMenu = new ToolStripMenuItem("File");
            exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Click += new EventHandler(ExitMenuItem_Click);  // Event Handler for Exit
            
            fileMenu.DropDownItems.Add(exitMenuItem);
            menuStrip.Items.Add(fileMenu);

            // Initialize and add the Help menu
            helpMenu = new ToolStripMenuItem("Help");
            aboutMenuItem = new ToolStripMenuItem("About");
            helpMenu.DropDownItems.Add(aboutMenuItem);
            menuStrip.Items.Add(helpMenu);

            // Add MenuStrip to the Form
            this.Controls.Add(menuStrip);

            // Attach MenuStrip to the Form
            this.MainMenuStrip = menuStrip;

            // Initialize Click Me Button
            startButton = new Button();
            startButton.Text = "Start Scan";
            startButton.Location = new System.Drawing.Point(10, 100);
            startButton.BackColor = Color.Transparent;
            startButton.FlatStyle = FlatStyle.Flat;
            startButton.Click += StartButton_Click;

            // Initialize Click Me Button
            stopButton = new Button();
            stopButton.Text = "Stop Scan";
            stopButton.Enabled = false;
            stopButton.Location = new System.Drawing.Point(90, 100);
            stopButton.BackColor = Color.Transparent;
            stopButton.FlatStyle = FlatStyle.Flat;
            stopButton.Click += StopButton_Click;

            // Initialize Label
            myLabel = new Label();
            myLabel.Text = "";
            myLabel.Location = new System.Drawing.Point(10, 150);
            myLabel.BackColor = Color.Transparent;

            // Initialize Exit Button
            exitButton = new Button();
            exitButton.Text = "Exit";
            exitButton.Location = new System.Drawing.Point(170, 100);
            exitButton.BackColor = Color.Transparent;
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Click += (sender, e) =>
            {
                Application.Exit();
            };

            // Initialize BackgroundWorker
            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += BackgroundWorker_PerformScans;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_ScanningCompleted;

            // Add Controls to Form
            this.Controls.Add(startButton);
            this.Controls.Add(stopButton);
            this.Controls.Add(myLabel);
            this.Controls.Add(exitButton);
        }

        private void PortScan_Complete(List<int> openPorts)
        {
            Console.WriteLine("Open ports are ", openPorts);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            
            myLabel.Text = "Scanning...";
            startButton.Enabled = false; // Disable the Start button
            stopButton.Enabled = true;  // Enable the Stop button

            if (backgroundWorker != null && !backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            myLabel.Text = "Stopped!";
            
            // stop the port scan
            cancellationTokenSource.Cancel();
            
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync(); // Stop the long-running process
            }
        }        

        private void BackgroundWorker_ScanningCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myLabel.Text = "Done!";
            stopButton.Enabled = false;  // Disable the Stop button
            startButton.Enabled = true; // re-enable the start button

            if (e.Cancelled)
            {
                Console.WriteLine("Operation was cancelled");
            }
            else if (e.Error != null)
            {
                Console.WriteLine($"An error occurred: {e.Error.Message}");
            }
            else
            {
                Console.WriteLine($"Operation completed successfully: {e.Result}");
            }
        }
}

