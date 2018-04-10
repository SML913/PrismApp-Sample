using System.Windows;

namespace UI.Services
{
    public class DialogService
    {
        public DialogService()
        {
             
        }

        public Window Window { get; set; }

        public  void ShowDialog()
        {
           
        }

        public  void CloseDialog()
        {
            Window.Close();
        }
    }

   
}
