namespace Braspag.Sdk.Contracts.Pagador
{
    public enum RecurrencyInterval : byte
    {
        Monthly = 1,
        Bimonthly = 2,
        Quarterly = 4,
        SemiAnnual = 6,
        Annual = 12
    }
}