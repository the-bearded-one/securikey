using QuestPDF.Fluent;
using PdfSharp;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Security;

namespace BusinessLogic.Reports
{
    public class ReportGenerator
    {
        public ReportGenerator()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public bool CreatePdf(string filePath, List<ScanResult> scanResults, List<SecurityCheck> securityChecks, string password)
        {
            bool isSuccess = false;
            try
            {
                // create scan report from scan results
                ScanReport report = new ScanReport(scanResults, securityChecks);

                // save off pdf to stream
                using (Stream pdfStream = new MemoryStream())
                {
                    // gerenate pdf into stream (uses QuestPdf)
                    report.GeneratePdf(pdfStream);

                    // add password to pdf (using PdfSharp)
                    using (PdfDocument document = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify))
                    {
                        // Set the user and owner passwords
                        PdfSecuritySettings securitySettings = document.SecuritySettings;
                        securitySettings.UserPassword = password;
                        securitySettings.OwnerPassword = password + "1dv9n8l427";
                        securitySettings.PermitAccessibilityExtractContent = false;
                        securitySettings.PermitAnnotations = false;
                        securitySettings.PermitAssembleDocument = false;
                        securitySettings.PermitExtractContent = false;
                        securitySettings.PermitFormsFill = false;
                        securitySettings.PermitFullQualityPrint = false;
                        securitySettings.PermitModifyDocument = false;
                        securitySettings.PermitPrint = false;

                        // Save the PDF with the added password
                        document.Save(filePath);
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail: Unable to save pdf");
            }

            return isSuccess;
        }
    }
}
