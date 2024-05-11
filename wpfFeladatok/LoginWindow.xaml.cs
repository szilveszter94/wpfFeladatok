using System.Windows;
using wpfFeladatok.ViewModels;

namespace wpfFeladatok;

public partial class LoginWindow : Window
{
    public LoginWindow(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        DataContext = loginViewModel;
    }
}