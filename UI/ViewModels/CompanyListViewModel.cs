using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
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
    public class CompanyListViewModel:BindableBase, ICompanyListViewModel
    {
        private readonly ICompanyRepository _companyrepository;
        private LookupItem _selectedCompany;
        private string _title;
        
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<LookupItem> _companies;
        private ILookupService _lookupService;

        public CompanyListViewModel(IEventAggregator eventAggregator,
            ICompanyRepository  companyRepository,
            ILookupService lookupService)
        {
            _eventAggregator = eventAggregator;
            _companyrepository = companyRepository;
            _lookupService = lookupService;

            _eventAggregator.GetEvent<CompanySavedEvent>().Subscribe(UpdateCompany);
            
            Companies = new ObservableCollection<LookupItem>();
            CompanyNotificationRequest = new InteractionRequest<IEditNotification>();

            EditCompanyNotificationCommand = new DelegateCommand(RaiseEditDialog,EditCanExecute);
            AddCompanyNotificationCommand = new DelegateCommand(RaiseAddDialog);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
           
            Load();
        }






        #region Methodes
      
        private void UpdateCompany(int companyId)
        {
            Companies.Clear();
            _companyrepository.ReloadCompany(companyId);
            var companies = _lookupService.GetAllCompaniesLookup();
            foreach (var company in companies)
            {
                Companies.Add(company);
            }
        }
        public void Load()
        {
            var companies = _lookupService.GetAllCompaniesLookup();
            foreach (var company in companies)
            {
                Companies.Add(company);
            }
        }
        private bool EditCanExecute()
        {
            return SelectedCompany != null;
        }
        private void RaiseAddDialog()
        {
            _eventAggregator.GetEvent<EditCompanyEvent>().Publish(0);

            CompanyNotificationRequest.Raise(
                new EditNotification()
                    { Title = "Add Company" },
                r =>
                {
                    if (r.Confirmed) { } else { }
                }
            );
        }
        private void RaiseEditDialog()
        {
            
            _eventAggregator.GetEvent<EditCompanyEvent>().Publish(SelectedCompany?.Id ?? 0);
           
            CompanyNotificationRequest.Raise(
                new EditNotification()
                    { Title = "Edit Company" },
                r =>
                {
                    if (r.Confirmed) { } else {  }
                }
            );
           
            
        }
        private bool OnDeleteCanExecute()
        {
            return SelectedCompany != null;
        }

        private void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Commands

        public InteractionRequest<IEditNotification> CompanyNotificationRequest { get; set; }
        public DelegateCommand EditCompanyNotificationCommand { get; set; }
        public DelegateCommand AddCompanyNotificationCommand { get; set; }
      
        public ICommand DeleteCommand { get; }
        #endregion

        #region Properties
        public ObservableCollection<LookupItem> Companies {
            get
            { return _companies; }
            set
            {
                SetProperty(ref _companies, value);
            } }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public LookupItem SelectedCompany
        {
            get { return _selectedCompany; }
            set
            {
                SetProperty(ref _selectedCompany, value);
                (EditCompanyNotificationCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();

            }
        }

        #endregion
    }
}
