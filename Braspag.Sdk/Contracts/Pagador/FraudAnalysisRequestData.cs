using System.Collections.Generic;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class FraudAnalysisRequestData
    {
        public string Sequence { get; set; }

        public string SequenceCriteria { get; set; }

        public string FingerPrintId { get; set; }

        public string Provider { get; set; }

        public bool? CaptureOnLowRisk { get; set; }

        public bool? VoidOnHighRisk { get; set; }

        public long TotalOrderAmount { get; set; }

        public FraudAnalysisBrowserData Browser { get; set; }

        public FraudAnalysisCartData Cart { get; set; }

        public List<FraudAnalysisMerchantDefinedFieldsData> MerchantDefinedFields { get; set; }

        public FraudAnalysisShippingData Shipping { get; set; }

        public FraudAnalysisTravelData Travel { get; set; }
    }
}