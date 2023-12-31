﻿namespace BusinessLogic
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
        CheckingSecurityProductsAntivirus,
        CheckingSecurityProductsAntivirusCompleted,
        CheckingSecurityProductsFirewall,
        CheckingSecurityProductsFirewallCompleted,
        CheckingHeartbleed,
        CheckingHeartbleedCompleted,
        CheckingSecurityProductsSpyware,
        CheckingSecurityProductsSpywareCompleted,
        CheckingWindowsVersion,
        CheckingWindowsVersionCompleted,
        CheckingApplicationVersions,
        CheckingApplicationVersionsCompleted,
        CheckingElevatedUser,
        CheckingElevatedUserCompleted,
        CheckingWindowsScriptingHost,
        CheckingWindowsScriptingHostCompleted,
        CheckingRdpEnabled,
        CheckingRdpEnabledCompleted,
        CheckingSecureBootEnabled,
        CheckingSecureBootEnabledCompleted,
        CheckingFirewall,
        CheckingFirewallCompleted,
        CheckingRegularUpdates,
        CheckingRegularUpdatesCompleted,
        CheckingBitLocker,
        CheckingBitLockerCompleted,
        CheckingUac,
        CheckingUacCompleted,
        CheckingPowerShellExecutionPolicy,
        CheckingPowerShellExecutionPolicyCompleted,
        CheckingNtlmV1Enabled,
        CheckingNtlmV1EnabledComplete,
        CheckingPageFileEncryption,
        CheckingPageFileEncryptionCompleted,
        CheckingAutoRunEnabled,
        CheckingAutoRunEnabledCompleted,
        CheckingSmbClientEnabled,
        CheckingSmbClientEnabledCompleted,
        CheckingSmbServerEnabled,
        CheckingSmbServerEnabledCompleted,
        CheckingTls,
        CheckingTlsCompleted,
        CheckingPasswordExpiry,
        CheckingPasswordExpiryCompleted,
        CheckingRemoteRegistry,
        CheckingRemoteRegistryCompleted,
        CheckingSpooler,
        CheckingSpoolerCompleted,
        CheckingTelnet,
        CheckingTelnetCompleted,
        CheckingIE,
        CheckingIECompleted,
        CheckingWsl,
        CheckingWslCompleted,
        CheckingUnsignedDriverUnelevated,
        CheckingUnsignedDriverUnelevatedCompleted,
        CheckingUnsignedDriverElevated,
        CheckingUnsignedDriverElevatedCompleted,
        CheckingPasswordComplexityPolicy,
        CheckingPasswordComplexityPolicyCompleted,
        CheckingAutoConnectOpenWifi,
        CheckingAutoConnectOpenWifiCompleted,
        CheckingWindowsGuestAccountEnabled,
        CheckingWindowsGuestAccountEnabledCompleted,
        CheckingSandbox,
        CheckingSandboxCompleted,

    }
}
