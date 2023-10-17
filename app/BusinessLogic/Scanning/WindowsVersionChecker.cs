using BusinessLogic;
using BusinessLogic.Scanning;
using BusinessLogic.Scanning.POCOs;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WUApiLib;

public class WindowsVersionChecker : IChecker
{
    public bool IsInternetAccessAuthorized { get; set; } = false;
    public bool IsUpdateAvailable { get; set; } = false;
    public Dictionary<string, string> VersionInfo = new Dictionary<string, string>();
    public List<string> AvailableUpdates = new List<string>();
    public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

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

        if (AvailableUpdates.Count > 0)
        {
            ScanResult result = new ScanResult();
            result.ScanType = "Windows Version";
            result.Severity = Severity.High;
            result.ShortDescription = "Windows has newer version";
            result.DetailedDescription = $"Keeping Windows up to date is crucial because it ensures the security of your system by patching vulnerabilities, safeguards your personal data, enhances system stability and performance, maintains compatibility with software and hardware, and provides access to new features, all while reducing the risk of cyberattacks, data breaches, and system failures.";
            ScanResults.Add(result);
        }
        else
        {
            ScanResult result = new ScanResult();
            result.ScanType = "Windows Version";
            result.Severity = Severity.Ok;
            result.ShortDescription = "You are protected with the newest Windows version";
            result.DetailedDescription = $"Keeping Windows up to date is crucial because it ensures the security of your system by patching vulnerabilities, safeguards your personal data, enhances system stability and performance, maintains compatibility with software and hardware, and provides access to new features, all while reducing the risk of cyberattacks, data breaches, and system failures.";
            ScanResults.Add(result);
        }

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
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
