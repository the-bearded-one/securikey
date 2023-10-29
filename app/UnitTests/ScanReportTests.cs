using BusinessLogic.Reports;
using QuestPDF.Fluent;
using System.Diagnostics;
using System.DirectoryServices;

namespace UnitTests
{
    [TestClass]
    public class ScanReportTests
    {
        const string _loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        [TestMethod]
        public void CreateScanReportNoPassword()
        {
            var scanResults = CreateScanResults();
            var sr = new ScanReport(scanResults);
            var currentDir = Directory.GetCurrentDirectory();
            var pdfPath = currentDir + "\\test.pdf";
            sr.GeneratePdf(pdfPath);

            // open pdf to view
            Process.Start("explorer.exe", pdfPath);
        }

        [TestMethod]
        public void CreateScanReportWithPassword()
        {
            var scanResults = CreateScanResults();
            var currentDir = Directory.GetCurrentDirectory();
            var pdfPath = currentDir + "\\test.pdf";


            ReportGenerator rg = new ReportGenerator();
            rg.CreatePdf(pdfPath, scanResults, "password");

            // open pdf to view
            Process.Start("explorer.exe", pdfPath);
        }

        private List<ScanResult> CreateScanResults()
        {
            List<ScanResult> results = new List<ScanResult>();

            for (int i = 0; i < 3; i++)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Cve Scan";
                result.Severity = BusinessLogic.Severity.High;
                result.ShortDescription = $"High severity result short description {i}";
                result.DetailedDescription = _loremIpsum;
                results.Add(result);
            }

            for (int i = 0; i < 5; i++)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Windows Scan";
                result.Severity = BusinessLogic.Severity.Medium;
                result.ShortDescription = $"Medium severity result short description {i}";
                result.DetailedDescription = _loremIpsum;
                results.Add(result);
            }

            for (int i = 0; i < 7; i++)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "Network Scan";
                result.Severity = BusinessLogic.Severity.Low;
                result.ShortDescription = $"Low severity result short description {i}";
                result.DetailedDescription = _loremIpsum;
                results.Add(result);
            }

            for (int i = 0; i < 8; i++)
            {
                ScanResult result = new ScanResult();
                result.ScanType = "User Configuration Scan";
                result.Severity = BusinessLogic.Severity.Ok;
                result.ShortDescription = $"OK severity result short description {i}";
                result.DetailedDescription = _loremIpsum;
                results.Add(result);
            }

            return results;
        }
    }
}