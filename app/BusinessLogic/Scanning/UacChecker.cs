using BusinessLogic;
using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;


public class UacChecker : IChecker
{
    public bool UnableToQuery { get; set; } = false;
    public bool IsUacDisabled { get; set; } = false;
    public bool IsUacAtRecommendedLevel { get; set; } = false;
    public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
    public List<SecurityCheck> SecurityCheckResults { get; private set; } = new List<SecurityCheck>();
    public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

    public const String ID = "SK-03";
    public SecurityCheck SecurityCheck { get; private set; }
    public UacChecker()
    {
        SecurityCheck = SecurityCheck.GetInstanceById(ID);
        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
    }

    public void Scan()
    {
        ScanResults.Clear();

        EventAggregator.Instance.FireEvent(BlEvents.CheckingUac);

        ProbeUac();

        if (IsUacDisabled == true)
        {
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
        }
        else
        {
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
        }

        SecurityResults.Add(SecurityCheck);

        EventAggregator.Instance.FireEvent(BlEvents.CheckingUacCompleted);
    }

    private void ProbeUac()
    {

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
                            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;

                            IsUacAtRecommendedLevel = true;
                        }
                        else
                        {
                            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;

                            IsUacAtRecommendedLevel = false;
                        }
                    }
                    else
                    {
                        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;

                        IsUacDisabled = true;

                    }
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                    SecurityCheck.ErrorMessage = "Unable to probe UAC settings";
                }
            }
        }
        catch (Exception ex)
        {
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
            SecurityCheck.ErrorMessage = ex.Message;
            UnableToQuery = true;
        }


    }



}
