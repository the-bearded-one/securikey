using System;
using System.Collections.Generic;
using System.IO;
using BusinessLogic.Resources;
using BusinessLogic;
using Newtonsoft.Json;



public class SecurityCheck
{
    public enum OutcomeTypes
    {
        NotRun,
        Pass,
        ActionRecommended,
        Error
    }

    public OutcomeTypes Outcome { get; set; }
    public String ErrorMessage{ get; set; }

    [JsonProperty("ID")]
    public string ID { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Category")]
    public string Category { get; set; }
    
    [JsonProperty("Relevance")]
    public string Relevance { get; set; }

    [JsonProperty("Severity")]
    public SeverityInfo Severity { get; set; }

    [JsonProperty("SimpleAnalogy")]
    public string SimpleAnalogy { get; set; }

    [JsonProperty("Windows")]
    public WindowsInfo Windows { get; set; }

    [JsonProperty("Remediation")]
    public string Remediation { get; set; }

    [JsonProperty("CompliantMessage")]
    public string CompliantMessage { get; set; }

    [JsonProperty("ActionRequiredMessage")]
    public string ActionRequiredMessage { get; set; }

    public class SeverityInfo
    {
        [JsonProperty("Rating")]
        public string Rating { get; set; }

        [JsonProperty("Justification")]
        public string Justification { get; set; }
    }

    public class WindowsInfo
    {
        [JsonProperty("Win10")]
        public bool Win10 { get; set; }

        [JsonProperty("Win11")]
        public bool Win11 { get; set; }

        [JsonProperty("RequiresElevatedPrivileges")]
        public bool RequiresElevatedPrivileges { get; set; }

        [JsonProperty("RequiresOnlineConnectivity")]
        public bool RequiresOnlineConnectivity { get; set; }
    }

    // Helper function to fetch an instance by ID from a JSON file
    public static SecurityCheck GetInstanceById(string id)
    {
        var jsonData = Shared.ReadAllTextFromFileAsBytes(Resources.security_checks);
        var allItems = JsonConvert.DeserializeObject<List<SecurityCheck>>(jsonData);
        var item = allItems.Find(x => x.ID == id);
        item.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        return item;

    }
}
