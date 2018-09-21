using Braspag.Sdk.Common;
using Braspag.Sdk.Contracts.Velocity;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Environment = Braspag.Sdk.Common.Environment;

namespace Braspag.Sdk.Velocity
{
    public class VelocityClient : IVelocityClient
    {
        private readonly MerchantCredentials _credentials;

        private readonly string _assemblyVersion;

        private VelocityClientOptions _options;

        public IRestClient RestClient { get; }

        public IDeserializer JsonDeserializer { get; }

        public VelocityClient(VelocityClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _credentials = options.Credentials;
            RestClient = _options.Environment == Environment.Production ? new RestClient { BaseUrl = new Uri(Endpoints.VelocityApiProduction) } : new RestClient { BaseUrl = new Uri(Endpoints.VelocityApiSandbox) };
            JsonDeserializer = new JsonDeserializer();
            _assemblyVersion = System.Reflection.Assembly.GetAssembly(typeof(IVelocityClient)).GetName().Version.ToString();
        }

        public async Task<AnalysisResponse> PerformAnalysisAsync(AnalysisRequest request, MerchantCredentials credentials = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_credentials == null && credentials == null)
                throw new InvalidOperationException("Credentials are null");

            var currentCredentials = credentials ?? _credentials;

            if (string.IsNullOrWhiteSpace(currentCredentials.MerchantId))
                throw new InvalidOperationException("Invalid credentials: MerchantId is null");

            if (string.IsNullOrWhiteSpace(currentCredentials.AccessToken))
                throw new InvalidOperationException("Invalid credentials: AccessToken is null");

            var httpRequest = new RestRequest(@"analysis/v2/", Method.POST) { RequestFormat = DataFormat.Json };
            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("MerchantId", currentCredentials.MerchantId);
            httpRequest.AddHeader("Authorization", $"Bearer {currentCredentials.AccessToken}");
            httpRequest.AddHeader("RequestId", Guid.NewGuid().ToString());
            httpRequest.AddHeader("Braspag-SDK-Version", _assemblyVersion);
            httpRequest.AddBody(new { request.Transaction, request.Card, request.Customer });

            var cancellationTokenSource = new CancellationTokenSource();

            var httpResponse = await RestClient.ExecuteTaskAsync(httpRequest, cancellationTokenSource.Token);

            if (httpResponse.StatusCode != HttpStatusCode.Created)
            {
                return new AnalysisResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ErrorDataCollection = JsonDeserializer.Deserialize<List<ErrorData>>(httpResponse)
                };
            }

            var jsonResponse = JsonDeserializer.Deserialize<AnalysisResponse>(httpResponse);
            jsonResponse.HttpStatus = httpResponse.StatusCode;
            return jsonResponse;
        }
    }
}