using Model;

namespace DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BusinessDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BusinessDbContext context)
        {
            context.Companies.AddOrUpdate(c => c.Name,
                new Company() { Name = "Pluralsight" },
                new Company { Name = "Microsoft" },
                new Company() { Name = "Infragistics" }
            );

            context.Employees.AddOrUpdate(e => e.FirstName,
                new Employee { FirstName = "Smail", LastName = "Agr" },
                new Employee { FirstName = "Sami", LastName = "Bean" },
                new Employee { FirstName = "Chamso", LastName = "Ali" }
            );
        }
    }
}
