using BusinessLogic;

public class ScanResults
{
    public string ScanType { get; set; }
    public string ShortDescription { get; set; }
    public string DetailedDescription { get; set; }
    public string Remediation { get; set; }
    public Severity Severity { get; set; }
}