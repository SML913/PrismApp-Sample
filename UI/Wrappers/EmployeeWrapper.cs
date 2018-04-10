using Microsoft.Practices.Prism.Mvvm;
using Model;

namespace UI.Wrappers
{
  public  class EmployeeWrapper:BindableBase
    {
        private string _firstName;
        private string _lastName;
        private int? _companyId;

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
                
                SetProperty(ref _firstName, value);
            }
        }
        public string LastName {
            get
            {
                return Model?.LastName;
            }
            set { SetProperty(ref _lastName, value); } 
        }
        public int? CompanyId { get { return Model?.CompanyId; }
            set { SetProperty(ref _companyId, value); } }
    }
}
