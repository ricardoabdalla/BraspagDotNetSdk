namespace Braspag.Sdk.Contracts.Pagador
{
    public class FraudAnalysisItemData
    {
        public string GiftCategory { get; set; }

        public string HostHedge { get; set; }

        public string NonSensicalHedge { get; set; }

        public string ObscenitiesHedge { get; set; }

        public string PhoneHedge { get; set; }

        public string TimeHedge { get; set; }

        public string VelocityHedge { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Sku { get; set; }

        public long UnitPrice { get; set; }

        public string Risk { get; set; }

        public string Type { get; set; }

        public FraudAnalysisPassengerData Passenger { get; set; }
    }
}