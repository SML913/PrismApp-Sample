using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Model;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using UI.Data.Repositories;
using UI.Event;
using UI.Notifications;
using UI.Services;
using UI.Wrappers;
using BindableBase = Prism.Mvvm.BindableBase;

namespace UI.ViewModels
{
    public class CompanyDetailViewModel:BindableBase, ICompanyDetailViewModel,IInteractionRequestAware
    {
        private readonly ICompanyRepository _companyRepository;
        private IEditNotification _notification;
        private CompanyWrapper _company;
        private bool _isDirty;
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<LookupItem> _employees;
        private readonly IEmployeeRepository _employeeRepository;
        private LookupItem _selectedEmployee;
        private ObservableCollection<LookupItem> _availableEmployees;
        private ILookupService _lookupService;
        private int _employeeId;

        public CompanyDetailViewModel(
            IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator,
            ICompanyRepository companyRepository,
            ILookupService lookupService)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EditCompanyEvent>().Subscribe(LoadCompany);
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _lookupService = lookupService;

            Employees=new ObservableCollection<LookupItem>();
            AvailableEmployees = new ObservableCollection<LookupItem>();
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
            CloseCommand = new DelegateCommand(OnCloseExecute);
            AddEmployeeCommand = new DelegateCommand(OnAddEmployeeExecute);
        }



        #region Methodes
      
        private void LoadCompany(int companyId)
        {
            if (Company != null && Company.Id == companyId && companyId>0)
            {
                _companyRepository.ReloadCompany(companyId);

                IsDirty = _companyRepository.HasChanges();
                SaveCommand.RaiseCanExecuteChanged();
            }

            var company = companyId != 0 ? _companyRepository.GetById(companyId) : new Company();
            Company =  new CompanyWrapper(company);
             if(companyId==0) _companyRepository.Add(company);
          



            var employees = _employeeRepository.GetAll();
            Employees.Clear();
            AvailableEmployees.Clear();
            foreach (var employee in employees)
            {
                if (employee.CompanyId == null)
                {
                    AvailableEmployees.Add(new LookupItem
                    {
                        Id = employee.Id,
                        DisplayMember = employee.LastName + " " + employee.FirstName
                    });

                    continue;
                }

                if(employee.CompanyId != companyId) continue;

                Employees.Add(new LookupItem
                {
                    Id = employee.Id,
                    DisplayMember = employee.LastName+" "+employee.FirstName
                });
               
            }

            Company.PropertyChanged += (s, e) =>
            {
                if (!IsDirty)
                {
                    IsDirty = _companyRepository.HasChanges();
                    SaveCommand.RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Company.Name))
                {
                    IsDirty = _companyRepository.HasChanges();
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
            return _companyRepository.HasChanges();
        }
        private void OnSaveExecute()
        {
           _companyRepository.Save();
            IsDirty = _companyRepository.HasChanges();
            SaveCommand.RaiseCanExecuteChanged();
            _eventAggregator.GetEvent<CompanySavedEvent>().Publish(Company.Id);
        }

        private bool OnDeleteCanExecute()
        {
            return SelectedEmployee != null;
        }

        private void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }
        private void OnAddEmployeeExecute()
        {
          var selectedEmployee =   _lookupService.GetEmployeeByIdLookup(SelecteEmployeeIdToAdd);
            AvailableEmployees.Remove(selectedEmployee);
            Employees.Add(selectedEmployee);

            var employeeToAdd = _employeeRepository.GetById(SelecteEmployeeIdToAdd);
           Company.Employees.Add(employeeToAdd);

            IsDirty = _companyRepository.HasChanges();
            SaveCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Poroperties

        public ObservableCollection<LookupItem> Employees
        {
            get { return _employees; }  
            set { SetProperty(ref _employees, value); }
        }

        public ObservableCollection<LookupItem> AvailableEmployees
        {
            get
            { return _availableEmployees; }
            set { SetProperty(ref _availableEmployees, value); }
        }
        public LookupItem SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                SetProperty(ref _selectedEmployee, value);
                ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }
        public CompanyWrapper Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                SetProperty(ref _isDirty, value);
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public int SelecteEmployeeIdToAdd
        {
            get { return _employeeId; }
            set
            {
                SetProperty(ref _employeeId, value);
                
            }
        }

       

        public INotification Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, (IEditNotification)value); }
        }
        public Action FinishInteraction { get; set; }

        #endregion

        #region commands
        public DelegateCommand SaveCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand DeleteCommand { get; }
        public DelegateCommand AddEmployeeCommand { get; }


        #endregion
    }
}
