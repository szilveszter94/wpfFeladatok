using System.Windows;

using wpfFeladatok.ViewModels;

namespace wpfFeladatok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainWindow = new MainWindowViewModel();
            mainWindow.UnselectUserListAction = () =>
            {
                UserList.UnselectAll();
            };
            DataContext = mainWindow;
        }
    }
}