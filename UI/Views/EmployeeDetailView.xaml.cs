using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for EmployeeDetailView.xaml
    /// </summary>
    public partial class EmployeeDetailView : UserControl
    {
        public EmployeeDetailView()
        {
            InitializeComponent();
            Loaded += EmployeeDetailView_Loaded;
        }

        private void EmployeeDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(Parent is Window parentWindow)) return;
            parentWindow.WindowStyle = WindowStyle.None;
            parentWindow.ResizeMode = ResizeMode.NoResize;
            parentWindow.MinWidth = 300;
            parentWindow.MinHeight = 300;
            parentWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
