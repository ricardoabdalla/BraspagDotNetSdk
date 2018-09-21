namespace Braspag.Sdk.Contracts.Pagador
{
    public class RecurrentPaymentDataResponse
    {
        public string RecurrentPaymentId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string NextRecurrency { get; set; }

        public string Interval { get; set; }

        public bool? AuthorizeNow { get; set; }

        public int ReasonCode { get; set; }

        public string ReasonMessage { get; set; }
    }
}