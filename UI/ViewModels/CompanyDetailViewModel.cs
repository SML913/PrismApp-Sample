using System;
using System.Windows;
using System.Windows.Input;
using Model;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using UI.Data.Repositories;
using UI.Event;
using UI.Notifications;
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
        private IEventAggregator _eventAggregator;

        public CompanyDetailViewModel(
            IEventAggregator eventAggregator,
            ICompanyRepository companyRepository)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EditCompanyEvent>().Subscribe(LoadCompany);
            _companyRepository = companyRepository;


            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            CloseCommand = new DelegateCommand(OnCloseExecute);
        }

        #region Methodes

        private void LoadCompany(int companyId)
        {
            if(Company!=null && Company.Id==companyId) _companyRepository.ReloadCompany(companyId);
            var company = companyId != 0 ? _companyRepository.GetById(companyId) : new Company();
            Company =  new CompanyWrapper(company);
            Company.PropertyChanged += (s, e) =>
            {
                if (!IsDirty)
                {
                    IsDirty = _companyRepository.HasChanges();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Company.Name))
                {
                    IsDirty = _companyRepository.HasChanges();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
        }
        private void OnCloseExecute()
        {
            IsDirty = false;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            
            _notification.Confirmed = true;
            FinishInteraction?.Invoke();
        }
        private bool OnSaveCanExecute()
        {
            return IsDirty;
        }
        private void OnSaveExecute()
        {
           _companyRepository.Save();
            IsDirty = _companyRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            _eventAggregator.GetEvent<CompanySavedEvent>().Publish(Company.Id);
        }


        #endregion

        #region Poroperties
        
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
       
        public INotification Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, (IEditNotification)value); }
        }
        public Action FinishInteraction { get; set; }

        #endregion

        #region commands
        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }


        #endregion
     }
}
