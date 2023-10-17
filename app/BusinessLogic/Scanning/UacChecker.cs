using BusinessLogic;
using BusinessLogic.Scanning;
using Microsoft.Win32;


public class UacChecker : IChecker
{
    public bool UnableToQuery { get; set; } = false;
    public bool IsUacDisabled { get; set; } = false;
    public bool IsUacAtRecommendedLevel { get; set; } = false;

    public void Scan()
    {
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
                        }
                        else
                        {                            
                            IsUacAtRecommendedLevel = false;
                        }
                    }
                    else
                    {
                        IsUacDisabled = true;                        
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
