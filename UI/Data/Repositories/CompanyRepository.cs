using System.Linq;
using DataAccess;
using Model;
using System.Collections.Generic;
using System.Data.Entity;
using UI.Services;

namespace UI.Data.Repositories
{
    public class CompanyRepository :  GenericRepository<Company,BusinessDbContext>, ICompanyRepository
    {
        
        public CompanyRepository(BusinessDbContext context,
            IDialogService dialogService):base(context,dialogService)
        {
            
        }

        public void AttachEmployee(Employee employee)
        {
            try
            {
                Context.Employees.Attach(employee);
            }
            catch
            {
                DialogService.ShowInfoDialogUsingMsgBox("An error has occurred. Can't atach employee entity");
            }

        }
     
        public void ReloadCompany(int companyId)
        {
            try
            {
                var dbEntityEntry = Context.ChangeTracker.Entries<Company>()
                    .SingleOrDefault(db => db.Entity.Id == companyId);
                dbEntityEntry?.Reload();
            }
            catch
            {
                DialogService.ShowInfoDialogUsingMsgBox("An error has occurred. Can't reload company entity");
            }

        }

        public override IEnumerable<Company> GetAll()
        {
            return Context.Companies.Include(c => c.Employees).ToList();
        }

        public override Company GetById(int companyId)
        {
            return Context.Companies.Include(c=> c.Employees).Single(c=> c.Id==companyId);
        }
    }
}
