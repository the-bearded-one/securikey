using BusinessLogic;
using BusinessLogic.Scanning;
using Microsoft.Win32;


public class UacChecker : IChecker
{
    public bool UnableToQuery { get; set; } = false;
    public bool IsUacDisabled { get; set; } = false;
    public bool IsUacAtRecommendedLevel { get; set; } = false;
    public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

    public void Scan()
    {
        ScanResults.Clear();

        EventAggregator.Instance.FireEvent(BlEvents.CheckingUac);

        try
        {
            // Open the relevant subkey as read-only
            using (RegistryKey uacKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", false))
            {
                // Read the EnableLUA value
                var enableLUA = uacKey.GetValue("EnableLUA");
                // Read the ConsentPromptBehaviorAdmin value
                var consentPromptBehaviorAdmin = uacKey.GetValue("ConsentPromptBehaviorAdmin");
                // Read the PromptOnSecureDesktop value
                var promptOnSecureDesktop = uacKey.GetValue("PromptOnSecureDesktop");

                if (enableLUA != null && consentPromptBehaviorAdmin != null && promptOnSecureDesktop != null)
                {
                    // Interpret the values
                    if ((int)enableLUA == 1)
                    {
                        if ((int)consentPromptBehaviorAdmin == 2 && (int)promptOnSecureDesktop == 1)
                        {                            
                            IsUacAtRecommendedLevel = true;

                            ScanResult result = new ScanResult();
                            result.ScanType = "User Access Control";
                            result.Severity = Severity.Ok;
                            result.ShortDescription = "You are protected by User Account Control";
                            result.DetailedDescription = $"User Account Control (UAC) is important for security in Windows operating systems because it helps prevent unauthorized or potentially malicious changes to the system by requiring user confirmation or administrator-level permissions for certain actions.";
                            ScanResults.Add(result);
                        }
                        else
                        {                            
                            IsUacAtRecommendedLevel = false;

                            ScanResult result = new ScanResult();
                            result.ScanType = "User Access Control";
                            result.Severity = Severity.Medium;
                            result.ShortDescription = "User Account Control is at an unsafe setting";
                            result.DetailedDescription = $"User Account Control (UAC) is important for security in Windows operating systems because it helps prevent unauthorized or potentially malicious changes to the system by requiring user confirmation or administrator-level permissions for certain actions.";
                            ScanResults.Add(result);
                        }
                    }
                    else
                    {
                        IsUacDisabled = true;

                        ScanResult result = new ScanResult();
                        result.ScanType = "User Access Control";
                        result.Severity = Severity.High;
                        result.ShortDescription = "User Account Control is disabled";
                        result.DetailedDescription = $"User Account Control (UAC) is important for security in Windows operating systems because it helps prevent unauthorized or potentially malicious changes to the system by requiring user confirmation or administrator-level permissions for certain actions.";
                        ScanResults.Add(result);
                    }
                }
                else
                {                    
                    UnableToQuery = true;
                }
            }
        }
        catch (Exception ex)
        {
            UnableToQuery = true;            
        }


        EventAggregator.Instance.FireEvent(BlEvents.CheckingUacCompleted);
    }


}
