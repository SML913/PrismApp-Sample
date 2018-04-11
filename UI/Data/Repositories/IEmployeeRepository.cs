using System.Collections.ObjectModel;
using Model;

namespace UI.Data.Repositories
{
    public interface IEmployeeRepository
    {
        ObservableCollection<Employee> GetAll();
        Employee GetById(int employeeId);
        void Add(Employee employee);
        void Remove(Employee employee);
        void Save();
        bool HasChanges();
        void ReloadEmployee(int employeeId);
    }
}