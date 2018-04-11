using System.Collections.ObjectModel;
using System.Linq;
using DataAccess;
using Model;

namespace UI.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly BusinessDbContext _context;

        public CompanyRepository(BusinessDbContext context)
        {
            _context = context;
        }

        public void Add(Company company)
        {
            _context.Companies.Add(company);
        }
        public void Remove(Company company)
        {
            _context.Companies.Remove(company);
        }
        public Company GetById(int companyId)
        {
           
            return _context.Set<Company>().Find(companyId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool HasChanges()
        {
          return  _context.ChangeTracker.HasChanges();
        }

        public ObservableCollection<Company> GetAll()
        {
          return new ObservableCollection<Company>(_context.Set<Company>());
        }

        public void ReloadCompany(int companyId)
        {
            
            var dbEntityEntry = _context.ChangeTracker.Entries<Company>()
                .SingleOrDefault(db => db.Entity.Id == companyId);
            dbEntityEntry?.Reload();
        }
    }
}
