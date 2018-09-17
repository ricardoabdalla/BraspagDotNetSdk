namespace Braspag.Sdk.Contracts.Velocity
{
    public class EmailageResultData
    {
        public string EmailExist { get; set; }

        public string FirstVerificationDate { get; set; }

        public int Score { get; set; }

        public string ScoreDescriptionId { get; set; }

        public string ReasonId { get; set; }

        public string ReasonDescription { get; set; }

        public string RiskBandId { get; set; }

        public string RiskBandDescription { get; set; }
    }
}