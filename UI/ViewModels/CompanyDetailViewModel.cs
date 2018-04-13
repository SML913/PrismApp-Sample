using System;
using System.Collections.ObjectModel;
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
    public class CompanyDetailViewModel:BindableBase,IInteractionRequestAware
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
        private LookupItem _selectedEmployeeToAdd;
        private readonly IDialogService _dialogService;

        public CompanyDetailViewModel(
            IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator,
            ICompanyRepository companyRepository,
           IDialogService dialogServicel)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EditCompanyEvent>().Subscribe(LoadCompany);
            _eventAggregator.GetEvent<EmployeeSavedEvent>().Subscribe(OnEmployeeSaved);
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
           _dialogService = dialogServicel;

            Employees=new ObservableCollection<LookupItem>();
            AvailableEmployees = new ObservableCollection<LookupItem>();

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
            CloseCommand = new DelegateCommand(OnCloseExecute);
            AddEmployeeCommand = new DelegateCommand(OnAddEmployeeExecute, OnAddEmployeeCanExecute);
        }

       



        #region Methodes


      
        private void OnEmployeeSaved(int employeeId)
        {
            //throw new NotImplementedException();

            //TODO update the available employees
        
        }

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

        private bool OnDeleteCanExecute( )
        {
            return SelectedEmployee != null;
        }

        private async void OnDeleteExecute()
        {
            var answer = await _dialogService.ShowOkCancelDialog("Warning",
                $"Do you really want to remove the employee {SelectedEmployee.DisplayMember}  ?");
            if (answer)
            {
                var empoyee = _employeeRepository.GetById(SelectedEmployee.Id);
                empoyee.CompanyId = null;
                _employeeRepository.Save();
                AvailableEmployees.Add(SelectedEmployee);
                if (Employees.Contains(SelectedEmployee))
                {
                    Employees.Remove(SelectedEmployee);
                }
                }
        }

        private void OnAddEmployeeExecute()
        {
            if (SelectedEmployeeToAdd == null)
            { 
                _dialogService.ShowInfoDialog("Please select an Employee to add.");
            return;
                }

        Employees.Add(SelectedEmployeeToAdd);
            var employeeToAdd = _employeeRepository.GetById(SelectedEmployeeToAdd.Id);
            Company.Employees.Add(employeeToAdd);

            if (AvailableEmployees.Contains(SelectedEmployeeToAdd))
            {
                AvailableEmployees.Remove(SelectedEmployeeToAdd);
            }
           
          

            IsDirty = _companyRepository.HasChanges();
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool OnAddEmployeeCanExecute()
        {
            return SelectedEmployeeToAdd != null;


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
            get { return _availableEmployees; }

            set { SetProperty(ref _availableEmployees, value); }
        }
        public LookupItem SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                SetProperty(ref _selectedEmployee, value);
                DeleteCommand.RaiseCanExecuteChanged();
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

        public LookupItem SelectedEmployeeToAdd
        {
            get { return _selectedEmployeeToAdd; }
            set
            {
                SetProperty(ref _selectedEmployeeToAdd, value);
                AddEmployeeCommand.RaiseCanExecuteChanged();
                
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
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand AddEmployeeCommand { get; }


        #endregion
    }
}
