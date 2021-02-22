using System;
using System.Collections.Generic;

namespace EGTDigital.Entities
{
    public class CurrencyData : Entity
    {
        public bool Status { get; set; } 

        public long Timestamp { get; set; }

        public string BaseCurrency { get; set; }

        public DateTime Date { get; set; }

        public List<CurrencyRate> Rates { get; set; }
    }
}
