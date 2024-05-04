using System.Windows;
using wpfFeladatok.ViewModels;

namespace wpfFeladatok;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _loginViewModel;
    private bool _closedByUser = false;
    public LoginWindow(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        _loginViewModel = loginViewModel;
        loginViewModel.CloseAction = () =>
        {
            _closedByUser = true;
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
        if (_closedByUser)
        {
            _loginViewModel.Status = "Login successful";
        }
        else
        {
            _loginViewModel.Status = "Login failed!";
        }
    }
}