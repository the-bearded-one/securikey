public class ScanResults
{
    public bool InternetConnected { get; set; }
    public List<int>? OpenPorts { get; set; }
    public int WindowsVersionMajor { get; set; }
    public int WindowsVersionMinor { get; set; }
    public int WindowsVersionBuild { get; set; }
    public int WindowsVersionRevision { get; set; }
}