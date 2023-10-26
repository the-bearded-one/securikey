using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class IEChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool IsDefaultBrowser { get; private set; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingIE);

            IsDefaultBrowser = CheckDefaultBrowser();

            ScanResult result = new ScanResult();
            result.ScanType = "Application Software";
            result.DetailedDescription = $"Internet Explorer is no longer supported. Consider switching to a modern browser.";
            if ( !IsDefaultBrowser )
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"Internet Explorer is not the default browser.";
            }
            else 
            {
                result.Severity = Severity.High;
                result.ShortDescription = $"Internet Explorer is the default browser!";
            }
            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingIECompleted);

        }

        private bool CheckDefaultBrowser()
        {
            try
            {
                // Get the default browser from the registry
                string browserKeyPath = @"HTTP\shell\open\command";
                RegistryKey browserKey = Registry.ClassesRoot.OpenSubKey(browserKeyPath, false);

                // Check if we successfully retrieved the registry key
                if (browserKey == null)
                {
                    return false;
                }

                // Get the default browser's path from the registry value
                string browserPath = browserKey.GetValue(null).ToString().ToLower();

                // Check if Internet Explorer is the default browser
                if (browserPath.Contains("iexplore.exe"))
                {
                    IsDefaultBrowser = true;
                }
                else
                {
                    IsDefaultBrowser = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }


    }
}
