namespace BusinessLogic
{
    public class Issue
    {
        public Issue()
        {
            // assign a guid on instantiation so we can uniquely identify the issue
            GUID = System.Guid.NewGuid().ToString();
        }

        public string GUID { get; private set; } = string.Empty;
        public string Asset { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Severity Severity { get; set; }
        public ExploitMaturity ExploitMaturity { get; set; }
        public string FixedIn { get; set; } = string.Empty;
        public string RemediationRecommendation { get; set; } = string.Empty;

        public override string ToString()
        {
            string issueData = $"GUID: {GUID}\r\nAsset: {Asset}\r\n"+
                $"Description: {Description}\r\n" +
                $"Severity: {Severity.ToString()}\r\n" +
                $"ExploitMaturity: {ExploitMaturity}\r\n" +
                $"FixedIn: {FixedIn}" +
                $"RemediationRecommendation: {RemediationRecommendation}";
            return issueData;
        }
    }
}