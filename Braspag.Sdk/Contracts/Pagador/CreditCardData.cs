namespace Braspag.Sdk.Contracts.Pagador
{
    public class CreditCardData
    {
        /// <summary>
        /// Token representativo do cartão no Cartão Protegido
        /// </summary>
        public string CardToken { get; set; }

        public string CardNumber { get; set; }

        public string Holder { get; set; }

        public string ExpirationDate { get; set; }

        /// <summary>
        /// Código de segurança impresso no verso do cartão (CVV)
        /// </summary>
        public string SecurityCode { get; set; }

        /// <summary>
        /// Token obtido através do Silent Order Post
        /// </summary>
        public string PaymentToken { get; set; }

        /// <summary>
        /// Bandeira do cartão
        /// </summary>
        public string Brand { get; set; }

        public bool? SaveCard { get; set; }

        public string Alias { get; set; }

        public AvsData Avs { get; set; }
    }
}