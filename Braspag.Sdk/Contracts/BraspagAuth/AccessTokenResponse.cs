using RestSharp.Deserializers;
using System.Net;

namespace Braspag.Sdk.Contracts.BraspagAuth
{
    public class AccessTokenResponse
    {
        public HttpStatusCode HttpStatus { get; set; }

        [DeserializeAs(Name = "access_token")]
        public string Token { get; set; }

        [DeserializeAs(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        [DeserializeAs(Name = "error")]
        public string Error { get; set; }

        [DeserializeAs(Name = "error_description")]
        public string ErrorDescription { get; set; }
    }
}