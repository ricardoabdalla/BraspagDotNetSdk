using Braspag.Sdk.Common;
using Braspag.Sdk.Contracts.Pagador;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Environment = Braspag.Sdk.Common.Environment;

namespace Braspag.Sdk.Pagador
{
    public class PagadorClient : IPagadorClient
    {
        private readonly MerchantCredentials _credentials;

        private PagadorClientOptions _options;

        public IRestClient RestClientApi { get; }

        public IRestClient RestClientQueryApi { get; }

        public IDeserializer JsonDeserializer { get; }

        public PagadorClient(PagadorClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _credentials = options.Credentials;
            RestClientApi = _options.Environment == Environment.Production ? new RestClient { BaseUrl = new Uri(Endpoints.PagadorApiProduction) } : new RestClient { BaseUrl = new Uri(Endpoints.PagadorApiSandbox) };
            RestClientQueryApi = _options.Environment == Environment.Production ? new RestClient { BaseUrl = new Uri(Endpoints.PagadorQueryApiProduction) } : new RestClient { BaseUrl = new Uri(Endpoints.PagadorQueryApiSandbox) };
            JsonDeserializer = new JsonDeserializer();
        }

        public async Task<SaleResponse> CreateSaleAsync(SaleRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/sales/", Method.POST) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddBody(new { request.MerchantOrderId, request.Customer, request.Payment });

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.Created)
            {
                return new SaleResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ErrorDataCollection = httpResponse.StatusCode != HttpStatusCode.Forbidden ? JsonDeserializer.Deserialize<List<ErrorData>>(httpResponse) : null
                };
            }

