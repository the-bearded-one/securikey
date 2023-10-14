using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public enum BlEvents
    {
        SystemScanStarted,
        SystemScanStopped,
        SystemScanAbortedError,
        SystemScanCompleted,
        CveCheckStarted,
        CveCheckCompleted,
        CheckingInternetStatus,
        CheckingInternetStatusCompleted,
        CheckingSecurityProducts,
        CheckingSecurityProductsCompleted,
        CheckingWindowsVersion,
        CheckingWindowsVersionCompleted,
        CheckingApplicationVersions,
        CheckingApplicationVersionsCompleted,
        CheckingElevatedUser,
        CheckingElevatedUserCompleted,
        CheckingWindowsScriptingHost,
        CheckingWindowsScriptingHostCompleted,
        CheckingRdpEnabled,
        CheckingRdpEnabledCompleted
    }
}
