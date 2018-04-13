using System.Threading.Tasks;
using System.Windows;

namespace UI.Services
{
    public interface IDialogService
    {
        Task<bool> ShowOkCancelDialog(string title, string message);
        Task ShowInfoDialog(string message);

      
    }
}