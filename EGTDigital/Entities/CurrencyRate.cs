namespace EGTDigital.Entities
{
    public class CurrencyRate : Entity
    {
        public string CurrencyName { get; set; }

        public decimal Rate { get; set; }

        public long CurrencyDataTimestamp { get; set; }
    }
}
