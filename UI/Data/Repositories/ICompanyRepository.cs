using System.Collections.ObjectModel;
using Model;

namespace UI.Data.Repositories
{
    public interface ICompanyRepository
    {
        ObservableCollection<Company> GetAll();
        Company GetById(int companyId);
        void ReloadCompany(int companyId);
        void Save();
        bool HasChanges();
    }
}