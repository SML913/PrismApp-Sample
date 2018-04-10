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
    public class EmployeeDetailViewModel:BindableBase, IEmployeeDetailViewModel,IInteractionRequestAware
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private CompanyWrapper _selectedCompany;
        private EmployeeWrapper _employee;
        private IEditNotification _notification;
        private bool _isDirty;
        private ILookupService _lookupService;

        public EmployeeDetailViewModel(
            IEventAggregator eventAggregator,
            IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository,
            ILookupService  lookupService)
        {
            eventAggregator.GetEvent<EditEmployeeEvent>().Subscribe(LoadEmployee);
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
            Companies.Add(new LookupItem.NullLocupItem{DisplayMember = "-"});
            var lookup = _lookupService.GetAllCompaniesLookup();
            foreach (var lookupItem in lookup)
            {
                Companies.Add(lookupItem);
            }

        }
        private void LoadEmployee(int employeeId)
        {
            var employee = _employeeRepository.GetById(employeeId);
            Employee = new EmployeeWrapper(employee);

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

        }

        #endregion

        #region Properties
        public bool IsDirdty
        {
            get { return _isDirty; }
            set
            {
                SetProperty(ref _isDirty, value);

                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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
        public CompanyWrapper SelectedCompany
        {
            get { return _selectedCompany; }

            set
            {
                SetProperty(ref _selectedCompany, value);
                IsDirdty = _employeeRepository.HasChanges();
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
