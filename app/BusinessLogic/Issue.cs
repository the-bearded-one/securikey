using System.Text;

namespace BusinessLogic
{
    public class Issue
    {
        public Issue()
        {
            UID = Guid.NewGuid().ToString();
        }

        public string UID { get; set; }
        public string Asset { get; set; }
        public string Description { get; set; }
        public Severity Severity { get; set; }
        public ExploitMaturity ExploitMaturity { get; set; }
        public string FixedIn { get; set; }
        public string RemediationRecommendation { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"UID: {UID}");
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