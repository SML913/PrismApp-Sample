using System.Windows;
using Prism.Unity;
using UI.Views;
using Microsoft.Practices.Unity;
using UI.Data.Repositories;
using UI.Services;

namespace UI.Startup
{
    public class Bootstrapper:UnityBootstrapper
    {
       
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterType<ICompanyRepository, CompanyRepository>();
            Container.RegisterType<IEmployeeRepository, EmployeeRepository>();
            Container.RegisterType<ICommonService, CommonService>();
            Container.RegisterType<IDialogService, DialogService>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
