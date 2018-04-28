using Model;

namespace UI.Data.Repositories
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        void ReloadEmployee(int employeeId);
        void AttachCompany(Company company);

    }
}