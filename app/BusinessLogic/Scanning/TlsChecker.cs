using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Scanning.Interfaces;

namespace BusinessLogic.Scanning
{
    public class TlsChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool IsVulnerable { get; private set; } = false;

        public const String ID = "SK-28";
        public SecurityCheck SecurityCheck { get; private set; }

        public TlsChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingTls);

            TestTls();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (IsVulnerable)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingTlsCompleted);

        }

        private void TestTls()
        {

            string host = "www.google.com";
            int port = 443; // Standard port for HTTPS

            // Use SystemDefault to let the OS decide on the TLS version
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            SslProtocols protocols = new SslProtocols();

            // Create a TCP client and connect
            using (var client = new TcpClient(host, port))
            {
                // Create an SSL stream that does not validate certificates (for testing purposes only)
                using (var sslStream = new SslStream(client.GetStream(), false,
                    new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)))
                {
                    try
                    {
                        // Authenticate as client using SystemDefault
                        sslStream.AuthenticateAsClient(host);

                        // Output the TLS version used for the connection
                        protocols = sslStream.SslProtocol;

                        if ( protocols.Equals(SslProtocols.None) || protocols.Equals(SslProtocols.Tls) || protocols.Equals(SslProtocols.Tls11) ) 
                        {
                            IsVulnerable = true;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                        SecurityCheck.ErrorMessage = ex.Message;

                        Console.WriteLine("Exception: " + ex.Message);
                    }
                }
            }
        }
    }

}

