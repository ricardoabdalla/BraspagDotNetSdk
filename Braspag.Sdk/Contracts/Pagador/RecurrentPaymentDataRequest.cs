namespace Braspag.Sdk.Contracts.Pagador
{
    public class RecurrentPaymentDataRequest
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Interval { get; set; }

        public bool? AuthorizeNow { get; set; }
    }
}