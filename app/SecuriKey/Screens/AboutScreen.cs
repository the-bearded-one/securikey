using BusinessLogic;
using OpenAI.Files;
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
    public partial class AboutScreen : UserControl, IScreen
    {
        public AboutScreen()
        {
            InitializeComponent();

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

            ChangePictureBoxOpacity(this.backgroundPictureBox, 0.1);
        }

        public event EventHandler<NavigationEventArgs> NavigationRequest;

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

        private void OnBackScanButtonClick(object sender, EventArgs e)
        {
            NavigationRequest?.Invoke(this, new NavigationEventArgs(new HomeScreen()));
        }

        private void OnWebsiteLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // launch securikey websiite using default browser
            Process.Start("explorer.exe", "https://www.securikey.io");
        }

        public UserControl AsUserControl()
        {
            return this;
        }
    }
}
