using System;
using System.Collections.ObjectModel;
using System.Linq;
using DataAccess;
using Model;
using System.Collections.Generic;

namespace UI.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BusinessDbContext _context;

        public EmployeeRepository(BusinessDbContext context)
        {
            _context = context;
        }
        public Employee GetById(int employeeId)
        {
            return _context.Set<Employee>().Find(employeeId);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void ReloadEmployee(int employeeId)
        {
            var dbEntityEntry = _context.ChangeTracker.Entries<Employee>()
                .SingleOrDefault(db => db.Entity.Id == employeeId);
            dbEntityEntry?.Reload();
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        public void Remove(Employee employee)
        {
            _context.Employees.Remove(employee);
        }
    }
}
