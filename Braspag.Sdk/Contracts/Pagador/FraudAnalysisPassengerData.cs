using System.Collections.Generic;

namespace Braspag.Sdk.Contracts.Pagador
{
    public class FraudAnalysisPassengerData
    {
        public string Email { get; set; }

        public string Identity { get; set; }

        public string Name { get; set; }

        public string Rating { get; set; }

        public string Phone { get; set; }

        public string Status { get; set; }

        public string TicketNumber { get; set; }

        public string FrequentFlyerNumber { get; set; }

        public List<FraudAnalysisTravelLegsData> TravelLegs { get; set; }
    }
}