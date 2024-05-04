namespace wpfFeladatok.ViewModels;

using Mediator;
using System.Windows.Input;
using Service;


public class LoginViewModel : BaseViewModel
{
    private const string TestUserName = "Admin";
    private const string TestPassword = "Admin123";

    public ICommand CheckLogin { get; }
    public Action? CloseAction { get; set; }
    public Action? ClearLoginFieldsAction { get; set; }

    public string? ActualUserName { get; set; }
    public string? ActualPassword { get; set; }

    private string _isErrorVisible = "Hidden";
    private string _status = "";
    public string Status
    {
        get { return _status; }
        set
        {
            _status = value;
            OnPropertyChanged();
            Mediator.NotifyLoginStatusChanged(value);
        }
    }

    public string IsErrorVisible
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

    public LoginViewModel()
    {
        CheckLogin = new RelayCommand(ExecuteCheckLogin);
    }
    
    private void ExecuteCheckLogin(object parameter)
    {
        if (ActualUserName == TestUserName && ActualPassword == TestPassword)
        {
            IsErrorVisible = "Hidden";
            CloseAction?.Invoke();
        }
        else
        {
            ClearLoginFieldsAction?.Invoke();
            Status = "Login failed!";
            IsErrorVisible = "Visible";
        }
    }
}
