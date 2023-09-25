using System;
using System.Management;
using System.Collections.Generic;

namespace BusinessLogic.Scanning
{
    public static class SecurityProductInfo
    {
        // WMI stuff is queryable by non-admin accounts
        public static List<string> QuerySecurityProduct(string className)
        {
            List<string> detectedProducts = new List<string>();

            string wmiNamespace = @"\\.\root\SecurityCenter2";
            string query = $"SELECT * FROM {className}";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiNamespace, query);

            foreach (ManagementObject item in searcher.Get())
            {
                string displayName = item["displayName"].ToString();
                detectedProducts.Add($"{className} Detected: {displayName}");
            }

            return detectedProducts;
        }
    }
}
