using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        //NIST API URL that only outputs CVEs that are in CISA's Known Exploited Vulnerabilities catalog
        string url = "https://services.nvd.nist.gov/rest/json/cves/2.0?hasKev";
        
        //get NVD CVE data from the API
        var nvdData = await GetNvdDataAsync(url);

        if (nvdData != null)
        {
            string filterSeverity = "MEDIUM";

            //load JSON data from file
            string inputJsonFile = "/Users/anastasias/Desktop/nvd_data_2.json";
            var nvdDataFromFile = LoadJsonFromFile(inputJsonFile);

            //search and save medium severity data
            string outputJsonFile = "/Users/anastasias/Desktop/filtered_nvd_data_2.json";
            SearchAndSaveMediumSeverity(nvdDataFromFile, outputJsonFile, filterSeverity);

            //filename where the software CVE JSON data is stored
            string keywordSearchInputFile = "/Users/anastasias/Desktop/filtered_nvd_data_2.json";  // Replace with the actual filename
            string outputKeywordSearchFile = "/Users/anastasias/Desktop/microsoft_2.json";

            //software to search for
            string keyword = "Microsoft";

            SearchAndSaveKeywordResults(keywordSearchInputFile, keyword, outputKeywordSearchFile);
        }
    }

    static async Task<JsonDocument> GetNvdDataAsync(string url)
    {
        try
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonDocument.ParseAsync(responseStream);
            }
            else
            {
                Console.WriteLine($"Request failed with status code {response.StatusCode}");
                return null;
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request failed: {e.Message}");
            return null;
        }
    }

    static JsonDocument LoadJsonFromFile(string filePath)
    {
        try
        {
            using var fileStream = File.OpenRead(filePath);
            return JsonDocument.Parse(fileStream);
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error reading JSON file: {e.Message}");
            return null;
        }
    }

    static void SearchAndSaveMediumSeverity(JsonDocument data, string filename, string filterSeverity)
    {
        var mediumSeverityResults = new JsonArray();

        foreach (var vulnerability in data.RootElement.GetProperty("vulnerabilities").EnumerateArray())
        {
            var cvssMetricV2 = vulnerability.GetProperty("cve").GetProperty("metrics").GetProperty("cvssMetricV2").EnumerateArray();
            
            foreach (var metric in cvssMetricV2)
            {
                var baseSeverity = metric.GetProperty("baseSeverity").GetString();

                if (baseSeverity == filterSeverity)
                {
                    mediumSeverityResults.Add(vulnerability.Clone());
                    break;
                }
            }
        }

        using var fs = File.CreateText(filename);
        using var jsonWriter = new Utf8JsonWriter(fs.BaseStream, new JsonWriterOptions { Indented = true });
        mediumSeverityResults.WriteTo(jsonWriter);
        Console.WriteLine($"Results with baseSeverity '{filterSeverity}' saved to {filename}");
    }

    static void SearchAndSaveKeywordResults(string jsonFile, string keyword, string outputFileName)
    {
        var matchingResults = new JsonArray();

        using (var fs = File.OpenRead(jsonFile))
        using (var jsonDocument = JsonDocument.Parse(fs))
        {
            foreach (var vulnerability in jsonDocument.RootElement.GetProperty("vulnerabilities").EnumerateArray())
            {
                var vulnerabilityName = vulnerability.GetProperty("cve").GetProperty("cisaVulnerabilityName").GetString();
                if (vulnerabilityName.ToLower().Contains(keyword.ToLower()))
                {
                    matchingResults.Add(vulnerability.Clone());
                }
            }
        }

        using var fs = File.CreateText(outputFileName);
        using var jsonWriter = new Utf8JsonWriter(fs.BaseStream, new JsonWriterOptions { Indented = true });
        matchingResults.WriteTo(jsonWriter);
        Console.WriteLine($"Keyword search results saved to {outputFileName}");
    }
}
