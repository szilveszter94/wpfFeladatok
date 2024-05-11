using System.Windows;
using System.Windows.Input;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels;

public class LoginViewModel : BaseViewModel
{
    public event EventHandler? IsLoginWindowClosed;
    public ICommand CheckLogin { get; }
    
    private readonly AuthenticationService _authenticationService;
    private bool _isErrorVisible;
    public bool IsErrorVisible
    {
        get { return _isErrorVisible; }
        set
        {
            if (_isErrorVisible != value)
            {
                _isErrorVisible = value;
                OnPropertyChanged();
            }
        }
    }

    public LoginViewModel(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        CheckLogin = new RelayCommand(ExecuteCheckLogin);
    }
    
    private void ExecuteCheckLogin(object parameter)
    {
        var loginWindow = GetLoginWindow();
        var isLoginSuccessful = _authenticationService.Login(loginWindow);
        if (isLoginSuccessful)
        {
            HandleSuccessfulLogin();
            return;
        }
        HandleUnsuccessfulLogin();
    }

    private void HandleSuccessfulLogin()
    {
        IsErrorVisible = false;
        OnIsLoginWindowClosed();
    }
    
    private void HandleUnsuccessfulLogin()
    {
        var loginWindow = GetLoginWindow();
        loginWindow.LoginBox.Clear();
        loginWindow.PasswordBox.Clear();
        IsErrorVisible = true;
    }

    private LoginWindow GetLoginWindow()
    {
        return Application.Current.Windows.OfType
            <LoginWindow>().SingleOrDefault(x => x.IsActive)!;
    }
    
    private void OnIsLoginWindowClosed()
    {
        IsLoginWindowClosed?.Invoke(null, EventArgs.Empty);
    }
}