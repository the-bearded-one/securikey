using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class PostureGrader
    {
        public string Grader()
        {
            // will drop in real values here soon
            string grade = CalculateCyberSecurityGrade(
                true, true, true, true, true, true, false, false, false, false, true, true, false, false, false);
            return grade;
        }

        public static string GetWindowsEdition()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                return queryObj["Caption"].ToString();
            }
            return "Unknown";
        }

        public static string CalculateCyberSecurityGrade(
        bool secureBoot,
        bool firewallInstalled,
        bool firewallEnabled,
        bool antivirusInstalled,
        bool antivirusEnabled,
        bool updates,
        bool rdpEnabled,
        bool rdpWeakEncryption,
        bool ports,
        bool admin,
        bool encryption,
        bool vpn,
        bool guestAccount,
        bool windowsScriptHost,
        bool appsWithVulnerabilities)
        {
            string edition = GetWindowsEdition();
            bool supportsBitLocker = !edition.Contains("Home");
            bool redistributeRDP = !rdpEnabled;

            int bitLockerWeight = 7;
            int rdpWeakEncryptionWeight = 4;
            Dictionary<string, int> weights = new Dictionary<string, int>
        {
            {"secureBoot", 12},
            {"firewallInstalled", 5},
            {"firewallEnabled", 5},
            {"antivirusInstalled", 5},
            {"antivirusEnabled", 5},
            {"updates", 7},
            {"rdpEnabled", 4},
            {"rdpWeakEncryption", redistributeRDP ? 0 : rdpWeakEncryptionWeight},
            {"ports", 6},
            {"admin", 7},
            {"encryption", supportsBitLocker ? bitLockerWeight : 0},
            {"vpn", 4},
            {"guestAccount", 6},
            {"windowsScriptHost", 6},
            {"appsWithVulnerabilities", 6}
        };

            if (!supportsBitLocker)
            {
                int numMetrics = weights.Count - 1;
                int extraPerMetric = bitLockerWeight / numMetrics;
                foreach (var key in weights.Keys.ToList())
                {
                    if (key != "encryption")
                    {
                        weights[key] += extraPerMetric;
                    }
                }
            }

            if (redistributeRDP)
            {
                int numMetrics = weights.Count - 1;
                int extraPerMetric = rdpWeakEncryptionWeight / numMetrics;
                foreach (var key in weights.Keys.ToList())
                {
                    if (key != "rdpWeakEncryption")
                    {
                        weights[key] += extraPerMetric;
                    }
                }
            }

            int totalScore = 0;
            totalScore += secureBoot ? weights["secureBoot"] : 0;
            totalScore += firewallInstalled ? weights["firewallInstalled"] : 0;
            totalScore += firewallEnabled ? weights["firewallEnabled"] : 0;
            totalScore += antivirusInstalled ? weights["antivirusInstalled"] : 0;
            totalScore += antivirusEnabled ? weights["antivirusEnabled"] : 0;
            totalScore += updates ? weights["updates"] : 0;
            totalScore += !rdpEnabled ? weights["rdpEnabled"] : 0;
            totalScore += !rdpWeakEncryption ? weights["rdpWeakEncryption"] : 0;
            totalScore += !ports ? weights["ports"] : 0;
            totalScore += !admin ? weights["admin"] : 0;
            totalScore += encryption ? weights["encryption"] : 0;
            totalScore += vpn ? weights["vpn"] : 0;
            totalScore += !guestAccount ? weights["guestAccount"] : 0;
            totalScore += !windowsScriptHost ? weights["windowsScriptHost"] : 0;
            totalScore += !appsWithVulnerabilities ? weights["appsWithVulnerabilities"] : 0;

            if (totalScore >= 90) return "A+";
            if (totalScore >= 80) return "A";
            if (totalScore >= 70) return "B";
            if (totalScore >= 60) return "C";
            if (totalScore >= 50) return "D";
            return "F";
        }

    }
}
