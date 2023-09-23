using System.Text;

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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"GUID: {GUID}");
            sb.AppendLine($"Asset: {Asset}");
            sb.AppendLine($"Description: {Description}");
            sb.AppendLine($"Severity: {Severity}");
            sb.AppendLine($"ExploitMaturity: {ExploitMaturity}");
            sb.AppendLine($"FixedIn: {FixedIn}");
            sb.AppendLine($"RemediationRecommendation: {RemediationRecommendation}");
            return sb.ToString();
        }
    }
}