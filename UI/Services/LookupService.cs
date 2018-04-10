using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Model;

namespace UI.Services
{
    public class LookupService : ILookupService
    {
      
        private readonly BusinessDbContext _context;

        public LookupService(BusinessDbContext context)
        {
            _context = context;
        }
        public IEnumerable<LookupItem> GetAllEmployeeLookup()
        {
            return _context.Employees.AsNoTracking()
                .Select(e =>
                    new LookupItem
                    {
                        Id = e.Id,
                        DisplayMember = e.LastName + " " + e.FirstName
                    }).ToList();
        }
        public LookupItem GetEmployeeByIdLookup(int employeeId)
        {
            var employee = _context.Employees.AsNoTracking()
                .FirstOrDefault(e => e.Id == employeeId);

            if (employee != null)
                return new LookupItem
                {
                    Id = employee.Id,
                    DisplayMember = employee.LastName + " " + employee.FirstName
                };
            return null;
        }

        public IEnumerable<LookupItem> GetAllCompaniesLookup()
        {
            return _context.Companies.AsNoTracking()
                .Select(c =>
                    new LookupItem
                    {
                        Id = c.Id,
                        DisplayMember  = c.Name
                    }).ToList();
        }
        public LookupItem GetCompanyByIdLookup(int companyId)
        {
            var company = _context.Companies.AsNoTracking()
                .FirstOrDefault(c => c.Id == companyId);

            if (company != null)
                return new LookupItem
                {
                    Id = company.Id,
                    DisplayMember = company.Name
                };
            return null;
        }
    }
}
