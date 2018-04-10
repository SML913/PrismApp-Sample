using Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataAccess
{
    public class BusinessDbContext : DbContext
    {
        public BusinessDbContext() : base("BusinessDb")
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
        }
    }
}
