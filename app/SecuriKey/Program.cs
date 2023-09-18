using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecuriKey
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MySimpleForm());
        }
    }

    public class MySimpleForm : Form
    {
        private Button myButton;
        private Label myLabel;
        private Button exitButton;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem helpMenu;
        private ToolStripMenuItem aboutMenuItem;
        public MySimpleForm()
        {

            
            myButton = new Button();
            myLabel = new Label();
            exitButton = new Button();            
            InitializeUI();
        }
        
        // Event Handler for Exit menu item
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            myButton = new Button();
            myButton.Text = "Test Click";
            myButton.Location = new System.Drawing.Point(10, 100);
            myButton.BackColor = Color.Transparent;
            myButton.FlatStyle = FlatStyle.Flat;
            myButton.Click += (sender, e) => 
            {
                myLabel.Text = "Button Clicked!";
            };

            // Initialize Label
            myLabel = new Label();
            myLabel.Text = "Hello Team!";
            myLabel.Location = new System.Drawing.Point(10, 150);
            myLabel.BackColor = Color.Transparent;

            // Initialize Exit Button
            exitButton = new Button();
            exitButton.Text = "Exit";
            exitButton.Location = new System.Drawing.Point(100, 100);
            exitButton.BackColor = Color.Transparent;
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Click += (sender, e) =>
            {
                Application.Exit();
            };

            // Add Controls to Form
            this.Controls.Add(myButton);
            this.Controls.Add(myLabel);
            this.Controls.Add(exitButton);
        }
    }
}
