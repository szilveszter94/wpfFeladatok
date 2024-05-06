using System.Windows;
using wpfFeladatok.ViewModels;

namespace wpfFeladatok;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _loginViewModel;
    private bool _isLoginSuccessful;
    public LoginWindow(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        _loginViewModel = loginViewModel;
        loginViewModel.CloseAction = () =>
        {
            _isLoginSuccessful = true;
            Close();
        };
        loginViewModel.ClearLoginFieldsAction = () =>
        {
            LoginBox.Clear();
            PasswordBox.Clear();
        };
        DataContext = loginViewModel;
    }
    
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        if (_isLoginSuccessful)
        {
            _loginViewModel.LoginStatus = "Login successful";
        }
        else
        {
            _loginViewModel.LoginStatus = "Login failed!";
        }
    }
}