using BusinessLogic;
using BusinessLogic.Scanning;
using System.Diagnostics;
using System.Text.Json;

using Newtonsoft.Json;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace UnitTests
{
    [TestClass]
    public class CveScannerUnitTests
    {
        [TestMethod]
        public void RunCveScanner()
        {
            var cveScanner = new CveChecker();
            cveScanner.Scan();
            var issues = cveScanner.GetVulnerabilities();
            foreach ( var issue in issues )
            {
                Console.WriteLine(issue.ToString());
                Console.WriteLine("\r\n");
            }
            Assert.IsNotNull(issues);
        }

        [TestMethod]
        [DeploymentItem( "../../../TestFiles/cves_severity_high.json")]
        public void LoadCveObject()
        {
            string cveFileName = "cves_severity_high.json";
            var cveFilePath = Path.Combine(Directory.GetCurrentDirectory(), cveFileName);
            var cveObjectAsJsonStr = File.ReadAllText(cveFilePath);
            CveRoot cveRoot = JsonConvert.DeserializeObject<CveRoot>(cveObjectAsJsonStr);
            Console.WriteLine(cveRoot.ToString());
            Assert.IsNotNull(cveRoot);
        }

        [TestMethod]
        public void CveManagerOnline()
        {
            CveManagerOnline cve = new CveManagerOnline();
            cve.Search("Windows", "1.0.0.0");

            foreach (Vulnerability vul in cve.Vulnerabilities) 
            {
                Console.WriteLine("=====================");
                Console.WriteLine($"cpeMatch: {vul.cve.configurations?[0]?.nodes?[0]?.cpeMatch?[0]?.criteria ?? "N/A"}");
                Console.WriteLine($"cve id: {vul.cve.id ?? "N/A"}");
                Console.WriteLine($"version: {vul.cve.metrics?.cvssMetricV31?[0]?.cvssData?.version ?? "N/A"}");
                Console.WriteLine($"vendorComments: {vul.cve.vendorComments?[0].comment ?? "N/A"}");
                Console.WriteLine($"description: {vul.cve.descriptions?[0]?.value ?? "N/A"}");
            }

            Assert.IsNotNull(cve.Vulnerabilities);
        }

        [TestMethod]
        public void StrCompare()
        {
            string low = "1.0.0.0";
            string med = "1.0.1.0";
            string high = "1.2.0.0";

            Assert.IsTrue(low.CompareTo(med) < 0);
            Assert.IsTrue(med.CompareTo(high) < 0);
            Assert.IsTrue(low.CompareTo(high) < 0);
            Assert.IsTrue(high.CompareTo(med) > 0);
            Assert.IsTrue(high.CompareTo(low) > 0);
            Assert.IsTrue(med.CompareTo(low) > 0);
            Assert.IsTrue(med.CompareTo(med) == 0);
        }
    }
}