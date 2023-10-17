namespace BusinessLogic.Scanning
{
    internal interface IChecker
    {
        public void Scan();
        public List<ScanResult> ScanResults { get; }
    }
}
