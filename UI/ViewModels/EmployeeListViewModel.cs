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

namespace UI.ViewModels
{
    public class EmployeeListViewModel : BindableBase, IEmployeeListViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private Employee _selectedEmployee;
        private string _title;
        private readonly IEventAggregator _eventAggregator;

        public EmployeeListViewModel( IEventAggregator eventAggregator,
            IEmployeeRepository employeeRepository )
        {
            _employeeRepository = employeeRepository;
            _eventAggregator = eventAggregator;

            Employees = new ObservableCollection<Employee>();
           
            EmployeeNotificationRequest = new InteractionRequest<IEditNotification>();
            EditEmployeeNotificationCommand = new DelegateCommand(RaiseEdiDialog,EditCanExecute);
            AddEmployeeNotificationCommand = new DelegateCommand(RaiseAddDialog);
            Load();
        }

      


        #region Methodes
        private bool EditCanExecute()
        {
            return SelectedEmployee != null;
        }
       public void Load()
        {
            Employees = _employeeRepository.GetAll();
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

        #endregion



        #region Properties
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ObservableCollection<Employee> Employees { get; set; }
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                SetProperty(ref _selectedEmployee, value); 
                (EditEmployeeNotificationCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

       

        #region Commands
        public InteractionRequest<IEditNotification> EmployeeNotificationRequest { get; set; }
        public DelegateCommand EditEmployeeNotificationCommand { get; set; }
        public DelegateCommand AddEmployeeNotificationCommand { get; set; }

        #endregion



     
       
        
    }
}