namespace BusinessLogic
{
    public enum Severity
    {
        Ok,
        Low,
        Medium,
        High,
        Critical
    }

    public static class Severities
    {
        public const string CRITICAL = "Critical";
        public const string HIGH = "High";
        public const string MEDIUM = "Medium";
        public const string LOW = "Low";
    }

    public class SeverityComparer : IComparer<SecurityCheck>
    {
        private Dictionary<string, int> severityOrder = new Dictionary<string, int>
        {
            { Severities.CRITICAL, 1 },
            { Severities.HIGH, 2 },
            { Severities.MEDIUM, 3 },
            { Severities.LOW, 4 }
        };

        public int Compare(SecurityCheck x, SecurityCheck y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentNullException();
            }

            int xOrder = severityOrder.TryGetValue(x.Severity.Rating, out var xVal) ? xVal : int.MaxValue;
            int yOrder = severityOrder.TryGetValue(y.Severity.Rating, out var yVal) ? yVal : int.MaxValue;

            return xOrder.CompareTo(yOrder);
        }
    }

}
