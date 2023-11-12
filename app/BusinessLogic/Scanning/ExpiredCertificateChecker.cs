using BusinessLogic.Scanning.Interfaces;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BusinessLogic.Scanning
{
    public class ExpiredCertificateChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool ExpiredCertsFound { get; private set; } = false;
        
        public const String ID = "SK-35";
        public SecurityCheck SecurityCheck { get; private set; }

        public ExpiredCertificateChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {


            CheckExpiredCerts();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (ExpiredCertsFound)
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

        private void CheckExpiredCerts()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    if (DateTime.Now > certificate.NotAfter)
                    {
                        Console.WriteLine($"Expired Certificate: {certificate.Subject}, Expiry: {certificate.NotAfter}");
                    }
                }
            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;

                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                store.Close();
            }


        }
        
    }
}
