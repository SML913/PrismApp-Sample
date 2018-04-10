using System.Windows;
using System.Windows.Controls;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for CompanyDetailView.xaml
    /// </summary>
    public partial class CompanyDetailView : UserControl
    {
        public CompanyDetailView()
        {
            InitializeComponent();
            Loaded += CompanyDetailView_Loaded;
        }

        private void CompanyDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(Parent is Window parentWindow)) return;
            
            parentWindow.WindowStyle = WindowStyle.None;
            parentWindow.ResizeMode = ResizeMode.NoResize;
            parentWindow.MinHeight = 300;
            parentWindow.MinWidth = 300;
        }
    }
}
