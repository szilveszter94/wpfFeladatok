namespace wpfFeladatok.ViewModels;

using Mediator;
using System.Windows.Input;
using Service;


public class LoginViewModel : BaseViewModel
{
    public ICommand CheckLogin { get; }
    public Action? CloseAction { get; set; }
    public Action? ClearLoginFieldsAction { get; set; }
    public string? ActualUserName { get; set; }
    public string? ActualPassword { get; set; }
    
    private const string TestUserName = "Admin";
    private const string TestPassword = "Admin123";
    private bool _isErrorVisible;
    private string _loginLoginStatus = "";
    
    public string LoginStatus
    {
        get => _loginLoginStatus;
        set
        {
            SetProperty(ref _loginLoginStatus, value);
            Mediator.NotifyLoginStatusChanged(value);
        }
    }

    public bool IsErrorVisible
    {
        get => _isErrorVisible;
        set => SetProperty(ref _isErrorVisible, value);
    }

    public LoginViewModel()
    {
        CheckLogin = new RelayCommand(ExecuteCheckLogin);
    }
    
    private void ExecuteCheckLogin(object parameter)
    {
        if (ActualUserName == TestUserName && ActualPassword == TestPassword)
        {
            IsErrorVisible = false;
            CloseAction?.Invoke();
        }
        else
        {
            ClearLoginFieldsAction?.Invoke();
            LoginStatus = "Login failed!";
            IsErrorVisible = true;
        }
    }
}
