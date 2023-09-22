namespace BusinessLogic
{
    public class Issue
    {
        long Uid { get; set; }
        Severity Severity { get; set; }
        ExploitMaturity ExploitMaturity { get; set; }
        string FixedIn { get; set; }
        string RemediationRecommendation { get; set; }
    }
}