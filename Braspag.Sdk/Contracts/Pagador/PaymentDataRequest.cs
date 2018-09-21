using System.Collections.Generic;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class PaymentDataRequest
    {
        public string Provider { get; set; }

        public string Type { get; set; }

        public long Amount { get; set; }

        public long ServiceTaxAmount { get; set; }

        public bool Capture { get; set; }

        public int Installments { get; set; }

        public string Interest { get; set; }

        public string Currency { get; set; }

        public string Country { get; set; }

        public bool? Authenticate { get; set; }

        public bool? Recurrent { get; set; }

        public string SoftDescriptor { get; set; }

        public string ReturnUrl { get; set; }

        public string BoletoNumber { get; set; }

        public string Assignor { get; set; }

        public string Demonstrative { get; set; }

        /// <summary>
        /// Data para vencimento do boleto. Formato: AAAA-MM-DD
        /// </summary>
        public string ExpirationDate { get; set; }

        /// <summary>
        /// CNPJ do Cedente
        /// </summary>
        public string Identification { get; set; }

        public string Instructions { get; set; }

        public byte? DaysToFine { get; set; }

        public decimal? FineRate { get; set; }

        public long? FineAmount { get; set; }

        public byte? DaysToInterest { get; set; }

        public decimal? InterestRate { get; set; }

        public long? InterestAmount { get; set; }

        public CreditCardData CreditCard { get; set; }

        public DebitCardData DebitCard { get; set; }

        public WalletData Wallet { get; set; }

        public CredentialsData Credentials { get; set; }

        public ExternalAuthenticationData ExternalAuthentication { get; set; }

        public FraudAnalysisRequestData FraudAnalysis { get; set; }

        public RecurrentPaymentDataRequest RecurrentPayment { get; set; }

        public List<ExtraData> ExtraDataCollection { get; set; }
    }
}