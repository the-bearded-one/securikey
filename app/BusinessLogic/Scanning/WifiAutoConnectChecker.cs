using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BusinessLogic.Scanning
{
    public class WifiAutoConnectChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool DoesWifiAutoConnect { get; private set; } = false;
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingAutoConnectOpenWifi);

            CheckWifiConfig();

            ScanResult result = new ScanResult();
            result.ScanType = "Wifi Auto Connect";
            result.DetailedDescription = $"Automatically connecting to open Wi-Fi networks exposes the system to various security risks, including man-in-the-middle attacks and data interception. Since these networks lack encryption and authentication, attackers can easily eavesdrop on your internet traffic or redirect you to malicious websites, thereby compromising your data and system integrity.";

            if (DoesWifiAutoConnect)
            {
                result.Severity = Severity.High;
                result.ShortDescription = "Your system auto-connects to open, public wifi networks";
            }
            else
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = "Your system does not auto-connects to open, public wifi networks";
            }
            ScanResults.Add(result);


            EventAggregator.Instance.FireEvent(BlEvents.CheckingAutoConnectOpenWifiCompleted);
        }

        private void CheckWifiConfig()
        {
            string wifiProfilePath = @"C:\ProgramData\Microsoft\Wlansvc\Profiles\Interfaces\";

            if (Directory.Exists(wifiProfilePath))
            {
                string[] subDirectories = Directory.GetDirectories(wifiProfilePath);

                foreach (string dir in subDirectories)
                {
                    string[] xmlFiles = Directory.GetFiles(dir, "*.xml");

                    foreach (string xmlFile in xmlFiles)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(xmlFile);

                        XmlNodeList autoConnectList = xmlDoc.GetElementsByTagName("autoConnect");
                        XmlNodeList authenticationList = xmlDoc.GetElementsByTagName("authentication");
                        XmlNodeList encryptionList = xmlDoc.GetElementsByTagName("encryption");

                        if (autoConnectList.Count > 0 && authenticationList.Count > 0 && encryptionList.Count > 0)
                        {
                            string autoConnectValue = autoConnectList[0].InnerText;
                            string authenticationValue = authenticationList[0].InnerText;
                            string encryptionValue = encryptionList[0].InnerText;

                            if (autoConnectValue.ToLower() == "true" && authenticationValue.ToLower() == "open" && encryptionValue.ToLower() == "none")
                            {
                                DoesWifiAutoConnect = true;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Wi-Fi profile directory not found.");
            }
        }

    }
}
