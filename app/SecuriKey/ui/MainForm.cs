using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

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
        public MainForm()
        {

            
            startButton = new Button();
            stopButton = new Button();
            myLabel = new Label();
            exitButton = new Button();            
            InitializeUI();
            
        }
        
        private void CheckPorts()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += async (sender, e) => {
                var openPorts = await PortChecker.CheckPortsAsync("localhost", 1, 65535);
                // Do something with openPorts
                Console.WriteLine(openPorts);
            };
            worker.RunWorkerAsync();    
        }

        // Event Handler for Exit menu item
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 10; i++)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                // Simulating a long-running process
                System.Threading.Thread.Sleep(1);
            }
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
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;


            // Add Controls to Form
            this.Controls.Add(startButton);
            this.Controls.Add(stopButton);
            this.Controls.Add(myLabel);
            this.Controls.Add(exitButton);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            myLabel.Text = "Scanning...";
            startButton.Enabled = false; // Disable the Start button
            stopButton.Enabled = true;  // Enable the Stop button


            bool isConnected = InternetConnectionChecker.IsConnectedToInternet();

            Console.WriteLine(isConnected ? "Connected to the Internet" : "Not connected to the Internet");

            CheckSecurityProducts();

            CheckPorts();
        
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            myLabel.Text = "Stopped!";
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync(); // Stop the long-running process
            }
        }        

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myLabel.Text = "Done!";
            stopButton.Enabled = false;  // Disable the Stop button
            if (!e.Cancelled)
            {
                startButton.Enabled = true;  // Re-enable the Start button if the task was not cancelled
            }
            // Perform any additional tasks when completed, if needed
        }

    }
