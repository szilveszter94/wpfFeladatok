namespace wpfFeladatok.Service;

public class AuthenticationService
{
    private const string TestUserName = "Admin";
    private const string TestPassword = "Admin123";
    public event EventHandler<bool?>? IsLoggedInChanged;

    private bool? _isLoggedIn;
    public bool? IsLoggedIn
    {
        get => _isLoggedIn;
        private set
        {
            if (_isLoggedIn != value)
            {
                _isLoggedIn = value;
                OnIsLoggedInChanged(value);
            }
        }
    }

    public bool Login(LoginWindow loginWindow)
    {
        var password = loginWindow.PasswordBox.Password;
        var username = loginWindow.LoginBox.Text;
        if (username == TestUserName && password == TestPassword)
        {
            IsLoggedIn = true;
            return true;
        }
        IsLoggedIn = false;
        return false;
    }
    
    private void OnIsLoggedInChanged(bool? newValue)
    {
        IsLoggedInChanged?.Invoke(this, newValue);
    }
}