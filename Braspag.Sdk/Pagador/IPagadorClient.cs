using System.Net;
using Braspag.Sdk.Contracts.Pagador;
using System.Threading.Tasks;

namespace Braspag.Sdk.Pagador
{
    public interface IPagadorClient
    {
        Task<SaleResponse> CreateSaleAsync(SaleRequest request, MerchantCredentials credentials = null);

        Task<CaptureResponse> CaptureAsync(CaptureRequest request, MerchantCredentials credentials = null);

        Task<VoidResponse> VoidAsync(VoidRequest request, MerchantCredentials credentials = null);

        Task<SaleResponse> GetAsync(string paymentId, MerchantCredentials credentials = null);

        Task<PaymentIdResponse> GetByOrderIdAsync(string orderId, MerchantCredentials credentials = null);

        Task<HttpStatusCode> ChangeRecurrencyCustomer(string recurrentPaymentId, CustomerData customer, MerchantCredentials credentials = null);
    }
}