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
            var cveScanner = new CveScanner();
            cveScanner.Scan();
            var issues = cveScanner.GetIssues();
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
    }
}