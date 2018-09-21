using Braspag.Sdk.Contracts.Pagador;
using Braspag.Sdk.Pagador;
using Braspag.Sdk.Tests.AutoFixture;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Braspag.Sdk.Tests.IntegrationTests
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

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_WithExternalAuthentication_ReturnsAuthorized(PagadorClient sut, SaleRequest request)
        {
            request.Payment.ExternalAuthentication = new ExternalAuthenticationData
            {
                Cavv = "AABBBlCIIgAAAAARJIgiEL0gDoE=",
                Eci = "5",
                Xid = "dnFoU3R4amdpWTJJdzJRVHNhNDZ"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.ExternalAuthentication);
            Assert.Equal("AABBBlCIIgAAAAARJIgiEL0gDoE=", response.Payment.ExternalAuthentication.Cavv);
            Assert.Equal("5", response.Payment.ExternalAuthentication.Eci);
            Assert.Equal("dnFoU3R4amdpWTJJdzJRVHNhNDZ", response.Payment.ExternalAuthentication.Xid);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_WithAuthentication_ReturnsNotFinished(PagadorClient sut, SaleRequest request)
        {
            request.Payment.Authenticate = true;
            request.Payment.ReturnUrl = "http://www.test.com/redirect";

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.NotFinished, response.Payment.Status);
            Assert.NotNull(response.Payment.AuthenticationUrl);
            Assert.Equal(request.Payment.ReturnUrl, response.Payment.ReturnUrl);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_WhenCardSaveIsTrue_ReturnsAuthorized(PagadorClient sut, SaleRequest request)
        {
            request.Payment.CreditCard.SaveCard = true;

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.CreditCard.CardToken);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_UsingCardToken_ReturnsAuthorized(PagadorClient sut, SaleRequest request)
        {
            request.Payment.CreditCard.Holder = null;
            request.Payment.CreditCard.CardNumber = null;
            request.Payment.CreditCard.Brand = null;
            request.Payment.CreditCard.ExpirationDate = null;
            request.Payment.CreditCard.CardToken = "283f90e4-1a90-4bf7-829f-d9e8f14215f1";

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_UsingDebitCard_ReturnsNotFinished(PagadorClient sut, SaleRequest request)
        {
            request.Payment.Type = "DebitCard";
            request.Payment.CreditCard = null;
            request.Payment.DebitCard = new DebitCardData
            {
                CardNumber = "4551870000000181",
                Holder = "BJORN IRONSIDE",
                ExpirationDate = "12/2025",
                SecurityCode = "123",
                Brand = "visa"
            };
            request.Payment.Authenticate = true;
            request.Payment.ReturnUrl = "http://www.test.com/redirect";

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.NotFinished, response.Payment.Status);
            Assert.NotNull(response.Payment.DebitCard);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_UsingRegisteredBoleto_ReturnsAuthorized(PagadorClient sut, SaleRequest request)
        {
            request.Payment.Type = "Boleto";
            request.Payment.CreditCard = null;
            request.Payment.BoletoNumber = "2017091101";
            request.Payment.Assignor = "Braspag";
            request.Payment.Demonstrative = "Texto demonstrativo";
            request.Payment.ExpirationDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            request.Payment.Identification = "11017523000167";
            request.Payment.Instructions = "Aceitar somente até a data de vencimento.";

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.Assignor);
            Assert.NotNull(response.Payment.Address);
            Assert.NotNull(response.Payment.BarCodeNumber);
            Assert.NotNull(response.Payment.BoletoNumber);
            Assert.NotNull(response.Payment.Demonstrative);
            Assert.NotNull(response.Payment.DigitableLine);
            Assert.NotNull(response.Payment.ExpirationDate);
            Assert.NotNull(response.Payment.Identification);
            Assert.NotNull(response.Payment.Instructions);
            Assert.NotNull(response.Payment.Url);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_UsingRecurrentPayment_ReturnsAuthorized(PagadorClient sut, SaleRequest request)
        {
            request.Payment.RecurrentPayment = new RecurrentPaymentDataRequest
            {
                AuthorizeNow = true,
                EndDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
                Interval = "Monthly"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.RecurrentPayment);
            Assert.NotNull(response.Payment.RecurrentPayment.RecurrentPaymentId);
            Assert.NotNull(response.Payment.RecurrentPayment.NextRecurrency);
            Assert.NotNull(response.Payment.RecurrentPayment.Interval);
            Assert.NotNull(response.Payment.RecurrentPayment.EndDate);
        }

        #endregion

        #region MultiStep_Tests

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_ThenCaptureAsync_ThenVoidAsync_ThenGetAsync(PagadorClient sut, SaleRequest request)
        {
            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);

            var captureRequest = new CaptureRequest
            {
                PaymentId = response.Payment.PaymentId,
                Amount = response.Payment.Amount
            };

            var captureResponse = await sut.CaptureAsync(captureRequest);

            Assert.Equal(HttpStatusCode.OK, captureResponse.HttpStatus);
            Assert.Equal(TransactionStatus.PaymentConfirmed, captureResponse.Status);

            var voidRequest = new VoidRequest
            {
                PaymentId = response.Payment.PaymentId,
                Amount = response.Payment.Amount
            };

            var voidResponse = await sut.VoidAsync(voidRequest);

            Assert.Equal(HttpStatusCode.OK, voidResponse.HttpStatus);
            Assert.Equal(TransactionStatus.Voided, voidResponse.Status);

            var getResponse = await sut.GetAsync(response.Payment.PaymentId);

            Assert.Equal(HttpStatusCode.OK, getResponse.HttpStatus);
            Assert.NotNull(getResponse.MerchantOrderId);
            Assert.NotNull(getResponse.Customer);
            Assert.NotNull(getResponse.Payment);
            Assert.Equal(TransactionStatus.Voided, getResponse.Payment.Status);
        }

        [Theory, AutoNSubstituteData]
        public async Task CreateSaleAsync_ThenGetByOrderIdAsync(PagadorClient sut, SaleRequest request)
        {
            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);

            var getResponse = await sut.GetByOrderIdAsync(response.MerchantOrderId);

            Assert.Equal(HttpStatusCode.OK, getResponse.HttpStatus);
            Assert.NotEmpty(getResponse.Payments);
            Assert.NotNull(getResponse.Payments[0].PaymentId);
        }

        #endregion

        #region Recurrent

        [Theory, AutoNSubstituteData]
        public async Task ChangeRecurrencyCustomer_ReturnsOk(PagadorClient sut, SaleRequest request)
        {
            request.Payment.RecurrentPayment = new RecurrentPaymentDataRequest
            {
                AuthorizeNow = true,
                EndDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
                Interval = "Monthly"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.RecurrentPayment);
            Assert.NotNull(response.Payment.RecurrentPayment.RecurrentPaymentId);
            Assert.NotNull(response.Payment.RecurrentPayment.NextRecurrency);
            Assert.NotNull(response.Payment.RecurrentPayment.Interval);
            Assert.NotNull(response.Payment.RecurrentPayment.EndDate);

            var customer = new CustomerData
            {
                Name = "Ragnar Lothbrok",
                Email = "ragnar.lothbrok@vikings.com.br",
                IdentityType = "CPF",
                Identity = "637.952.420-70"
            };

            var recurrentResponse = await sut.ChangeRecurrencyCustomer(response.Payment.RecurrentPayment.RecurrentPaymentId, customer);

            Assert.Equal(HttpStatusCode.OK, recurrentResponse);
        }

        [Theory, AutoNSubstituteData]
        public async Task ChangeRecurrencyEndDate_ReturnsOk(PagadorClient sut, SaleRequest request)
        {
            request.Payment.RecurrentPayment = new RecurrentPaymentDataRequest
            {
                AuthorizeNow = true,
                EndDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
                Interval = "Monthly"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.RecurrentPayment);
            Assert.NotNull(response.Payment.RecurrentPayment.RecurrentPaymentId);
            Assert.NotNull(response.Payment.RecurrentPayment.NextRecurrency);
            Assert.NotNull(response.Payment.RecurrentPayment.Interval);
            Assert.NotNull(response.Payment.RecurrentPayment.EndDate);

            var endDate = DateTime.UtcNow.AddMonths(4).ToString("yyyy-MM-dd");

            var recurrentResponse = await sut.ChangeRecurrencyEndDate(response.Payment.RecurrentPayment.RecurrentPaymentId, endDate);

            Assert.Equal(HttpStatusCode.OK, recurrentResponse);
        }

        [Theory, AutoNSubstituteData]
        public async Task ChangeRecurrencyInterval_ReturnsOk(PagadorClient sut, SaleRequest request)
        {
            request.Payment.RecurrentPayment = new RecurrentPaymentDataRequest
            {
                AuthorizeNow = true,
                EndDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
                Interval = "Monthly"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.RecurrentPayment);
            Assert.NotNull(response.Payment.RecurrentPayment.RecurrentPaymentId);
            Assert.NotNull(response.Payment.RecurrentPayment.NextRecurrency);
            Assert.NotNull(response.Payment.RecurrentPayment.Interval);
            Assert.NotNull(response.Payment.RecurrentPayment.EndDate);

            var recurrentResponse = await sut.ChangeRecurrencyInterval(response.Payment.RecurrentPayment.RecurrentPaymentId, RecurrencyInterval.Quarterly);

            Assert.Equal(HttpStatusCode.OK, recurrentResponse);
        }

        [Theory, AutoNSubstituteData]
        public async Task ChangeRecurrencyDay_ReturnsOk(PagadorClient sut, SaleRequest request)
        {
            request.Payment.RecurrentPayment = new RecurrentPaymentDataRequest
            {
                AuthorizeNow = true,
                EndDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
                Interval = "Monthly"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.RecurrentPayment);
            Assert.NotNull(response.Payment.RecurrentPayment.RecurrentPaymentId);
            Assert.NotNull(response.Payment.RecurrentPayment.NextRecurrency);
            Assert.NotNull(response.Payment.RecurrentPayment.Interval);
            Assert.NotNull(response.Payment.RecurrentPayment.EndDate);

            var recurrentResponse = await sut.ChangeRecurrencyDay(response.Payment.RecurrentPayment.RecurrentPaymentId, 10);

            Assert.Equal(HttpStatusCode.OK, recurrentResponse);
        }

        [Theory, AutoNSubstituteData]
        public async Task ChangeRecurrencyAmount_ReturnsOk(PagadorClient sut, SaleRequest request)
        {
            request.Payment.RecurrentPayment = new RecurrentPaymentDataRequest
            {
                AuthorizeNow = true,
                EndDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
                Interval = "Monthly"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.RecurrentPayment);
            Assert.NotNull(response.Payment.RecurrentPayment.RecurrentPaymentId);
            Assert.NotNull(response.Payment.RecurrentPayment.NextRecurrency);
            Assert.NotNull(response.Payment.RecurrentPayment.Interval);
            Assert.NotNull(response.Payment.RecurrentPayment.EndDate);

            var recurrentResponse = await sut.ChangeRecurrencyAmount(response.Payment.RecurrentPayment.RecurrentPaymentId, 15000);

            Assert.Equal(HttpStatusCode.OK, recurrentResponse);
        }

        [Theory, AutoNSubstituteData]
        public async Task ChangeRecurrencyNextPaymentDate_ReturnsOk(PagadorClient sut, SaleRequest request)
        {
            request.Payment.RecurrentPayment = new RecurrentPaymentDataRequest
            {
                AuthorizeNow = true,
                EndDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
                Interval = "Monthly"
            };

            var response = await sut.CreateSaleAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.HttpStatus);
            Assert.Equal(TransactionStatus.Authorized, response.Payment.Status);
            Assert.NotNull(response.Payment.RecurrentPayment);
            Assert.NotNull(response.Payment.RecurrentPayment.RecurrentPaymentId);
            Assert.NotNull(response.Payment.RecurrentPayment.NextRecurrency);
            Assert.NotNull(response.Payment.RecurrentPayment.Interval);
            Assert.NotNull(response.Payment.RecurrentPayment.EndDate);

            var nextPaymentDate = DateTime.UtcNow.AddMonths(1).ToString("yyyy-MM-dd");

            var recurrentResponse = await sut.ChangeRecurrencyNextPaymentDate(response.Payment.RecurrentPayment.RecurrentPaymentId, nextPaymentDate);

            Assert.Equal(HttpStatusCode.OK, recurrentResponse);
        }

        #endregion
    }
}
