using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace BusinessLogic
{
    public class CveManagerOnline
    {        
        //NIST API URL that only outputs CVEs that are in CISA's Known Exploited Vulnerabilities catalog
        string url = @"https://services.nvd.nist.gov/rest/json/cves/2.0";
        public CveRoot CveData { get; set; }

        public void Search(string keyword, string version)
        {
            try
            {
                var keywordParam = $"keywordSearch={keyword}";
                var fullUrl = $"{url}?{keywordParam}&keywordExactMatch&hasKev";

                var httpClient = new HttpClient();
                var response = httpClient.GetAsync(fullUrl);

                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var cveData = response.Result.Content.ReadAsStringAsync().Result;

                    CveData = JsonConvert.DeserializeObject<CveRoot>(cveData);
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
        }
    }
}
