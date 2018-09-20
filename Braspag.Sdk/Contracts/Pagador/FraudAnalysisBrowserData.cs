namespace Braspag.Sdk.Contracts.Pagador
{
    public class FraudAnalysisBrowserData
    {
        public bool CookiesAccepted { get; set; }

        public string HostName { get; set; }

        public string Email { get; set; }

        public string Type { get; set; }

        public string IpAddress { get; set; }
    }
}