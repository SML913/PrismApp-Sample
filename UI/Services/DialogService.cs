using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace UI.Services
{
    public class DialogService : IDialogService
    {
        private MetroWindow MetroWindow=> (MetroWindow)Application.Current.MainWindow;
       
  

        public async Task<bool> ShowOkCancelDialog(string title, string message)
        {
          var result = await MetroWindow.ShowMessageAsync(title,message,MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative) return true;
            return false;
        }

        public async Task ShowInfoDialog(string message)
        {
           await MetroWindow.ShowMessageAsync("Info",message);
        }


       
    }

   
}
