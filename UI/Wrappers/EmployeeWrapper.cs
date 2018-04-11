using Model;
using UI.ViewModels;

namespace UI.Wrappers
{
    public  class EmployeeWrapper:ViewModelBase
    {
        public EmployeeWrapper(Employee model)
        {
            Model = model;
        }
        public Employee Model { get; }
        public int Id { get { return Model.Id; }  }

        public string FirstName
        {
            get { return Model?.FirstName; }
            set
            {
                Model.FirstName = value;
                OnPropertyChanged();
            }
        }
        public string LastName {
            get
            {
                return Model?.LastName;
            }
            set
            {
                Model.LastName = value;
                OnPropertyChanged();
            } 
        }
        public int? CompanyId {
            get { return Model?.CompanyId; }
            set
            {
                Model.CompanyId = value;
                OnPropertyChanged();
            } }
    }
}
