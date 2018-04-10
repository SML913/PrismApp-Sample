using System.Collections.ObjectModel;
using DataAccess;
using Model;

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

        public ObservableCollection<Employee> GetAll()
        {
            return new ObservableCollection<Employee>( _context.Set<Employee>());
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
