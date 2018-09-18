using Braspag.Sdk.CartaoProtegido;
using Braspag.Sdk.Contracts.CartaoProtegido;
using System;
using System.Threading.Tasks;
using Environment = Braspag.Sdk.Common.Environment;
using MerchantCredentials = Braspag.Sdk.Contracts.CartaoProtegido.MerchantCredentials;

namespace Braspag.Sdk.NetCore.ExampleApp
{
    public class CartaoProtegidoDemo
    {
        public static void Run()
        {
            Console.WriteLine("CARTAO PROTEGIDO");
            Console.WriteLine("=====================================");

            /* Criação do Cliente Cartão Protegido */
            var cartaoProtegidoClient = new CartaoProtegidoClient(new CartaoProtegidoClientOptions
            {
                Environment = Environment.Sandbox,
                Credentials = new MerchantCredentials
                {
                    MerchantKey = "106c8a0c-89a4-4063-bf50-9e6c8530593b"
                }
            });

            /* Salvar cartão */
            var saveCreditCardResponse = SaveCreditCardAsync(cartaoProtegidoClient).Result;

            Console.WriteLine("Card tokenized");
            Console.WriteLine($"Card Token: {saveCreditCardResponse.JustClickKey}");
            Console.WriteLine();

            /* Recuperação dos dados de cartão via token */
            var getCreditCardResponse = GetCreditCardAsync(cartaoProtegidoClient, saveCreditCardResponse.JustClickKey).Result;

            Console.WriteLine("Card data obtained from secure server");
            Console.WriteLine($"Card Number: {getCreditCardResponse.CardNumber}");
            Console.WriteLine($"Card Holder: {getCreditCardResponse.CardHolder}");
            Console.WriteLine($"Card Expiration: {getCreditCardResponse.CardExpiration}");
            Console.WriteLine();
        }

        private static async Task<SaveCreditCardResponse> SaveCreditCardAsync(ICartaoProtegidoClient client)
        {
            var request = new SaveCreditCardRequest
            {
                CustomerName = "Bjorn Ironside",
                CustomerIdentification = "762.502.520-96",
                CardHolder = "BJORN IRONSIDE",
                CardExpiration = "10/2025",
                CardNumber = "1000100010001000",
                JustClickAlias = DateTime.Now.Ticks.ToString()
            };

            return await client.SaveCreditCardAsync(request);
        }

        private static async Task<GetCreditCardResponse> GetCreditCardAsync(ICartaoProtegidoClient client, string token)
        {
            var request = new GetCreditCardRequest
            {
                JustClickKey = token,
                RequestId = Guid.NewGuid()
            };

            return await client.GetCreditCardAsync(request);
        }
    }
}