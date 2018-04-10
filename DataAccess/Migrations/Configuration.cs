using Model;

namespace DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.BusinessDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataAccess.BusinessDbContext context)
        {
            context.Companies.AddOrUpdate(c => c.Name,
                new Company() { Name = "Pluralsight" },
                new Company { Name = "Microsoft" },
                new Company() { Name = "Microsoft" }
            );

            context.Employees.AddOrUpdate(e => e.FirstName,
                new Employee { FirstName = "Smail", LastName = "Agr" },
                new Employee { FirstName = "Sami", LastName = "Bou" },
                new Employee { FirstName = "Chamso", LastName = "Sali" }
            );
        }
    }
}
