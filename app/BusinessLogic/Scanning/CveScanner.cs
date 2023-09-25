﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Newtonsoft;
using Newtonsoft.Json;

namespace BusinessLogic.Scanning
{
    public class CveScanner
    {
        private SbomGenerator _sbomGenerator = new SbomGenerator();
        private List<ApplicationInfo> _appsInfo = null;
        private List<Vulnerability> _Vulnerabilities = new List<Vulnerability>();

        public bool IsScanning { get; private set; } = false;

        public List<Vulnerability> GetVulnerabilities()
        {
            return _Vulnerabilities;
        }

        public void Scan()
        {
            // clear list of previous vulnerabilities in case there was a previous scan
            this._Vulnerabilities.Clear();

            // get a list of install apps
            _appsInfo = _sbomGenerator.GetInstalledAppInfo();

            // load local CVE db
            string cveFileName = "cves_severity_high.json";
            var cveFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", cveFileName);
            var cveObjectAsJsonStr = File.ReadAllText(cveFilePath);
            CveRoot cveRoot = JsonConvert.DeserializeObject<CveRoot>(cveObjectAsJsonStr);

            // TODO: Talk to Anastasia about how to scan the CVE DB
            foreach (var appInfo in _appsInfo)
            {
                var vulnerabilities = cveRoot.vulnerabilities
                    .Where(v => v.cve.configurations
                        .Any(c => c.nodes
                            .Any(n => n.cpeMatch
                                .Any(m => (appInfo.DisplayName != null && m.criteria.Contains(appInfo.DisplayName, StringComparison.OrdinalIgnoreCase)) || (appInfo.Publisher != null && m.criteria.Contains(appInfo.Publisher, StringComparison.OrdinalIgnoreCase)))
                            )
                        )
                    );

                _Vulnerabilities.AddRange(vulnerabilities);
            }
        }
    }
}