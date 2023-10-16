using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class WindowsUpdateChecker : IChecker
    {
        public bool AreRegularUpdatesEnabled { get; private set; } = false;

        public void Scan()
        {

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRegularUpdates);

            try
            {
                // Open the registry key for reading
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", false))
                {
                    if (key != null)
                    {
                        // Read the AUOptions value from the key
                        Object val = key.GetValue("AUOptions");
                        if (val != null)
                        {
                            int setting = Convert.ToInt32(val);
                            switch (setting)
                            {
                                case 2:
                                case 3:
                                case 4:
                                    Console.WriteLine("Windows Auto Update is enabled.");
                                    break;
                                default:
                                    Console.WriteLine("Windows Auto Update is disabled.");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Windows Auto Update setting could not be determined.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Windows Auto Update setting could not be determined.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRegularUpdatesCompleted);

        }
    }

}
