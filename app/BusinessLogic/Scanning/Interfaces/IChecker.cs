namespace BusinessLogic.Scanning.Interfaces
{
    internal interface IChecker
    {
        public void Scan();
        public List<ScanResult> ScanResults { get; }
        public List<SecurityCheck> SecurityResults { get; }

    }
}
