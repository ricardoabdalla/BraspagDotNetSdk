using System.Collections.Generic;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class PaymentDataResponse
    {
        /// <summary>
        /// Id da transação no provedor do meio de pagamento
        /// </summary>
        public string AcquirerTransactionId { get; set; }

        /// <summary>
        /// Valor da transação em centavos
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// URL para qual o Lojista deve redirecionar o Cliente para o fluxo de autenticação
        /// </summary>
        public string AuthenticationUrl { get; set; }

        public string Assignor { get; set; }

        /// <summary>
        /// Código de autorização da transação
        /// </summary>
        public string AuthorizationCode { get; set; }

        public string BoletoNumber { get; set; }

        public long? CapturedAmount { get; set; }

        public string CapturedDate { get; set; }

        public string Country { get; set; }

        public CredentialsDataRequest Credentials { get; set; }

        public CreditCardData CreditCard { get; set; }

        public string Currency { get; set; }

        public byte? DaysToFine { get; set; }

        public byte? DaysToInterest { get; set; }

        public DebitCardData DebitCard { get; set; }

        public string Demonstrative { get; set; }

        public string Eci { get; set; }

        public string ExpirationDate { get; set; }

        public ExternalAuthenticationData ExternalAuthentication { get; set; }

        public List<ExtraData> ExtraDataCollection { get; set; }

        public long? FineAmount { get; set; }

        public decimal? FineRate { get; set; }

        public FraudAnalysisRequestData FraudAnalysis { get; set; }

        public string Identification { get; set; }

        /// <summary>
        /// Número de parcelas da transação
        /// </summary>
        public int Installments { get; set; }

        public string Instructions { get; set; }

        public string Interest { get; set; }

        public long? InterestAmount { get; set; }

        public decimal? InterestRate { get; set; }

        public List<LinkData> Links { get; set; }

        /// <summary>
        /// ID da transação na Braspag
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Número do Comprovante de Venda
        /// </summary>
        public string ProofOfSale { get; set; }

        /// <summary>
        /// Nome do provedor do meio de pagamento
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Código retornado pelo provedor do meio de pagamento (adquirente e bancos)
        /// </summary>
        public string ProviderReturnCode { get; set; }

        /// <summary>
        /// Mensagem retornada pelo provedor do meio de pagamento (adquirente e bancos)
        /// </summary>
        public string ProviderReturnMessage { get; set; }

        /// <summary>
        /// Código de retorno da Operação
        /// </summary>
        public string ReasonCode { get; set; }

        /// <summary>
        /// Mensagem de retorno da Operação
        /// </summary>
        public string ReasonMessage { get; set; }

        /// <summary>
        /// Data em que a transação foi recebida pela Braspag
        /// </summary>
        public string ReceivedDate { get; set; }

        public bool? Recurrent { get; set; }

        public RecurrentPaymentDataRequest RecurrentPayment { get; set; }

        public string ReturnUrl { get; set; }

        public long ServiceTaxAmount { get; set; }

        public string SoftDescriptor { get; set; }

        /// <summary>
        /// Status da Transação
        /// </summary>
        public TransactionStatus Status { get; set; }

        /// <summary>
        /// Tipo do Meio de Pagamento (CreditCard, DebitCard)
        /// </summary>
        public string Type { get; set; }

        public WalletDataRequest Wallet { get; set; }
    }
}