﻿using BusinessLogic.Scanning;
using BusinessLogic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]
    public class SbomGeneratorTests
    {
        [TestMethod]
        public void RunSbomScan()
        {
            var sbomGenerator = new SbomGenerator();
            var appInfos = sbomGenerator.GetInstalledAppInfo();
            foreach (var appInfo in appInfos)
            {
                Debug.WriteLine(appInfo.ToString());
            }
            Assert.IsTrue(appInfos.Count > 0);
        }
    }
}
