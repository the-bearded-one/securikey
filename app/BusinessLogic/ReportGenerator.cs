using PdfSharp.Drawing;
using PdfSharp.Pdf;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLogic
{
    public class ReportGenerator
    {
        public ReportGenerator() 
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public void CreatePdf(string filePath, string content, string password)
        {
            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = "SecuriKey Security Posture Report";
            
            // create an empty page
            var page = pdf.AddPage();

            // create a graphics object for drawing
            var g = XGraphics.FromPdfPage(page);

            // create a font
            var font = new XFont("Arial",
                8,
                XFontStyle.Regular);

            // split the multiline text into lines
            string[] lines = content.Split("\r\n");

            // Set the starting position for drawing
            double x = 50;
            double y = 50;

            // draw each line of text
            foreach (string line in lines)
            {
                g.DrawString(line, font, XBrushes.Black, new XPoint(x, y));
                y += font.Height; // Move to the next line
            }

            // add password to pdf
            pdf.SecuritySettings.UserPassword = password;
            pdf.SecuritySettings.OwnerPassword = password + 123;
            // Restrict some rights.
            pdf.SecuritySettings.PermitAccessibilityExtractContent = false;
            pdf.SecuritySettings.PermitAnnotations = false;
            pdf.SecuritySettings.PermitAssembleDocument = false;
            pdf.SecuritySettings.PermitExtractContent = false;
            pdf.SecuritySettings.PermitFormsFill = false;
            pdf.SecuritySettings.PermitFullQualityPrint = false;
            pdf.SecuritySettings.PermitModifyDocument = false;
            pdf.SecuritySettings.PermitPrint = true;

            // save document
            pdf.Save(filePath);
        }
    }
}
