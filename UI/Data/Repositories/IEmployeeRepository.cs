using System.Collections.ObjectModel;
using Model;

namespace UI.Data.Repositories
{
    public interface IEmployeeRepository
    {
        ObservableCollection<Employee> GetAll();
        Employee GetById(int employeeId);
        void Save();
        bool HasChanges();
    }
}