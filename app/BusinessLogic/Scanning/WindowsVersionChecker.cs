using System.Diagnostics;
using System.Text.RegularExpressions;

public static class WindowsVersionChecker
{
    public static Dictionary<string, string> GetVersionInfo()
    {
        Dictionary<string, string> versionInfo = new Dictionary<string, string>();

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
        versionInfo.Add("OS Name", GetSystemInfoValue(output, "OS Name"));
        versionInfo.Add("OS Version", GetSystemInfoValue(output, "OS Version"));
        versionInfo.Add("OS Manufacturer", GetSystemInfoValue(output, "OS Manufacturer"));
        versionInfo.Add("System Manufacturer", GetSystemInfoValue(output, "System Manufacturer"));
        versionInfo.Add("System Model", GetSystemInfoValue(output, "System Model"));
        versionInfo.Add("System Type", GetSystemInfoValue(output, "System Type"));

        string osVersion = GetSystemInfoValue(output, "OS Version:");
        string systemManufacturer = GetSystemInfoValue(output, "System Manufacturer:");

        return versionInfo;
    }

    private static string GetSystemInfoValue(string systemInfo, string key)
    {
        // Use regular expressions to extract the value associated with the specified key
        string pattern = $"{key}:\\s*(.*?)\\s*\\r\\n";
        Match match = Regex.Match(systemInfo, pattern);

        if (match.Success) return match.Groups[1].Value.Trim();
        else return "Not found";
    }
}
