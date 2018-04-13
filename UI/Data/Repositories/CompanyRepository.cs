using System.Collections.ObjectModel;
using System.Linq;
using DataAccess;
using Model;
using System.Collections.Generic;

namespace UI.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly BusinessDbContext _context;

        public CompanyRepository(BusinessDbContext context)
        {
            _context = context;
        }

       
      
        public Company GetById(int companyId)
        {
           
            return _context.Companies.Find(companyId);
        }
        public IEnumerable<Company> GetAll()
        {
            return _context.Companies.ToList();
        }
        public void Add(Company company)
        {
            _context.Companies.Add(company);
        }
        public void Remove(Company company)
        {
            _context.Companies.Remove(company);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public bool HasChanges()
        {
          return  _context.ChangeTracker.HasChanges();
        }
        public void ReloadCompany(int companyId)
        {
            
            var dbEntityEntry = _context.ChangeTracker.Entries<Company>()
                .SingleOrDefault(db => db.Entity.Id == companyId);
            dbEntityEntry?.Reload();
        }
    }
}
