using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
        private Company _selectedCompany;
        private string _title;
        
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<Company> _companies;

        public CompanyListViewModel(IEventAggregator eventAggregator,
            ICompanyRepository  companyRepository)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<CompanySavedEvent>().Subscribe(UpdateCompany);
            _companyrepository = companyRepository;
            Companies = new ObservableCollection<Company>();

            CompanyNotificationRequest = new InteractionRequest<IEditNotification>();
            EditCompanyNotificationCommand = new DelegateCommand(RaiseEditDialog,EditCanExecute);
            AddCompanyNotificationCommand = new DelegateCommand(RaiseAddDialog);
            Load();
        }




        #region Methodes
        private void UpdateCompany(int companyId)
        {
            Companies.Clear();
            _companyrepository.ReloadCompany(companyId);
            Companies = _companyrepository.GetAll();
        }
        public void Load()
        {
            Companies = _companyrepository.GetAll();
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

        #endregion

        #region Commands

        public InteractionRequest<IEditNotification> CompanyNotificationRequest { get; set; }
        public DelegateCommand EditCompanyNotificationCommand { get; set; }
        public DelegateCommand AddCompanyNotificationCommand { get; set; }


        #endregion

        #region Properties
        public ObservableCollection<Company> Companies {
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
        public Company SelectedCompany
        {
            get { return _selectedCompany; }
            set
            {
                SetProperty(ref _selectedCompany, value);
                (EditCompanyNotificationCommand).RaiseCanExecuteChanged();

            }
        }

        #endregion
    }
}
