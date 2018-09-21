using System.Collections.Generic;
using System.Net;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class PaymentIdResponse
    {
        public HttpStatusCode HttpStatus { get; set; }

        public List<PaymentIdData> Payments { get; set; }
    }
}