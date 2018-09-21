using System.Net;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class RecurrentDataResponse
    {
        public HttpStatusCode HttpStatus { get; set; }

        public CustomerData Customer { get; set; }

        public RecurrentPaymentData RecurrentPayment { get; set; }
    }
}