using Braspag.Sdk.Contracts.Pagador;
using System.Net;
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

        Task<HttpStatusCode> ChangeRecurrencyEndDate(string recurrentPaymentId, string endDate, MerchantCredentials credentials = null);

        Task<HttpStatusCode> ChangeRecurrencyInterval(string recurrentPaymentId, RecurrencyInterval interval, MerchantCredentials credentials = null);

        Task<HttpStatusCode> ChangeRecurrencyDay(string recurrentPaymentId, int day, MerchantCredentials credentials = null);

        Task<HttpStatusCode> ChangeRecurrencyAmount(string recurrentPaymentId, long amount, MerchantCredentials credentials = null);

        Task<HttpStatusCode> ChangeRecurrencyNextPaymentDate(string recurrentPaymentId, string nextPaymentDate, MerchantCredentials credentials = null);

        Task<HttpStatusCode> ChangeRecurrencyPayment(string recurrentPaymentId, PaymentDataRequest payment, MerchantCredentials credentials = null);

        Task<HttpStatusCode> DeactivateRecurrency(string recurrentPaymentId, MerchantCredentials credentials = null);

        Task<HttpStatusCode> ReactivateRecurrency(string recurrentPaymentId, MerchantCredentials credentials = null);

        Task<RecurrentDataResponse> GetRecurrency(string recurrentPaymentId, MerchantCredentials credentials = null);
    }
}