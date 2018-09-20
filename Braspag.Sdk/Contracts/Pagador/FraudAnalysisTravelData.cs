using System.Collections.Generic;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class FraudAnalysisTravelData
    {
        public string JourneyType { get; set; }

        public string DepartureDateTime { get; set; }

        public List<FraudAnalysisPassengerData> Passengers { get; set; }
    }
}