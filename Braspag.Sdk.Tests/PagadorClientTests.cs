using Braspag.Sdk.Contracts.Pagador;
using Braspag.Sdk.Pagador;
using Braspag.Sdk.Tests.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using RestSharp;
using Xunit;

namespace Braspag.Sdk.Tests
{
    public class PagadorClientTests
    {
        #region CreateSaleAsync

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_ForValidCreditCard_ReturnsAuthorized(PagadorClient sut, SaleRequest request)
        {
            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_ForValidCreditCardWithAutomaticCapture_ReturnsPaymentConfirmed(PagadorClient sut, SaleRequest request)
        {
            request.Payment.Capture = true;
            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.PaymentConfirmed, response.Payment.Status);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_WithFullCustomerData_ReturnsAuthorized(PagadorClient sut, SaleRequest request)
        {
            request.Customer.Address = new AddressData
            {
                Street = "Alameda Xingu",
                Number = "512",
                Complement = "27 andar",
                District = "Alphaville",
                City = "Barueri",
                State = "SP",
                Country = "Brasil",
                ZipCode = "06455-030"
            };

            request.Customer.DeliveryAddress = new AddressData
            {
                Street = "Av. Marechal Camara",
                Number = "160",
                Complement = "sala 934",
                District = "Centro",
                City = "Rio de Janeiro",
                State = "RJ",
                Country = "Brasil",
                ZipCode = "20020-080"
            };

            request.Customer.Birthdate = "1982-06-30";
            request.Customer.Mobile = "(55) 11 99999-9999";
            request.Customer.Phone = "(55) 11 9999-9999";

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Customer.Address);
            Assert.NotNull(response.Customer.DeliveryAddress);
            Assert.Equal("1982-06-30", response.Customer.Birthdate);
            Assert.Equal("(55) 11 99999-9999", response.Customer.Mobile);
            Assert.Equal("(55) 11 9999-9999", response.Customer.Phone);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_WithAvsAnalysis_ReturnsAuthorized(PagadorClient sut, SaleRequest request)
        {
            request.Payment.CreditCard.Avs = new AvsData
            {
                Street = "Alameda Xingu",
                Number = "512",
                Complement = "27 andar",
                District = "Alphaville",
                ZipCode = "04604007",
                Cpf = "76250252096"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.CreditCard.Avs);
            Assert.Equal("S", response.Payment.CreditCard.Avs.ReturnCode);
            Assert.Equal(3, response.Payment.CreditCard.Avs.Status);
        }

        #endregion
    }
}
