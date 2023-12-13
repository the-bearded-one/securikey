using BusinessLogic.Scanning.Interfaces;
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
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool IsDefaultBrowser { get; private set; } = false;

        public const String ID = "SK-11";
        public SecurityCheck SecurityCheck { get; private set; }

        public IEChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingIE);

            IsDefaultBrowser = CheckDefaultBrowser();

            if ( SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error )
            {
                if (IsDefaultBrowser)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

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
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }


    }
}
