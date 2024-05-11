using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public int MaxValue { get; } = 255;
        public int MinValue { get; } = 0;
        public RelayCommand SwitchTheme { get; }
        public RelayCommand ShowMessage { get; }
        public RelayCommand ShowLoginWindow { get; }
        
        private bool _isAnimationCheckBoxChecked;
        private bool _isThemeChangeEnableCheckBoxChecked;
        private bool _isSwitchThemeEnabled;
        private DateTime _currentTime = DateTime.Now;
        private DispatcherTimer _timer;
        private int _redValue;
        private int _greenValue;
        private int _blueValue;
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
        
        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
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

        public bool IsSwitchThemeEnabled
        {
            get { return _isSwitchThemeEnabled; }
            set {
                if (_isSwitchThemeEnabled != value)
                {
                    _isSwitchThemeEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsThemeChangeEnableCheckBoxChecked
        {
            get { return _isThemeChangeEnableCheckBoxChecked; }
            set
            {
                if (_isThemeChangeEnableCheckBoxChecked != value)
                {
                    if (!value || _isAnimationCheckBoxChecked)
                    {
                        IsSwitchThemeEnabled = false;
                    }
                    else
                    {
                        IsSwitchThemeEnabled = true;
                    }
                    _isThemeChangeEnableCheckBoxChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAnimationCheckBoxChecked
        {
            get { return _isAnimationCheckBoxChecked; }
            set
            {
                if (_isAnimationCheckBoxChecked != value)
                {
                    if (value || !_isThemeChangeEnableCheckBoxChecked)
                    {
                        IsSwitchThemeEnabled = false;
                    }
                    else
                    {
                        IsSwitchThemeEnabled = true;
                    }
                    _isAnimationCheckBoxChecked = value;
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
        
        public int RedValue
        {
            get { return _redValue; }
            set
            {
                if (_redValue != value)
                {
                    _redValue = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int GreenValue
        {
            get { return _greenValue; }
            set
            {
                if (_greenValue != value)
                {
                    _greenValue = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int BlueValue
        {
            get { return _blueValue; }
            set
            {
                if (_blueValue != value)
                {
                    _blueValue = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public MainWindowViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            ShowMessage = new RelayCommand(ShowMessageAction, _ => !IsPopupActive);
            ShowLoginWindow = new RelayCommand(ShowLoginView, _ => !IsPopupActive);
            _authenticationService = new AuthenticationService();
            _loginViewModel = new LoginViewModel(_authenticationService);
            _loginViewModel.IsLoginWindowClosed += IsClosedByUser;
            _authenticationService.IsLoggedInChanged += AuthenticationService_IsLoggedInChanged;
            LoginStatus = _authenticationService.IsLoggedIn;
            SwitchTheme = new RelayCommand(ToggleThemes);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
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
        
        public void ToggleThemes(object obj)
        {
            if (obj is bool isLightTheme)
            {
                var theme = isLightTheme ? "Light" : "Dark";
                Application.Current.Resources.MergedDictionaries.Clear();
                Uri uri1 = new Uri($"/Resources/Themes/{theme}/ColorResources.xaml", UriKind.Relative);
                var resourceDict = Application.LoadComponent(uri1) as ResourceDictionary;
                Uri uri2 = new Uri("/Resources/Themes/ImageResources.xaml", UriKind.Relative);
                var resourceDict2 = Application.LoadComponent(uri2) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                Application.Current.Resources.MergedDictionaries.Add(resourceDict2);
            }
        }
    }
}
