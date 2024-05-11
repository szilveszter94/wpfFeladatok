using wpfFeladatok.ViewModels;

namespace wpfFeladatok;

public partial class LoginWindow
{
    public LoginWindow(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        DataContext = loginViewModel;
    }
}