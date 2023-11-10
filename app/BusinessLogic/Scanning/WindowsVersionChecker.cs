using BusinessLogic;
using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WUApiLib;

public class WindowsVersionChecker : IChecker
{
    public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
    public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();
    public bool IsInternetAccessAuthorized { get; set; } = false;
    public bool IsUpdateAvailable { get; set; } = false;
    public Dictionary<string, string> VersionInfo = new Dictionary<string, string>();
    public List<string> AvailableUpdates = new List<string>();
    
    public SecurityCheck SecurityCheck { get; private set; }

    public const String ID = "SK-31";

    public WindowsVersionChecker()
    {
        SecurityCheck = SecurityCheck.GetInstanceById(ID);
        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
    }
    public void Scan()
    {
        ScanResults.Clear();

        EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsVersion);

        VersionInfo.Clear();
        AvailableUpdates.Clear();

        // Create a new process to run PowerShell
        Process process = new Process();

        // Configure the process start info
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = "systeminfo"
        };
        process.StartInfo = psi;

        // Start the process
        process.Start();

        // Read the output
        string output = process.StandardOutput.ReadToEnd();

        // Wait for the process to finish
        process.WaitForExit();

        // Parse system information and store it in variables
        VersionInfo.Add("OS Name", GetSystemInfoValue(output, "OS Name"));
        VersionInfo.Add("OS Version", GetSystemInfoValue(output, "OS Version"));
        VersionInfo.Add("OS Manufacturer", GetSystemInfoValue(output, "OS Manufacturer"));
        VersionInfo.Add("System Manufacturer", GetSystemInfoValue(output, "System Manufacturer"));
        VersionInfo.Add("System Model", GetSystemInfoValue(output, "System Model"));
        VersionInfo.Add("System Type", GetSystemInfoValue(output, "System Type"));

        // if internet access is authorized, check if a windows update is available
        if (IsInternetAccessAuthorized) CheckForWindowsUpdate();

        if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
        {
            if (AvailableUpdates.Count > 0)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
            }
            else
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
            }
        }

        SecurityResults.Add(SecurityCheck);

        EventAggregator.Instance.FireEvent(BlEvents.CheckingWindowsVersionCompleted);
    }

    private string GetSystemInfoValue(string systemInfo, string key)
    {
        // Use regular expressions to extract the value associated with the specified key
        string pattern = $"{key}:\\s*(.*?)\\s*\\r\\n";
        Match match = Regex.Match(systemInfo, pattern);

        if (match.Success) return match.Groups[1].Value.Trim();
        else return "Not found";
    }

    private void CheckForWindowsUpdate()
    {
        try
        {
            UpdateSession updateSession = new UpdateSession();
            IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
            ISearchResult searchResult = updateSearcher.Search("IsInstalled=0");

            if (searchResult.Updates.Count > 0)
            {
                IsUpdateAvailable = true;

                foreach (IUpdate update in searchResult.Updates)
                {
                    AvailableUpdates.Add(update.Title);
                }
            }
            else
            {
                IsUpdateAvailable = false;
            }
        }
        catch (Exception ex)
        {
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
            SecurityCheck.ErrorMessage = ex.Message;
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
