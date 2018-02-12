using Example.BusinessApp.Sales.Shared;
using Microsoft.EntityFrameworkCore;

namespace Example.BusinessApp.Sales.Business
{
    public class SalesDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(@"Server=(localdb)\mssqllocaldb;Database=Test;Trusted_Connection=True;ConnectRetryCount=0");
        }
    }
}
