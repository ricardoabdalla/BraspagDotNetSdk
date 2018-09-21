namespace Braspag.Sdk.Contracts.Pagador
{
    public class RecurrentTransactionData
    {
        public int PaymentNumber { get; set; }

        public string RecurrentPaymentId { get; set; }

        public string TransactionId { get; set; }

        public int TryNumber { get; set; }
    }
}