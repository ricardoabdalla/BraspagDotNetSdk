namespace Braspag.Sdk.Contracts.Pagador
{
    public class AvsData
    {
        public string Cpf { get; set; }

        public string ZipCode { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string District { get; set; }

        public int Status { get; set; }

        public string ReturnCode { get; set; }
    }
}