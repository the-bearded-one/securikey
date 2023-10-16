namespace BusinessLogic.Scanning.POCOs
{
    public class Vulnerability
    {        
        public string? Version { get; set; }
        public string? Severity { get; set; }
        public string? CVE { get; set; }
    }

    public class ApplicationInfo
    {
        public string? Application { get; set; }
        public List<Vulnerability>? Vulnerabilities { get; set; }
    }

}
