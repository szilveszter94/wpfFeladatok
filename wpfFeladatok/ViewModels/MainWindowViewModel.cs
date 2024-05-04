using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _textContent = "";
        private bool _isAnimationCheckBoxChecked;
        private bool _isThemeChangeEnableCheckBoxChecked;
        private bool _isPopupActive;
        private bool _isSwitchThemeEnabled;
        private DateTime _currentTime = DateTime.Now;
        private DispatcherTimer _timer;
        public ICommand ShowMessage { get; }
        public ICommand ShowLoginWindow { get; }
        
        public ICommand SwitchToDarkThemeCommand { get; }
        public ICommand SwitchToLightThemeCommand { get; }
        
        
        private string _status = "";

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
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
                }
            }
        }
        
        public MainWindowViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            ShowMessage = new RelayCommand(ShowMessageAction(), CanShowMessage);
            ShowLoginWindow = new RelayCommand(ShowLoginView(), CanShowMessage);
            Mediator.Mediator.LoginStatusChanged += UpdateStatus;
            SwitchToDarkThemeCommand = new RelayCommand(SwitchToDarkTheme());
            SwitchToLightThemeCommand = new RelayCommand(SwitchToLightTheme());
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }
        
        private Action<object?> ShowMessageAction()
        {
            return (obj) => MessageBox.Show("Operation has completed successfully");
        }
        
        private Action<object?> ShowLoginView()
        {
            return (obj) =>
            {
                try
                {
                    var loginViewModel = new LoginViewModel();
                    var loginWindow = new LoginWindow(loginViewModel);

                    var mainWindow = Application.Current.MainWindow;
                    if (mainWindow != null)
                    {
                        loginWindow.Owner = mainWindow;
                        loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    }

                    loginWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
        }

        private bool CanShowMessage(object? parameter)
        {
            return !_isPopupActive;
        }
        
        private void UpdateStatus(string status)
        {
            Status = status;
        }
        
        public Action<object?> SwitchToDarkTheme()
        {
            return (obj) =>
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("/Resources/Themes/Dark/ColorResources.xaml", UriKind.RelativeOrAbsolute)
                });
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("/Resources/Themes/ImageResources.xaml", UriKind.RelativeOrAbsolute)
                });
            };
        }

        public Action<object?> SwitchToLightTheme()
        {
            return (obj) =>
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(
                    new ResourceDictionary() 
                    { 
                        Source = new Uri("/Resources/Themes/Light/ColorResources.xaml", UriKind.RelativeOrAbsolute) 
                    });
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("/Resources/Themes/ImageResources.xaml", UriKind.RelativeOrAbsolute)
                });
            };
        }
    }
}
