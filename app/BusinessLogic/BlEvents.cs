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
        CheckingSmbEnabled,
        CheckingSmbEnabledCompleted,
        CheckingHeartbleed,
        CheckingHeartbleedCompleted,
        CheckingPasswordExpiry,
        CheckingPasswordExpiryCompleted,
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
    }
}
