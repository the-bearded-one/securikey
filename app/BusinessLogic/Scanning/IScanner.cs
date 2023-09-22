using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public interface IScanner
    {
        public void Scan();
        public List<Issue> GetIssues();
        public bool IsScanning { get; }
    }
}
