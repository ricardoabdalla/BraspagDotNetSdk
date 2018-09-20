namespace Braspag.Sdk.Contracts.Pagador
{
    public class DebitCardData
    {
        public string CardNumber { get; set; }

        public string Holder { get; set; }

        public string ExpirationDate { get; set; }

        public string SecurityCode { get; set; }

        public string Brand { get; set; }

        public bool? SaveCard { get; set; }
    }
}