            var jsonResponse = JsonDeserializer.Deserialize<SaleResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<CaptureResponse> CaptureAsync(CaptureRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/sales/{paymentId}/capture", Method.PUT) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("paymentId", request.PaymentId);
            httpRequest.AddQueryParameter("amount", request.Amount.ToString(CultureInfo.InvariantCulture));

            if (request.ServiceTaxAmount.HasValue)
            {
                httpRequest.AddQueryParameter("serviceTaxAmount", request.ServiceTaxAmount.Value.ToString(CultureInfo.InvariantCulture));
            }

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new CaptureResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ErrorDataCollection = JsonDeserializer.Deserialize<List<ErrorData>>(httpResponse)
                };
            }

            var jsonResponse = JsonDeserializer.Deserialize<CaptureResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<VoidResponse> VoidAsync(VoidRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/sales/{paymentId}/void", Method.PUT) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("paymentId", request.PaymentId);
            httpRequest.AddQueryParameter("amount", request.Amount.ToString(CultureInfo.InvariantCulture));

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new VoidResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ErrorDataCollection = JsonDeserializer.Deserialize<List<ErrorData>>(httpResponse)
                };
            }

            var jsonResponse = JsonDeserializer.Deserialize<VoidResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<SaleResponse> GetAsync(string paymentId, MerchantCredentials credentials = null)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
                throw new ArgumentNullException(nameof(paymentId));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/sales/{paymentId}", Method.GET) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("paymentId", paymentId);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientQueryApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new SaleResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ErrorDataCollection = JsonDeserializer.Deserialize<List<ErrorData>>(httpResponse)
                };
            }

            var jsonResponse = JsonDeserializer.Deserialize<SaleResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<PaymentIdResponse> GetByOrderIdAsync(string orderId, MerchantCredentials credentials = null)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new ArgumentNullException(nameof(orderId));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/sales/", Method.GET) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddQueryParameter("merchantOrderId", orderId);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientQueryApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            var jsonResponse = JsonDeserializer.Deserialize<PaymentIdResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<HttpStatusCode> ChangeRecurrencyCustomer(string recurrentPaymentId, CustomerData customer, MerchantCredentials credentials = null)
        {
            if (string.IsNullOrWhiteSpace(recurrentPaymentId))
                throw new ArgumentNullException(nameof(recurrentPaymentId));

            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/recurrentpayment/{recurrentPaymentId}/customer", Method.PUT) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("recurrentPaymentId", recurrentPaymentId);
            httpRequest.AddBody(customer);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);
            return httpResponse.StatusCode;
        }

        public async Task<HttpStatusCode> ChangeRecurrencyEndDate(string recurrentPaymentId, string endDate, MerchantCredentials credentials = null)
        {
            if (string.IsNullOrWhiteSpace(recurrentPaymentId))
                throw new ArgumentNullException(nameof(recurrentPaymentId));

            if (string.IsNullOrWhiteSpace(endDate))
                throw new ArgumentNullException(nameof(endDate));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/recurrentpayment/{recurrentPaymentId}/enddate", Method.PUT) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("recurrentPaymentId", recurrentPaymentId);
            httpRequest.AddBody(endDate);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);
            return httpResponse.StatusCode;
        }

        public async Task<HttpStatusCode> ChangeRecurrencyInterval(string recurrentPaymentId, RecurrencyInterval interval, MerchantCredentials credentials = null)
        {
            if (string.IsNullOrWhiteSpace(recurrentPaymentId))
                throw new ArgumentNullException(nameof(recurrentPaymentId));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/recurrentpayment/{recurrentPaymentId}/interval", Method.PUT) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("recurrentPaymentId", recurrentPaymentId);
            httpRequest.AddBody(interval);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);
            return httpResponse.StatusCode;
        }

        public async Task<HttpStatusCode> ChangeRecurrencyDay(string recurrentPaymentId, int day, MerchantCredentials credentials = null)
        {
            if (string.IsNullOrWhiteSpace(recurrentPaymentId))
                throw new ArgumentNullException(nameof(recurrentPaymentId));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/recurrentpayment/{recurrentPaymentId}/recurrencyday", Method.PUT) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("recurrentPaymentId", recurrentPaymentId);
            httpRequest.AddBody(day);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);
            return httpResponse.StatusCode;
        }

        public async Task<HttpStatusCode> ChangeRecurrencyAmount(string recurrentPaymentId, long amount, MerchantCredentials credentials = null)
        {
            if (string.IsNullOrWhiteSpace(recurrentPaymentId))
                throw new ArgumentNullException(nameof(recurrentPaymentId));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/recurrentpayment/{recurrentPaymentId}/amount", Method.PUT) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("recurrentPaymentId", recurrentPaymentId);
            httpRequest.AddBody(amount);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);
            return httpResponse.StatusCode;
        }

        public async Task<HttpStatusCode> ChangeRecurrencyNextPaymentDate(string recurrentPaymentId, string nextPaymentDate, MerchantCredentials credentials = null)
        {
            if (string.IsNullOrWhiteSpace(recurrentPaymentId))
                throw new ArgumentNullException(nameof(recurrentPaymentId));

            if (string.IsNullOrWhiteSpace(nextPaymentDate))
                throw new ArgumentNullException(nameof(nextPaymentDate));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/recurrentpayment/{recurrentPaymentId}/nextPaymentDate", Method.PUT) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("MerchantKey", currentCredentials.MerchantKey);
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddUrlSegment("recurrentPaymentId", recurrentPaymentId);
            httpRequest.AddBody(nextPaymentDate);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClientApi.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);
            return httpResponse.StatusCode;
        }

        public Task<HttpStatusCode> ChangeRecurrencyPayment(string recurrentPaymentId, PaymentDataRequest payment, MerchantCredentials credentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> DeactivateRecurrency(string recurrentPaymentId, MerchantCredentials credentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> ReactivateRecurrency(string recurrentPaymentId, MerchantCredentials credentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<RecurrentDataResponse> GetRecurrency(string recurrentPaymentId, MerchantCredentials credentials = null)
        {
            throw new NotImplementedException();
        }
    }
}