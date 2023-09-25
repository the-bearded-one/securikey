using BusinessLogic;
using System;
using System.Collections.Generic;

public static class WindowsVersionChecker
{
    public static Dictionary<string, object> GetVersionInfo()
    {
        var versionInfo = new Dictionary<string, object>();
        OperatingSystem osInfo = Environment.OSVersion;
        Version version = osInfo.Version;

        versionInfo.Add("Platform", osInfo.Platform);
        versionInfo.Add("Version", version);
        versionInfo.Add("Major Version", version.Major);
        versionInfo.Add("Minor Version", version.Minor);
        versionInfo.Add("Build Number", version.Build);
        versionInfo.Add("Revision", version.Revision);
        versionInfo.Add("Service Pack", osInfo.ServicePack);

        if (osInfo.Platform == PlatformID.Win32NT && version.Major >= 6 && version.Minor >= 2)
        {
            Version fullVersion = new Version(version.Major, version.Minor, version.Build);
            versionInfo.Add("Full Version", fullVersion.ToString());
        }

        return versionInfo;
    }
}
