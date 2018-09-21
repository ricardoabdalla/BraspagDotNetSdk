using Braspag.Sdk.BraspagAuth;
using Braspag.Sdk.Common;
using Braspag.Sdk.Contracts.BraspagAuth;
using Braspag.Sdk.Tests.AutoFixture;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Braspag.Sdk.Tests.IntegrationTests
{
    public class BraspagAuthClientTests
    {
        [Theory, AutoNSubstituteData]
        public async Task CreateAccessTokenAsync_ForValidCredentials_ReturnsAccessToken(BraspagAuthClient sut)
        {
            var request = new AccessTokenRequest
            {
                GrantType = OAuthGrantType.ClientCredentials,
                ClientId = "b7554867-c69a-4fd2-b059-40c08f6f924a",
                ClientSecret = "9tV4mICf6YmKiRkfPce8+jHc4M2hLGgBdMCxnlj3LDY="
            };

            var response = await sut.CreateAccessTokenAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.HttpStatus);
            Assert.NotNull(response.Token);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateAccessTokenAsync_WhenClientIdIsInvalid_ReturnsInvalidClientError(BraspagAuthClient sut)
        {
            var request = new AccessTokenRequest
            {
                GrantType = OAuthGrantType.ClientCredentials,
                ClientId = "b7554867-5D6B-0000-0000-7DBA4F26026F",
                ClientSecret = "43VtT15I5l7BocriFWPAlhTjd55VupXq5jVVbubJWLA="
            };

            var response = await sut.CreateAccessTokenAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatus);
            Assert.Equal("invalid_client", response.Error);
            Assert.NotNull(response.Error);
            Assert.NotNull(response.ErrorDescription);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateAccessTokenAsync_WhenClientSecretIsInvalid_ReturnsInvalidClientError(BraspagAuthClient sut)
        {
            var request = new AccessTokenRequest
            {
                GrantType = OAuthGrantType.ClientCredentials,
                ClientId = "b7554867-c69a-4fd2-b059-40c08f6f924a",
                ClientSecret = "43VtT15I5l7BocriFWPAlhTjd55VupXq5jVVbu99999="
            };

            var response = await sut.CreateAccessTokenAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatus);
            Assert.Equal("invalid_client", response.Error);
            Assert.NotNull(response.Error);
            Assert.NotNull(response.ErrorDescription);
        }
    }
}