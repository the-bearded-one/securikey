import requests
import json

#Python code that gets CVEs with known vulberabilities and medium severity from NIST API 
#and searches for matching software (Ex. Microsoft) with results saved to file

def get_nvd_data(url):
    #function that receives data from NIST API

    try:
        response = requests.get(url)

        if response.status_code == 200:
            data = response.json()
            return data

        else:
            print(f"Request failed with status code {response.status_code}")
            return None

    except requests.exceptions.RequestException as e:
        print(f"Request failed: {str(e)}")
        return None

def search_and_save_medium_severity(data, filename, filter_severity):
    #function that searches for medium severities in JSON file and saves them to new file
    
    try:
        medium_severity_results = []

        for vulnerability in data.get("vulnerabilities", []):
            cvss_metric_v2 = vulnerability.get("cve", {}).get("metrics", {}).get("cvssMetricV2", [])
            
            for metric in cvss_metric_v2:
                baseSeverity = metric.get("baseSeverity")

                if baseSeverity == filter_severity:
                    medium_severity_results.append(vulnerability)

        with open(filename, 'w', encoding='utf-8') as json_file:
            json.dump({"vulnerabilities": medium_severity_results}, json_file, indent=4)


        print(f"Results with baseSeverity 'MEDIUM' saved to {filename}")

    except Exception as e:
        print(f"Error saving results to file: {str(e)}")

def search_by_keyword(data, keyword):
    #function that searches for keyword in JSON data

    matching_results = []

    for vulnerability in data.get("vulnerabilities", []):
        vulnerability_name = vulnerability.get("cve", {}).get("cisaVulnerabilityName", "")
        
        if keyword.lower() in vulnerability_name.lower():
            matching_results.append(vulnerability)

    return matching_results

def search_keyword_in_file(json_file, keyword, output_file):
    #function that searches for specific software type in JSON data

    with open(json_file, 'r', encoding='utf-8') as file:
            data = json.load(file)

    matching_results = search_by_keyword(data, keyword)

    with open(output_file, 'w', encoding='utf-8') as json_file:
        json.dump({"vulnerabilities": matching_results}, json_file, indent=4)

    return matching_results


#NIST API URL that only outputs CVEs that are in CISA's Known Exploited Vulnerabilities catalog
url = "https://services.nvd.nist.gov/rest/json/cves/2.0?hasKev"
nvd_data = get_nvd_data(url)

if nvd_data is not None:
    filter_severity = "MEDIUM"
    with open("nvd_data.json", 'r', encoding='utf-8') as json_file:
        nvd_data = json.load(json_file)

    search_and_save_medium_severity(nvd_data, "filtered_nvd_data.json", filter_severity)

#filename where the keyword search JSON data is stored
input_data_filename = "filtered_nvd_data.json"  
output_file = "microsoft.json"

#keyword (software type) to search for
keyword = "Microsoft"

search_keyword_in_file(input_data_filename, keyword, output_file)
