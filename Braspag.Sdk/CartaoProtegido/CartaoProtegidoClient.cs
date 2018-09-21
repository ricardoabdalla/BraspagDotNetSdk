using Braspag.Sdk.Common;
using Braspag.Sdk.Contracts.CartaoProtegido;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Environment = Braspag.Sdk.Common.Environment;

namespace Braspag.Sdk.CartaoProtegido
{
    public class CartaoProtegidoClient : ICartaoProtegidoClient
    {
        private readonly MerchantCredentials _credentials;

        private readonly string _assemblyVersion;

        private CartaoProtegidoClientOptions _options;

        public IRestClient RestClient { get; }

        public IDeserializer XmlDeserializer { get; }

        public CartaoProtegidoClient(CartaoProtegidoClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _credentials = options.Credentials;
            RestClient = _options.Environment == Environment.Production ? new RestClient { BaseUrl = new Uri(Endpoints.CartaoProtegidoProduction) } : new RestClient { BaseUrl = new Uri(Endpoints.CartaoProtegidoSandbox) };
            XmlDeserializer = new XmlDeserializer();
            _assemblyVersion = System.Reflection.Assembly.GetAssembly(typeof(ICartaoProtegidoClient)).GetName().Version.ToString();
        }

        public async Task<GetCreditCardResponse> GetCreditCardAsync(GetCreditCardRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/cartaoprotegido.asmx", Method.POST)
            {
                RequestFormat = DataFormat.Xml,
                XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer()
            };

            var sb = new StringBuilder();
            sb.AppendLine("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.AppendLine("<soap:Body>");
            sb.AppendLine("<GetCreditCard xmlns=\"http://www.cartaoprotegido.com.br/WebService/\">");
            sb.AppendLine("<getCreditCardRequestWS>");
            sb.AppendLine(request.RequestId.HasValue
                ? $"<RequestId>{request.RequestId.Value}</RequestId>"
                : $"<RequestId>{Guid.NewGuid()}</RequestId>");
            sb.AppendLine($"<MerchantKey>{currentCredentials.MerchantKey}</MerchantKey>");
            sb.AppendLine($"<JustClickKey>{request.JustClickKey}</JustClickKey>");
            sb.AppendLine($"<JustClickAlias>{request.JustClickAlias}</JustClickAlias>");
            sb.AppendLine("</getCreditCardRequestWS>");
            sb.AppendLine("</GetCreditCard>");
            sb.AppendLine("</soap:Body>");
            sb.AppendLine("</soap:Envelope>");

            httpRequest.AddHeader("Braspag-SDK-Version", _assemblyVersion);
            httpRequest.AddParameter("text/xml", sb.ToString(), ParameterType.RequestBody);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClient.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new GetCreditCardResponse
                {
                    HttpStatus = httpResponse.StatusCode
                };
            }

            var jsonResponse = XmlDeserializer.Deserialize<GetCreditCardResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<GetMaskedCreditCardResponse> GetMaskedCreditCardAsync(GetMaskedCreditCardRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/cartaoprotegido.asmx", Method.POST)
            {
                RequestFormat = DataFormat.Xml,
                XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer()
            };

            var sb = new StringBuilder();
            sb.AppendLine("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.AppendLine("<soap:Body>");
            sb.AppendLine("<GetMaskedCreditCard xmlns=\"http://www.cartaoprotegido.com.br/WebService/\">");
            sb.AppendLine("<getMaskedCreditCardRequestWS>");
            sb.AppendLine(request.RequestId.HasValue
                ? $"<RequestId>{request.RequestId.Value}</RequestId>"
                : $"<RequestId>{Guid.NewGuid()}</RequestId>");
            sb.AppendLine($"<MerchantKey>{currentCredentials.MerchantKey}</MerchantKey>");
            sb.AppendLine($"<JustClickKey>{request.JustClickKey}</JustClickKey>");
            sb.AppendLine($"<JustClickAlias>{request.JustClickAlias}</JustClickAlias>");
            sb.AppendLine("</getMaskedCreditCardRequestWS>");
            sb.AppendLine("</GetMaskedCreditCard>");
            sb.AppendLine("</soap:Body>");
            sb.AppendLine("</soap:Envelope>");

            httpRequest.AddHeader("Braspag-SDK-Version", _assemblyVersion);
            httpRequest.AddParameter("text/xml", sb.ToString(), ParameterType.RequestBody);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClient.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new GetMaskedCreditCardResponse
                {
                    HttpStatus = httpResponse.StatusCode
                };
            }

            var jsonResponse = XmlDeserializer.Deserialize<GetMaskedCreditCardResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<GetJustClickKeyResponse> GetJustClickKeyAsync(GetJustClickKeyRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/cartaoprotegido.asmx", Method.POST)
            {
                RequestFormat = DataFormat.Xml,
                XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer()
            };

            var sb = new StringBuilder();
            sb.AppendLine("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.AppendLine("<soap:Body>");
            sb.AppendLine("<GetJustClickKey xmlns=\"http://www.cartaoprotegido.com.br/WebService/\">");
            sb.AppendLine("<getJustClickKeyRequestWS>");
            sb.AppendLine(request.RequestId.HasValue
                ? $"<RequestId>{request.RequestId.Value}</RequestId>"
                : $"<RequestId>{Guid.NewGuid()}</RequestId>");
            sb.AppendLine($"<MerchantKey>{currentCredentials.MerchantKey}</MerchantKey>");
            sb.AppendLine($"<SaveCreditCardRequestId>{request.SaveCreditCardRequestId}</SaveCreditCardRequestId>");
            sb.AppendLine("</getJustClickKeyRequestWS>");
            sb.AppendLine("</GetJustClickKey>");
            sb.AppendLine("</soap:Body>");
            sb.AppendLine("</soap:Envelope>");

            httpRequest.AddHeader("Braspag-SDK-Version", _assemblyVersion);
            httpRequest.AddParameter("text/xml", sb.ToString(), ParameterType.RequestBody);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClient.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new GetJustClickKeyResponse
                {
                    HttpStatus = httpResponse.StatusCode
                };
            }

            var jsonResponse = XmlDeserializer.Deserialize<GetJustClickKeyResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<SaveCreditCardResponse> SaveCreditCardAsync(SaveCreditCardRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/cartaoprotegido.asmx", Method.POST)
            {
                RequestFormat = DataFormat.Xml,
                XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer()
            };

            var sb = new StringBuilder();
            sb.AppendLine("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.AppendLine("<soap:Body>");
            sb.AppendLine("<SaveCreditCard xmlns=\"http://www.cartaoprotegido.com.br/WebService/\">");
            sb.AppendLine("<saveCreditCardRequestWS>");
            sb.AppendLine(request.RequestId.HasValue
                ? $"<RequestId>{request.RequestId.Value}</RequestId>"
                : $"<RequestId>{Guid.NewGuid()}</RequestId>");
            sb.AppendLine($"<MerchantKey>{currentCredentials.MerchantKey}</MerchantKey>");
            sb.AppendLine($"<CustomerIdentification>{request.CustomerIdentification}</CustomerIdentification>");
            sb.AppendLine($"<CustomerName>{request.CustomerName}</CustomerName>");
            sb.AppendLine($"<CardHolder>{request.CardHolder}</CardHolder>");
            sb.AppendLine($"<CardNumber>{request.CardNumber}</CardNumber>");
            sb.AppendLine($"<CardExpiration>{request.CardExpiration}</CardExpiration>");
            sb.AppendLine($"<JustClickAlias>{request.JustClickAlias}</JustClickAlias>");
            sb.AppendLine("<DataCollection />");
            sb.AppendLine("</saveCreditCardRequestWS>");
            sb.AppendLine("</SaveCreditCard>");
            sb.AppendLine("</soap:Body>");
            sb.AppendLine("</soap:Envelope>");

            httpRequest.AddHeader("Braspag-SDK-Version", _assemblyVersion);
            httpRequest.AddParameter("text/xml", sb.ToString(), ParameterType.RequestBody);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClient.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new SaveCreditCardResponse
                {
                    HttpStatus = httpResponse.StatusCode
                };
            }

            var jsonResponse = XmlDeserializer.Deserialize<SaveCreditCardResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }

        public async Task<InvalidateCreditCardResponse> InvalidateCreditCardAsync(InvalidateCreditCardRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantKey))
                throw new InvalidOperationException("Invalid credentials: MerchantKey is null");

            var httpRequest = new RestRequest(@"v2/cartaoprotegido.asmx", Method.POST)
            {
                RequestFormat = DataFormat.Xml,
                XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer()
            };

            var sb = new StringBuilder();
            sb.AppendLine("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.AppendLine("<soap:Body>");
            sb.AppendLine("<InvalidateCreditCard xmlns=\"http://www.cartaoprotegido.com.br/WebService/\">");
            sb.AppendLine("<invalidateCreditCardRequestWS>");
            sb.AppendLine(request.RequestId.HasValue
                ? $"<RequestId>{request.RequestId.Value}</RequestId>"
                : $"<RequestId>{Guid.NewGuid()}</RequestId>");
            sb.AppendLine($"<MerchantKey>{currentCredentials.MerchantKey}</MerchantKey>");
            sb.AppendLine($"<JustClickKey>{request.JustClickKey}</JustClickKey>");
            sb.AppendLine($"<JustClickAlias>{request.JustClickAlias}</JustClickAlias>");
            sb.AppendLine("</invalidateCreditCardRequestWS>");
            sb.AppendLine("</InvalidateCreditCard>");
            sb.AppendLine("</soap:Body>");
            sb.AppendLine("</soap:Envelope>");

            httpRequest.AddHeader("Braspag-SDK-Version", _assemblyVersion);
            httpRequest.AddParameter("text/xml", sb.ToString(), ParameterType.RequestBody);

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClient.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new InvalidateCreditCardResponse
                {
                    HttpStatus = httpResponse.StatusCode
                };
            }

            var jsonResponse = XmlDeserializer.Deserialize<InvalidateCreditCardResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }
    }
}