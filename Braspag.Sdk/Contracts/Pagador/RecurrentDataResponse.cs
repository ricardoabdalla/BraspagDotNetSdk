namespace Braspag.Sdk.Contracts.Pagador
{
    public class RecurrentDataResponse
    {
        public CustomerData Customer { get; set; }

        public RecurrentPaymentData RecurrentPayment { get; set; }
    }
}