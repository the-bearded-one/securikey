namespace BusinessLogic.Scanning.POCOs
{
    public class ApplicationInfo
    {
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public string Publisher { get; set; }
        public override string ToString()
        {
            string theString = $"DisplayName: {DisplayName}    DisplayVersion: {DisplayVersion}    Publisher: {Publisher}";
            return theString;
        }
    }
}
