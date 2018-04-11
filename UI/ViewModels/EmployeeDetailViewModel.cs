using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Events;
using Model;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using UI.Data.Repositories;
using UI.Event;
using UI.Notifications;
using UI.Services;
using UI.Wrappers;

namespace UI.ViewModels
{
    public class EmployeeDetailViewModel : BindableBase, IEmployeeDetailViewModel, IInteractionRequestAware
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private EmployeeWrapper _employee;
        private IEditNotification _notification;
        private bool _isDirty;
        private readonly ILookupService _lookupService;
        private readonly IEventAggregator _eventAggregator;
     
        public EmployeeDetailViewModel(
            IEventAggregator eventAggregator,
            IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository,
            ILookupService lookupService)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EditEmployeeEvent>().Subscribe(LoadEmployee);
            _eventAggregator.GetEvent<CompanySavedEvent>().Subscribe(ReloadCompanies);
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _lookupService = lookupService;
            Companies = new ObservableCollection<LookupItem>();

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            CloseCommand = new DelegateCommand(OnCloseExecute);
           
            LoadCompanies();
        }

      



        #region Methodes

        public void LoadCompanies()
        {
            Companies.Clear();
            Companies.Add(new LookupItem.NullLocupItem {DisplayMember = "-"});
            var lookup = _lookupService.GetAllCompaniesLookup();
            foreach (var lookupItem in lookup)
            {
                Companies.Add(lookupItem);
            }

        }
        private void ReloadCompanies(int companyId)
        {
           LoadCompanies();
        }

        private void LoadEmployee(int employeeId)
        {
            if (Employee != null && Employee.Id == employeeId && employeeId > 0)
            {
                _employeeRepository.ReloadEmployee(employeeId);

                IsDirty = _employeeRepository.HasChanges();
                SaveCommand.RaiseCanExecuteChanged();
            }

            var employee = employeeId != 0 ? _employeeRepository.GetById(employeeId) : new Employee();
            Employee = new EmployeeWrapper(employee);
             if(employeeId==0)   _employeeRepository.Add(employee);


        Employee.PropertyChanged += (s, e) =>
        {
        if (!IsDirty)
        {
            IsDirty = _companyRepository.HasChanges();
             SaveCommand.RaiseCanExecuteChanged();
        }
        if (e.PropertyName == nameof(Employee.FirstName)||
            e.PropertyName == nameof(Employee.LastName) ||
            e.PropertyName == nameof(Employee.CompanyId))
        {
            IsDirty = _employeeRepository.HasChanges();
            SaveCommand.RaiseCanExecuteChanged();
        }
    };
}
        
        private void OnCloseExecute()
        {
            _notification.Confirmed = true;
            FinishInteraction?.Invoke();
        }

        private bool OnSaveCanExecute()
        {
            return _employeeRepository.HasChanges();
        }

        private void OnSaveExecute()
        {
            _employeeRepository.Save();
            IsDirty = _employeeRepository.HasChanges();
            SaveCommand.RaiseCanExecuteChanged();
            _eventAggregator.GetEvent<EmployeeSavedEvent>().Publish(Employee.Id);

        }

        #endregion

        #region Properties

       
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                SetProperty(ref _isDirty, value);

            }
        }
      
        public EmployeeWrapper Employee
        {
            get { return _employee; }
            set
            {
                SetProperty(ref _employee, value);
            }
        }
    
      
        public ObservableCollection<LookupItem> Companies { get; set; }
        public INotification Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, (IEditNotification)value); }
        }
        public Action FinishInteraction { get; set; }
        #endregion

        #region Commands

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CloseCommand { get; }
      

        #endregion


    }
}
