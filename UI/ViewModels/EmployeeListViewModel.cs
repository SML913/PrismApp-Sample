using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Mvvm;
using Model;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using UI.Data.Repositories;
using UI.Event;
using UI.Notifications;
using UI.Services;

namespace UI.ViewModels
{
    public class EmployeeListViewModel : BindableBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private LookupItem _selectedEmployee;
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<LookupItem> _employees;
        private readonly ICommonService _commonService;
        private readonly IDialogService _dialogService;

        public EmployeeListViewModel( IEventAggregator eventAggregator,
            IEmployeeRepository employeeRepository ,
            IDialogService dialogService,
            ICommonService commonService
            )
        {
            _employeeRepository = employeeRepository;
            _eventAggregator = eventAggregator;
            _commonService = commonService;
            _dialogService = dialogService;
            _eventAggregator.GetEvent<EmployeeSavedEvent>().Subscribe(UpdateEmployees);
            Employees = new ObservableCollection<LookupItem>();
           
            EmployeeNotificationRequest = new InteractionRequest<IEditNotification>();
            EditEmployeeNotificationCommand = new DelegateCommand(RaiseEdiDialog,EditCanExecute);
            AddEmployeeNotificationCommand = new DelegateCommand(RaiseAddDialog);
            DeleteCommand = new DelegateCommand(DeleteExecute, DeleteCanExecute);

            Load();
        }






        #region Methodes
        private void UpdateEmployees(int employeeId)
        {
           Employees.Clear();
            _employeeRepository.ReloadEmployee(employeeId);

            var employees= _commonService.GetAllEmployeeLookup();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }

        }
        private bool EditCanExecute()
        {
            return SelectedEmployee != null;
        }
       public void Load()
        {
            var employees = _commonService.GetAllEmployeeLookup();
            Employees.Clear();
            foreach (var employee in employees)
            {
                    Employees.Add(employee);
            }
        }
        private void RaiseEdiDialog()
        {
            _eventAggregator.GetEvent<EditEmployeeEvent>().Publish(SelectedEmployee?.Id ?? 0);

            EmployeeNotificationRequest.Raise(new EditNotification() { Title = "Edit Employee" });
        }
        private void RaiseAddDialog()
        {
            _eventAggregator.GetEvent<EditEmployeeEvent>().Publish(0);

            EmployeeNotificationRequest.Raise(new EditNotification() { Title = "Add Employee" });
        }

        private bool DeleteCanExecute()
        {
            return SelectedEmployee != null;
        }

        private async void DeleteExecute()
        {
            var answer = await _dialogService.ShowOkCancelDialog("Warning",
                $"Do you really want to delete the employee {SelectedEmployee.DisplayMember}  ?");
            if (!answer) return;
            var employee = _employeeRepository.GetById(SelectedEmployee.Id);
            _employeeRepository.Remove(employee);
            _employeeRepository.Save();
            SelectedEmployee = null;
            Load();
        }

        #endregion



        #region Properties
      
        public ObservableCollection<LookupItem> Employees {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
            
        }
        public LookupItem SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                SetProperty(ref _selectedEmployee, value); 

                EditEmployeeNotificationCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }


        #endregion

       

        #region Commands
        public InteractionRequest<IEditNotification> EmployeeNotificationRequest { get; set; }
        public DelegateCommand EditEmployeeNotificationCommand { get; set; }
        public DelegateCommand AddEmployeeNotificationCommand { get; set; }
        public DelegateCommand DeleteCommand { get; }

        #endregion






    }
}