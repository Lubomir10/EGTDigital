using Microsoft.EntityFrameworkCore;

namespace EGTDigital.Entities
{
    public class EgtDbContext : DbContext
    {
        public EgtDbContext(DbContextOptions<EgtDbContext> options) : base(options)
        {

        }

        public DbSet<Request> Requests { get; set; }

        public DbSet<CurrencyData> CurrencyDatas { get; set; }

        public DbSet<CurrencyRate> CurrencyRates { get; set; }
    }
}
