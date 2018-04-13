using System.Collections.ObjectModel;
using Model;
using System.Collections.Generic;

namespace UI.Data.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int employeeId);
        void Add(Employee employee);
        void Remove(Employee employee);
        void Save();
        bool HasChanges();
        void ReloadEmployee(int employeeId);
    }
}