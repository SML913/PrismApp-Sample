using Model;

namespace UI.Data.Repositories
{
    public interface ICompanyRepository:IGenericRepository<Company>
    {
        void ReloadCompany(int companyId);
        void AttachEmployee(Employee employee);
    }
}