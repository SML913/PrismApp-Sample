using System.Collections.ObjectModel;
using Model;

namespace UI.Data.Repositories
{
    public interface ICompanyRepository
    {
        ObservableCollection<Company> GetAll();
        Company GetById(int companyId);
        void ReloadCompany(int companyId);
        void Add(Company company);
        void Remove(Company company);
        void Save();
        bool HasChanges();
    }
}