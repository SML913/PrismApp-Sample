using System;
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
    public class EmployeeListViewModel : BindableBase, IEmployeeListViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private LookupItem _selectedEmployee;
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<LookupItem> _employees;
        private readonly ILookupService _lookupService;

        public EmployeeListViewModel( IEventAggregator eventAggregator,
            IEmployeeRepository employeeRepository ,
            ILookupService lookupService
            )
        {
            _employeeRepository = employeeRepository;
            _eventAggregator = eventAggregator;
            _lookupService = lookupService;
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

            var employees= _lookupService.GetAllEmployeeLookup();
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
            var employees = _lookupService.GetAllEmployeeLookup();
            foreach (var employee in employees)
            {
                    Employees.Add(employee);
            }
        }
        private void RaiseEdiDialog()
        {
            _eventAggregator.GetEvent<EditEmployeeEvent>().Publish(SelectedEmployee?.Id ?? 0);

            EmployeeNotificationRequest.Raise(new EditNotification() { Title = "Edit Employee" }, r =>
            {
                if (r.Confirmed) { }
                else { }
            });
        }
        private void RaiseAddDialog()
        {
            _eventAggregator.GetEvent<EditEmployeeEvent>().Publish(0);

            EmployeeNotificationRequest.Raise(new EditNotification() { Title = "Add Employee" }, r =>
            {
                if (r.Confirmed) { }
                else { }
            });
        }

        private bool DeleteCanExecute()
        {
            return SelectedEmployee != null;
        }

        private void DeleteExecute()
        {
            throw new NotImplementedException();
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