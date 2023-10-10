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
                bool isPotentiallyVulnerable = IsVersionVulnerable(vul);
            }

            return vulnerabilities;
        }

        private bool IsVersionVulnerable(Vulnerability vul)
        {
            foreach (var config in vul.cve.configurations)
            {
                foreach(var node in config.nodes)
                {
                    foreach(var cpe in node.cpeMatch)
                    {
                        var versionStart = cpe.versionStartIncluding;
                        var versionEnd = cpe.versionEndIncluding;
                    }
                }
            }
            //def is_given_version_within_ranges(node, given_version):
            //if isinstance(node, dict):
            //    if "versionStartIncluding" in node or "versionEndExcluding" in node:
            //        start = node["versionStartIncluding"] if "versionStartIncluding" in node else None
            //        end = node["versionEndExcluding"] if "versionEndExcluding" in node else None
            //        if is_version_within_range(given_version, start, end):
            //            return True
            //    for key, value in node.items():
            //        if is_given_version_within_ranges(value, given_version):
            //            return True
            //elif isinstance(node, list):
            //    for item in node:
            //        if is_given_version_within_ranges(item, given_version):
            //            return True
            //return False
            return false;
        }

        private bool IsVersionWithinRange(string version, string versionStart, string versionEnd)
        {


            //def is_version_within_range(version, start, end):
            //# normalize each version to make it comparable
            //version_n = normalize_version(version)

            //if start and end:
            //    start_n = normalize_version(start)
            //    end_n = normalize_version(end)
            //    return start_n <= version_n < end_n
            //elif start:
            //    start_n = normalize_version(start)
            //    return start_n <= version_n
            //elif end:
            //    end_n = normalize_version(end)
            //    return version_n < end_n
            //else:
            //    return False
            return false;
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
