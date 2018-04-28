using System.Linq;
using DataAccess;
using Model;
using UI.Services;

namespace UI.Data.Repositories
{
    public class EmployeeRepository :GenericRepository<Employee,BusinessDbContext>, IEmployeeRepository
    {
        

        public EmployeeRepository(BusinessDbContext context,IDialogService dialogService):base(context,dialogService)
        {
            
        }
      
        public void ReloadEmployee(int employeeId)
        {
            try
            {
                var dbEntityEntry = Context.ChangeTracker.Entries<Employee>()
                    .SingleOrDefault(db => db.Entity.Id == employeeId);
                dbEntityEntry?.Reload();
            }
            catch
            {
                DialogService.ShowInfoDialogUsingMsgBox("An error has occurred. can't reload employee");
            }


        }

        public void AttachCompany(Company company)
        {
            try
            {
                Context.Companies.Attach(company);
            }
            catch
            {
                DialogService.ShowInfoDialogUsingMsgBox("An error has occurred. can't attach company entity");
            }
           
        }
    }
}
