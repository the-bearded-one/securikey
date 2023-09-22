using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class SbomManager
    {
        public string GetInstalledApplications()
        {
            StringBuilder applicationsInfo = new StringBuilder();

            // Registry keys where installed programs are typically registered
            string[] registryKeys = new[]
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall" // for 32-bit apps on 64-bit systems
            };

            foreach (var registryKey in registryKeys)
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
                                    string name = subKey.GetValue("DisplayName") as string;
                                    string version = subKey.GetValue("DisplayVersion") as string;
                                    string publisher = subKey.GetValue("Publisher") as string;

                                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(version))
                                    {
                                        applicationsInfo.AppendLine($"Name: {name}");//\r\nVersion: {version}\r\n-----------------------------------");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            File.WriteAllText(@"C:\projects\PublishSingleFileTest\PublishSingleFileTest\bin\Debug\net6.0\_apps_reg.txt", applicationsInfo.ToString());
            return applicationsInfo.ToString();
        }
    }
}
