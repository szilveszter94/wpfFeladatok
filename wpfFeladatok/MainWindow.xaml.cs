using wpfFeladatok.ViewModels;

namespace wpfFeladatok;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
