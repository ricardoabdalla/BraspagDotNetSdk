using System.Collections.Generic;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class FraudAnalysisCartData
    {
        public bool? IsGift { get; set; }

        public bool? ReturnsAccepted { get; set; }

        public List<FraudAnalysisItemData> Items { get; set; }
    }
}