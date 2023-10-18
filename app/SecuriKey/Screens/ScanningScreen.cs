using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SecuriKey.Screens
{
    public partial class ScanningScreen : UserControl, IScreen
    {
        List<string> wordsOfEncourangement = new List<string>();
        System.Timers.Timer animationTimer = new System.Timers.Timer();
        Stopwatch encouragementStopwatch = new Stopwatch();

        public ScanningScreen()
        {
            InitializeComponent();

            // create initial list of digital security
            wordsOfEncourangement.Add("hang tight");
            wordsOfEncourangement.Add("you are on your way to a more secure future");
            wordsOfEncourangement.Add("thank you for staying ahead of threats");
            wordsOfEncourangement.Add("you are taking control of your security");
            wordsOfEncourangement.Add("guard your online presence");
            wordsOfEncourangement.Add("protect your data");
            wordsOfEncourangement.Add("don't forget to take further action");
            wordsOfEncourangement.Add("stay proactive with your digital security");
            wordsOfEncourangement.Add("it is a good habit to scan regularly");
            wordsOfEncourangement.Add("digital security - you can do it!");
            wordsOfEncourangement.Add("protect the things you care about");
            wordsOfEncourangement.Add("empower yourself through digital security");

            SuspendLayout();

            // randomize initial words of encouragement
            UpdateWordsOfEncouragement();

            // create timer for on screen animations
            animationTimer.Interval = 300;
            animationTimer.Elapsed += OnAnimationTimerTick;
            animationTimer.Start();

            // start the encouragement stopwatch so that we can rotate encouragement
            encouragementStopwatch.Start();

            // to get transparency, the parent of controls need to be the background. so set the background image of this screen to parent of other screens
            // // put all controls into a seperate list since we will be iterating over them
            List<Control> ctrls = new List<Control>();
            foreach (Control ctrl in Controls)
            {
                ctrls.Add(ctrl);
            }
            // // now iterate over the controls and change the parent
            foreach (var ctrl in ctrls)
            {
                if (ctrl != this.backgroundPictureBox) ctrl.Parent = this.backgroundPictureBox;
            }

            // TODO fix this kludge - darkent he image of the background
            ChangePictureBoxOpacity(backgroundPictureBox, 0.15);

            ResumeLayout();
        }

        public event EventHandler<NavigationEventArgs> NavigationRequest;

        private void OnAnimationTimerTick(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { OnAnimationTimerTick(sender, e); }));
            }
            else
            {
                // update progress bar
                progressBar.Value = BL.Instance.ScanPercentCompleted;

                // animate text so user knows we haven't froze
                if (progressBar.Value < progressBar.Maximum)
                {
                    switch (progressLabel.Text)
                    {
                        case "scanning.":
                            progressLabel.Text = "scanning..";
                            break;
                        case "scanning..":
                            progressLabel.Text = "scanning...";
                            break;
                        case "scanning...":
                            progressLabel.Text = "scanning.";
                            break;
                    }
                }
                else
                {
                    progressLabel.Text = progressLabel.Text.Replace("scanning", "preparing report");
                    switch (progressLabel.Text)
                    {
                        case "preparing report.":
                            progressLabel.Text = "preparing report..";
                            break;
                        case "preparing report..":
                            progressLabel.Text = "preparing report...";
                            break;
                        case "preparing report...":
                            progressLabel.Text = "preparing report.";
                            break;
                    }
                }

                // show a new encouragement every three seconds
                if (encouragementStopwatch.ElapsedMilliseconds > 3000) UpdateWordsOfEncouragement();
            }
        }

        private void UpdateWordsOfEncouragement()
        {
            Random random = new Random();
            int idx = random.Next(0, wordsOfEncourangement.Count - 1);
            encouragementLabel.Text = wordsOfEncourangement[idx];
            encouragementStopwatch.Restart();
        }

        private void ChangePictureBoxOpacity(PictureBox pictureBox, double opacity)
        {
            if (pictureBox.Image != null)
            {
                // Create a new Bitmap with the same size as the PictureBox's image.
                Bitmap bmp = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height);

                // Create a graphics object for the new Bitmap.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Create a color matrix that sets the image opacity.
                    ColorMatrix matrix = new ColorMatrix();
                    matrix.Matrix33 = (float)opacity; // Opacity value, 1.0 for fully visible, 0.0 for fully transparent

                    // Create an image attribute and set the color matrix.
                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix);

                    // Draw the image with the specified opacity onto the new Bitmap.
                    g.DrawImage(pictureBox.Image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                }

                // Set the PictureBox's image to the modified Bitmap.
                pictureBox.Image = bmp;
            }
        }

        public UserControl AsUserControl()
        {
            return this;
        }
    }
}
