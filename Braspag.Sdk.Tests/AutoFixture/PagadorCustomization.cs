using AutoFixture;
using Braspag.Sdk.Contracts.Pagador;
using Braspag.Sdk.Pagador;
using System;
using Environment = Braspag.Sdk.Common.Environment;

namespace Braspag.Sdk.Tests.AutoFixture
{
    public class PagadorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<PagadorClientOptions>(
                c => c
                    .With(x => x.Environment, Environment.Sandbox)
                    .With(x => x.Credentials, new MerchantCredentials
                    {
                        MerchantId = "33B6AC07-C48D-4F13-A5B9-D3516A378A0C",
                        MerchantKey = "d6Rb3OParKvLfzNrURzwcT0f1lzNazS1o19yP6Y8"
                    }));

            fixture.Customize<SaleRequest>(
                c => c
                    .With(x => x.MerchantOrderId, DateTime.Now.Ticks.ToString())
                    .With(x => x.Customer, new CustomerData
                    {
                        Name = "Bjorn Ironside",
                        Email = "bjorn.ironside@vikings.com.br",
                        IdentityType = "CPF",
                        Identity = "762.502.520-96"
                    })
                    .With(x => x.Payment, new PaymentDataRequest
                    {
                        Amount = 1000,
                        Provider = "Simulado",
                        Type = "CreditCard",
                        Currency = "BRL",
                        Country = "BRA",
                        Installments = 1,
                        SoftDescriptor = "Braspag SDK",
                        CreditCard = new CreditCardData
                        {
                            CardNumber = "4485623136297301",
                            Holder = "BJORN IRONSIDE",
                            ExpirationDate = "12/2025",
                            SecurityCode = "123",
                            Brand = "visa"
                        },
                        Capture = false,
                        Authenticate = false,
                        Recurrent = false,
                        Credentials = null,
                        Assignor = null,
                        DebitCard = null,
                        ExternalAuthentication = null,
                        FraudAnalysis = null,
                        ExtraDataCollection = null,
                        Wallet = null,
                        RecurrentPayment = null,
                        ReturnUrl = null
                    }));
        }
    }
}