using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class SbomGenerator
    {
        private List<ApplicationInfo> _applicationsInfo = new List<ApplicationInfo>();

        // Registry keys where installed programs are typically registered
        private readonly string[] __registryKeys = new[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall" // for 32-bit apps on 64-bit systems
        };

        public List<ApplicationInfo> GetInstalledAppInfo()
        {
            // clear current sbom, just in case it changed
            _applicationsInfo.Clear();

            // search through registry to find installed apps
            foreach (var registryKey in __registryKeys)
            {
                using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
                {
                    if (key != null)
                    {
                        foreach (string subKeyName in key.GetSubKeyNames())
                        {
                            using (var subKey = key.OpenSubKey(subKeyName))
                            {
                                if (subKey != null)
                                {
                                    ApplicationInfo applicationInfo = new ApplicationInfo();
                                    applicationInfo.DisplayName = subKey?.GetValue("DisplayName") as string;
                                    applicationInfo.DisplayVersion = subKey?.GetValue("DisplayVersion") as string;
                                    applicationInfo.Publisher = subKey?.GetValue("Publisher") as string;
                                    _applicationsInfo.Add(applicationInfo);
                                }
                            }
                        }
                    }
                }
            }

            return _applicationsInfo;
        }
    }
}
