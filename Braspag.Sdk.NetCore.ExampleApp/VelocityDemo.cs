using Braspag.Sdk.BraspagAuth;
using Braspag.Sdk.Common;
using Braspag.Sdk.Contracts.BraspagAuth;
using Braspag.Sdk.Contracts.Velocity;
using Braspag.Sdk.Velocity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Environment = Braspag.Sdk.Common.Environment;

namespace Braspag.Sdk.NetCore.ExampleApp
{
    public class VelocityDemo
    {
        public static void Run()
        {
            Console.WriteLine("VELOCITY");
            Console.WriteLine("=====================================");

            /* Criação do Token de Acesso OAUTH via Braspag Auth */
            var braspagAuthClient = new BraspagAuthClient(new BraspagAuthClientOptions
            {
                Environment = Environment.Sandbox
            });

            var authRequest = new AccessTokenRequest
            {
                GrantType = OAuthGrantType.ClientCredentials,
                ClientId = "5d85902e-592a-44a9-80bb-bdda74d51bce",
                ClientSecret = "mddRzd6FqXujNLygC/KxOfhOiVhlUr2kjKPsOoYHwhQ=",
                Scope = "VelocityApp"
            };

            var authResponse = braspagAuthClient.CreateAccessTokenAsync(authRequest).Result;

            /* Criação do Cliente Velocity */
            var velocityClient = new VelocityClient(new VelocityClientOptions
            {
                Environment = Environment.Sandbox,
                Credentials = new MerchantCredentials
                {
                    MerchantId = "94E5EA52-79B0-7DBA-1867-BE7B081EDD97",
                    AccessToken = authResponse.Token
                }
            });

            /* Analisando uma transação com Velocity */
            var analysisResponse = PerformAnalysisAsync(velocityClient).Result;

            Console.WriteLine("Transaction analyzed");
            Console.WriteLine($"Score: {analysisResponse.AnalysisResult.Score}");
            Console.WriteLine($"Status: {analysisResponse.AnalysisResult.Status}");
            Console.WriteLine($"Accept By WhiteList: {analysisResponse.AnalysisResult.AcceptByWhiteList}");
            Console.WriteLine($"Reject By BlackList: {analysisResponse.AnalysisResult.RejectByBlackList}");
            Console.WriteLine();
        }

        private static async Task<AnalysisResponse> PerformAnalysisAsync(IVelocityClient client)
        {
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
                    Identity = "76250252096",
                    IpAddress = "127.0.0.1",
                    Email = "bjorn.ironside@vikings.com.br",
                    BirthDate = "1982-06-30",
                    Phones = new List<PhoneData>
                    {
                        new PhoneData
                        {
                            Type = "Cellphone",
                            Number = "999999999",
                            Ddi = "55",
                            Ddd = "11"
                        }
                    },
                    Billing = new AddressData
                    {
                        Street = "Alameda Xingu",
                        Number = "512",
                        Neighborhood = "Alphaville",
                        City = "Barueri",
                        State = "SP",
                        Country = "BR",
                        ZipCode = "06455-030"
                    }
                }
            };

            return await client.PerformAnalysisAsync(request);
        }
    }
}