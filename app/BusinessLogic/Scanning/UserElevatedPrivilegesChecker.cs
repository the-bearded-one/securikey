using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class UserElevatedPrivilegesChecker : IChecker
    {

        public bool IsElevatedUser { get; private set; }
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();


        public const String ID = "SK-05";
        public SecurityCheck SecurityCheck { get; private set; }
        
        public UserElevatedPrivilegesChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            SecurityResults.Clear();
            EventAggregator.Instance.FireEvent(BlEvents.CheckingElevatedUser);

            try
            {
                IsElevatedUser = IsCurrentUserAdmin();

                if (IsElevatedUser)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }

                SecurityResults.Add(SecurityCheck);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Exception running User Elevation checker");
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingElevatedUserCompleted);
        }

        public bool IsCurrentUserAdmin()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "net",
                Arguments = "user " + Environment.UserName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"
            };

            string output = "";
            using (var process = Process.Start(psi))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            return output.Contains("Administrators");
        }


    }
}
