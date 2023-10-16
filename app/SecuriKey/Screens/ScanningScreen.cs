using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SecuriKey.Screens
{
    public partial class ScanningScreen : UserControl
    {
        List<string> wordsOfEncourangement = new List<string>();
        System.Timers.Timer animationTimer = new System.Timers.Timer();
        Stopwatch encouragementStopwatch = new Stopwatch();

        public ScanningScreen()
        {
            InitializeComponent();

            // create initial list of digital security
            wordsOfEncourangement.Add("hang tight");
            wordsOfEncourangement.Add("you're on your way to a more secure future");
            wordsOfEncourangement.Add("thank you for staying ahead of threats");
            wordsOfEncourangement.Add("you're taking control");
            wordsOfEncourangement.Add("guard you're online presence");
            wordsOfEncourangement.Add("protect your data");
            wordsOfEncourangement.Add("don't forget to take further action");
            wordsOfEncourangement.Add("stay proactive with your digital security");
            wordsOfEncourangement.Add("it is a good habit to scan regularly");
            wordsOfEncourangement.Add("digital security - you can do it!");
            wordsOfEncourangement.Add("protect the things you care about");
            wordsOfEncourangement.Add("empower yourself through digital security");

            // create timer for on screen animations
            animationTimer.Interval = 300;
            animationTimer.Elapsed += OnAnimationTimerTick;
            animationTimer.Start();

            // start the encouragement stopwatch so that we can rotate encouragement
            encouragementStopwatch.Start();
        }

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

                // show a new encouragement every three seconds
                if (encouragementStopwatch.ElapsedMilliseconds > 3000)
                {
                    Random random = new Random();
                    int idx = random.Next(0, wordsOfEncourangement.Count - 1);
                    encouragementLabel.Text = wordsOfEncourangement[idx];
                    encouragementStopwatch.Restart();
                }
            }
        }
    }
}
