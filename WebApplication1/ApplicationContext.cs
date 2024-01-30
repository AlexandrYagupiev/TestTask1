using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Subscriptiont> Sublist { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-DF4L35O;Initial Catalog=Prinzip;Integrated Security=True;Encrypt=False");
        }
    }
}
