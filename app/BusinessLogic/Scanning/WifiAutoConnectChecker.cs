using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class WifiAutoConnectChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool DoesWifiAutoConnect { get; private set; } = false;
        
        public const String ID = "SK-21";
        public SecurityCheck SecurityCheck { get; private set; }

        public WifiAutoConnectChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }
        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingAutoConnectOpenWifi);

            try
            {

                CheckWifiConfig();

                if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
                {
                    if (DoesWifiAutoConnect)
                    {
                        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                    }
                    else
                    {
                        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                    }
                }

                SecurityResults.Add(SecurityCheck);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Exception while running WifiAutoChecker");
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingAutoConnectOpenWifiCompleted);
        }

        private void CheckWifiConfig()
        {

            try
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
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
            }
        }

    }
}
