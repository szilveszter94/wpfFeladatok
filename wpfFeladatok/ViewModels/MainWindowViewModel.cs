using System.ComponentModel;
using System.Windows;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    public RelayCommand ShowMessage { get; }
    public RelayCommand ShowLoginWindow { get; }
    private string _textContent = "";
    private bool _isPopupActive;
    private bool _isClosedByUser;
    private readonly AuthenticationService _authenticationService;
    private readonly LoginViewModel _loginViewModel;
    private LoginWindow? _loginWindow;
    
    private bool? _loginStatus;
    public bool? LoginStatus
    {
        get => _loginStatus;
        set
        {
            _loginStatus = value;
            OnPropertyChanged();
        }
    }
    
    public string TextContent
    {
        get { return _textContent; }
        set
        {
            if (_textContent != value)
            {
                _textContent = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsPopupActive
    {
        get { return _isPopupActive; }
        set
        {
            if (_isPopupActive != value)
            {
                _isPopupActive = value;
                OnPropertyChanged();
                ShowMessage.RaiseCanExecuteChanged();
                ShowLoginWindow.RaiseCanExecuteChanged();
            }
        }
    }
    
    public MainWindowViewModel()
    {
        ShowMessage = new RelayCommand(ShowMessageAction, _ => !IsPopupActive);
        ShowLoginWindow = new RelayCommand(ShowLoginView, _ => !IsPopupActive);
        _authenticationService = new AuthenticationService();
        _loginViewModel = new LoginViewModel(_authenticationService);
        _loginViewModel.IsLoginWindowClosed += IsClosedByUser;
        _authenticationService.IsLoggedInChanged += AuthenticationService_IsLoggedInChanged;
        LoginStatus = _authenticationService.IsLoggedIn;
    }

    private void ShowMessageAction(object obj)
    {
        MessageBox.Show("Operation has completed successfully");
    }
    
    private void ShowLoginView(object obj)
    {
        try
        {
            _loginWindow = new LoginWindow(_loginViewModel);
            _loginWindow.Closing += LoginWindow_Closed;
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                _loginWindow.Owner = mainWindow;
                _loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            _loginWindow.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void AuthenticationService_IsLoggedInChanged(object? sender, bool? e)
    {
        LoginStatus = e;
    }
    
    private void LoginWindow_Closed(object? sender, CancelEventArgs e)
    {
        if (!_isClosedByUser)
        {
            LoginStatus = false;
        }
    }
    
    private void IsClosedByUser(object? sender, EventArgs e)
    {
        LoginStatus = true;
        _isClosedByUser = true;
        _loginWindow?.Close();
        _isClosedByUser = false;
    }
}
