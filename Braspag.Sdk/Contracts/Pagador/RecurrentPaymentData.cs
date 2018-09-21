using System.Collections.Generic;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class RecurrentPaymentData
    {
        public int Installments { get; set; }

        public string RecurrentPaymentId { get; set; }

        public string NextRecurrency { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Interval { get; set; }

        public long Amount { get; set; }

        public string Country { get; set; }

        public string CreateDate { get; set; }

        public string Currency { get; set; }

        public int CurrentRecurrencyTry { get; set; }

        public string OrderNumber { get; set; }

        public string Provider { get; set; }

        public int RecurrencyDay { get; set; }

        public int SuccessfulRecurrences { get; set; }

        public int Status { get; set; }

        public List<RecurrentTransactionData> RecurrentTransactions { get; set; }
    }
}