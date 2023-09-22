using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class CveScanner : IScanner
    {
        private SbomGenerator _sbomGenerator = new SbomGenerator();
        private List<ApplicationInfo> _appsInfo = null;
        private List<Issue> _Issues = new List<Issue>();

        public bool IsScanning { get; private set; } = false;

        public List<Issue> GetIssues()
        {
            return _Issues;
        }

        public void Scan()
        {
            // get a list of install apps
            _appsInfo = _sbomGenerator.GetInstalledAppInfo();

            // check against cve database to determine if there is a known vulnerability
            // fudge data for now
            Issue testIssue = new Issue();
            testIssue.Asset = "Windows OS, version 5";
            testIssue.Description = "Vulnerability in buff overflow";
            testIssue.Severity = Severity.Critical;
            testIssue.ExploitMaturity = ExploitMaturity.Mature;
            testIssue.FixedIn = "Vulnerability fixed in version 8";
            testIssue.RemediationRecommendation = "Update Windows OS to latest version";
            _Issues.Add(testIssue);

            Issue testIssue2 = new Issue();
            testIssue2.Asset = "Google Chrome, version 8";
            testIssue2.Description = "Cookie leak information";
            testIssue2.Severity = Severity.High;
            testIssue.ExploitMaturity = ExploitMaturity.ProofOfConcept;
            testIssue2.FixedIn = "Vulnerability fixed in version 9";
            testIssue2.RemediationRecommendation = "Update Google Chrome browser to latest version";
            _Issues.Add(testIssue2);
        }
    }
}
