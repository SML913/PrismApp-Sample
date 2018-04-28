using System.Threading.Tasks;
using System.Windows;

namespace UI.Services
{
    public interface IDialogService
    {
        Task<bool> ShowOkCancelDialog(string title, string message);
        Task ShowInfoDialog(string message);

       

         bool ShowOkCancelDialogUsingMsgBox(string title, string message);
         void ShowInfoDialogUsingMsgBox(string message);



    }
}