namespace Braspag.Sdk.Contracts.Pagador
{
    public class WalletData
    {
        public string Type { get; set; }

        public string Walletkey { get; set; }

        public WalletAdditionalData AdditionalData { get; set; }
    }
}