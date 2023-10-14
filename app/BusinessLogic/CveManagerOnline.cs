using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Xml.Linq;

namespace BusinessLogic
{
    public class CveManagerOnline
    {        
        //NIST API URL that only outputs CVEs that are in CISA's Known Exploited Vulnerabilities catalog
        string url = @"https://services.nvd.nist.gov/rest/json/cves/2.0";
        public List<Vulnerability> Vulnerabilities { get; set; } = new List<Vulnerability>();

        public void Search(string keyword)
        {
            Vulnerabilities = GetCveData(keyword);
        }

        public void Search(string keyword, string version)
        {
            var vulnerabilitiesUnfiltered = GetCveData(keyword);
            Vulnerabilities = FilterVersion(vulnerabilitiesUnfiltered, version);
        }

        private List<Vulnerability> GetCveData(string keyword)
        {
            CveRoot cveData = null;
            try
            {
                var keywordParam = $"keywordSearch={keyword}";
                var fullUrl = $"{url}?{keywordParam}&keywordExactMatch&hasKev";

                var httpClient = new HttpClient();
                var response = httpClient.GetAsync(fullUrl);

                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var cveDataAsJsonStr = response.Result.Content.ReadAsStringAsync().Result;

                    cveData = JsonConvert.DeserializeObject<CveRoot>(cveDataAsJsonStr);
                }
                else
                {
                    Console.WriteLine($"Request failed with status code {response.Result.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request failed: {e.Message}");
            }
            return cveData.vulnerabilities;
        }

        private List<Vulnerability> FilterVersion(List<Vulnerability> vulnerabilitiesUnfiltered, string version)
        {
            List<Vulnerability> vulnerabilities = new List<Vulnerability>();

            foreach(var vul  in vulnerabilitiesUnfiltered)
            {
                bool isPotentiallyVulnerable = IsVersionVulnerable(vul, version);

                if (isPotentiallyVulnerable)
                {
                    // save off the vulnerabilities
                    vulnerabilities.Add(vul);
                }
            }

            return vulnerabilities;
        }

        private bool IsVersionVulnerable(Vulnerability vul, string version)
        {
            bool isVulnerable = false;
            foreach (var config in vul.cve.configurations)
            {
                foreach(var node in config.nodes)
                {
                    foreach(var cpe in node.cpeMatch)
                    {
                        var versionStart = cpe.versionStartIncluding;
                        var versionEnd = cpe.versionEndExcluding;

                        // check if version is within range
                        if (versionStart != null && versionEnd != null)
                        {
                            // if active version is between the known start and end vulnerable version, then we know we're vulnerable
                            if (version.CompareTo(versionStart) >= 0 && version.CompareTo(versionEnd) <= 0) isVulnerable = true;
                        }
                        else if (versionStart != null)
                        {
                            // if active version is greater than the versin we know is vulnerable, and it doesn't have an end version,
                            // then it is possible that the vulnerability was never fixed. So mark it as vulnerable
                            if (version.CompareTo(versionStart) >= 0) isVulnerable = true;
                        }
                        else if (versionEnd != null)
                        {
                            // if there is no start version, and but the active version of software is less than the fixed version, then
                            // its possible we have an vulernable version
                            if (version.CompareTo(versionEnd) <= 0) isVulnerable = true;
                        }
                    }
                }
            }

            return isVulnerable;
        }

        private string NormalizeVersion(string version, int minDig = 4)
        {
            // Split the version string by dots
            string[] parts = version.Split('.');

            // Normalize each part to have at least four digits
            var normalizedParts = parts.Select(part => part.PadLeft(minDig, '0')).ToList();

            // If there are less than four parts, add another section to make all version strings at least 4 number parts
            while (normalizedParts.Count < 4)
            {
                normalizedParts.Add("0000");
            }

            // Join the normalized parts with dots to form the normalized version
            string normalizedVersion = string.Join(".", normalizedParts);

            return normalizedVersion;
        }
}
}
