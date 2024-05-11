using System.Windows;
using wpfFeladatok.ViewModels;

namespace wpfFeladatok;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
