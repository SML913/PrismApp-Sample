using System.Collections.ObjectModel;
using Model;
using System.Collections.Generic;

namespace UI.Data.Repositories
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAll();
        Company GetById(int companyId);
        void ReloadCompany(int companyId);
        void Add(Company company);
        void Remove(Company company);
        void Save();
        bool HasChanges();
    }
}