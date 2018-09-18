using System;
using Braspag.Sdk.BraspagAuth;
using Braspag.Sdk.Common;
using Braspag.Sdk.Contracts.BraspagAuth;
using Braspag.Sdk.Contracts.Velocity;
using Braspag.Sdk.Tests.AutoFixture;
using Braspag.Sdk.Velocity;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Braspag.Sdk.Tests
{
    public class VelocityClientTests
    {
        [Theory, AutoNSubstituteData]
        public async Task PerformAnalysisAsync_ForValidCreditCard_ReturnsAuthorized(VelocityClient sut, BraspagAuthClient authClient)
        {
            var authRequest = new AccessTokenRequest
            {
                GrantType = OAuthGrantType.ClientCredentials,
                ClientId = "5d85902e-592a-44a9-80bb-bdda74d51bce",
                ClientSecret = "mddRzd6FqXujNLygC/KxOfhOiVhlUr2kjKPsOoYHwhQ=",
                Scope = "VelocityApp"
            };

            var authResponse = await authClient.CreateAccessTokenAsync(authRequest);

            Assert.Equal(HttpStatusCode.OK, authResponse.HttpStatus);

            var request = new AnalysisRequest
            {
                Transaction = new TransactionData
                {
                    OrderId = DateTime.Now.Ticks.ToString(),
                    Date = DateTime.UtcNow.ToString("O"),
                    Amount = 1000
                },
                Card = new CardData
                {
                    Holder = "BJORN IRONSIDE",
                    Brand = "visa",
                    Number = "1000100010001000",
                    Expiration = "10/2025"
                },
                Customer = new CustomerData
                {
                    Name = "Bjorn Ironside",
                    Identity = "762.502.520-96",
                    IpAddress = "127.0.0.1",
                    Email = "bjorn.ironside@vikings.com.br"
                }
            };

            var response = await sut.PerformAnalysisAsync(request, new MerchantCredentials { AccessToken = authResponse.Token, MerchantId = "94E5EA52-79B0-7DBA-1867-BE7B081EDD97" } );

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.NotNull(response.AnalysisResult);
            Assert.NotNull(response.Transaction);
            Assert.NotNull(response.RequestId);
            Assert.Null(response.ErrorDataCollection);
        }
    }
}
