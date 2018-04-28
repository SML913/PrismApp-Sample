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


        #region private field

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
        private readonly ICompanyRepository _companyRepository;
        private ObservableCollection<LookupItem> _emloyeesOnAddedState;

        #endregion


        public CompanyDetailViewModel(
            ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator,
           IDialogService dialogServicel)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _dialogService = dialogServicel;

            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EditCompanyEvent>().Subscribe(LoadCompany);
            _eventAggregator.GetEvent<EmployeeSavedEvent>().Subscribe(OnEmployeeSaved);

            

            Employees=new ObservableCollection<LookupItem>();
            EmployeesOnAddedState=new ObservableCollection<LookupItem>();
            AvailableEmployees = new ObservableCollection<LookupItem>();

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
            CloseCommand = new DelegateCommand(OnCloseExecute);
            AddEmployeeCommand = new DelegateCommand(OnAddEmployeeExecute, OnAddEmployeeCanExecute);
        }

       



        #region Methodes


      
        private void OnEmployeeSaved(int employeeId)
        {
            _employeeRepository.ReloadEmployee(employeeId);
        }

        private void LoadCompany(int companyId)
        {
           
            var company = companyId != 0 ? _companyRepository.GetById(companyId) : new Company();
            Company = new CompanyWrapper(company);

            if (companyId == 0) _companyRepository.Add(company);


            Employees.Clear();
            AvailableEmployees.Clear();
            var employees = _employeeRepository.GetAll();

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
                  
                    IsDirty = _employeeRepository.HasChanges();
                    SaveCommand.RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Company.Name))
                {
                  
                    IsDirty = _companyRepository.HasChanges();
                    SaveCommand.RaiseCanExecuteChanged();
                }
            };
        }
        private void OnCloseExecute( )
        {
            if (Company != null && Company.Id > 0)
            {
                _companyRepository.ReloadCompany(Company.Id);
            }
            EmployeesOnAddedState.Clear();

            _notification.Confirmed = true;
            FinishInteraction?.Invoke();
        }
        private bool OnSaveCanExecute()
        {
            return IsDirty||_companyRepository.HasChanges();
        }
        private void OnSaveExecute()
        {
            if (EmployeesOnAddedState.Count != 0)
            {
                foreach (var employee in EmployeesOnAddedState)
                {
                    var employeeToAdd = _employeeRepository.GetById(employee.Id);
                      _companyRepository.AttachEmployee(employeeToAdd);
                        Company.Employees.Add(employeeToAdd);
                    
                }
                
            }

            _companyRepository.Save();
            IsDirty = _companyRepository.HasChanges();
            SaveCommand.RaiseCanExecuteChanged();
            _eventAggregator.GetEvent<CompanySavedEvent>().Publish(Company.Id);
        }
        private bool OnDeleteCanExecute( )
        {
            return SelectedEmployee != null;
        }
        private  void OnDeleteExecute()
        {
            //var answer = await _dialogService.ShowOkCancelDialog("Warning",
            //    $"Do you really want to remove the employee {SelectedEmployee.DisplayMember}  ?");

            var answer = _dialogService.ShowOkCancelDialogUsingMsgBox("Warning",
               $"Do you really want to remove the employee {SelectedEmployee.DisplayMember}  ?"); 
            if (answer)
            {
                if (EmployeesOnAddedState.Contains(SelectedEmployee))
                {
                    var employee = SelectedEmployee;
                    if (Employees.Contains(SelectedEmployee))
                    {
                        AvailableEmployees.Add(employee);
                        Employees.Remove(employee);
                     
                    }
                    EmployeesOnAddedState.Remove(SelectedEmployee);

                    if (EmployeesOnAddedState.Count == 0)
                    {
                        IsDirty = _employeeRepository.HasChanges();
                        SaveCommand.RaiseCanExecuteChanged();
                    }
                    return;
                }
                
                    var empoyee = _employeeRepository.GetById(SelectedEmployee.Id);
                    empoyee.CompanyId = null;
                    _employeeRepository.Save();
                  _eventAggregator.GetEvent<EmployeeStateChanged>().Publish(empoyee.Id);
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
               // _dialogService.ShowInfoDialog("Please select an Employee to add.");

               _dialogService.ShowInfoDialogUsingMsgBox("Please select an Employee to add.");
                   return;
            }
            
           

            EmployeesOnAddedState.Add(SelectedEmployeeToAdd);
            Employees.Add(SelectedEmployeeToAdd);
            if (AvailableEmployees.Contains(SelectedEmployeeToAdd))
            {
                AvailableEmployees.Remove(SelectedEmployeeToAdd);
            }

            IsDirty = true;//_companyRepository.HasChanges();
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

        public ObservableCollection<LookupItem> EmployeesOnAddedState
        {
            get { return _emloyeesOnAddedState; }
            set { SetProperty(ref _emloyeesOnAddedState, value); }
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
