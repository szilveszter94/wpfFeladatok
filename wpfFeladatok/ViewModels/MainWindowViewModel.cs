using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using wpfFeladatok.Models;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; } = new ()
        {
            new User { Id =1, LastName = "Nagy", FirstName = "Kati"},
            new User{Id =2, LastName = "Kovács", FirstName = "Zoli"},
            new User{Id =3, LastName = "Kis", FirstName = "Pisti"}
        };

        public ObservableCollection<UserDetails> UserDetails { get; } = new()
        {
            new ()
                { Address = "Main st. 123", Id = 1, UserId = 1, Phone = "075684954", Email = "email@qewqada.com" },
            new ()
                { Address = "Main st. 153", Id = 2, UserId = 2, Phone = "231223134", Email = "email@afqew.com" },
            new ()
                { Address = "Main st. 2321", Id = 3, UserId = 3, Phone = "1249123154", Email = "email@qwe13.com" }
        };
        public int MaxValue { get; } = 255;
        public int MinValue { get; } = 0;
        private string _textContent = "";
        private bool _isAnimationCheckBoxChecked;
        private bool _isThemeChangeEnableCheckBoxChecked;
        private bool _isPopupActive;
        private bool _isSwitchThemeEnabled;

        private string? _firstName;
        private string? _lastName;
        private string? _address;
        private string? _phoneNumber;
        private string? _emailAddress;
        private User? _selectedUser;
        private bool _isUserInputErrorMessageVisible;
        private bool _isAddUserEnabled = true;
        private bool _isSaveUserEnabled;
        private bool _isUserDetailErrorMessageVisible;
        
        private DateTime _currentTime = DateTime.Now;
        private DispatcherTimer _timer;
        
        private int _redValue;
        private int _greenValue;
        private int _blueValue;
        public ICommand ShowMessageCommand { get; }
        public ICommand ShowLoginWindowCommand { get; }
        
        public ICommand SwitchThemeCommand { get; }
        public ICommand AddNewUserCommand { get; }
        public ICommand SaveUserCommand { get; }
        public ICommand CancelCommand { get; }
        public Action? UnselectUserListAction { get; set; }
        
        
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
        
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (_emailAddress != value)
                {
                    _emailAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        public User? SelectedUser
        {
            get { return _selectedUser;}
            set
            {
                if (_selectedUser != value)
                {
                    HandleSelect(value);
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAddUserEnabled
        {
            get { return _isAddUserEnabled; }
            set
            {
                if (_isAddUserEnabled != value)
                {
                    _isAddUserEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsSaveUserEnabled
        {
            get { return _isSaveUserEnabled; }
            set
            {
                if (_isSaveUserEnabled != value)
                {
                    _isSaveUserEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsUserInputErrorMessageVisible
        {
            get { return _isUserInputErrorMessageVisible; }
            set
            {
                if (_isUserInputErrorMessageVisible != value)
                {
                    _isUserInputErrorMessageVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsUserDetailsErrorMessageVisible
        {
            get { return _isUserDetailErrorMessageVisible; }
            set
            {
                if (_isUserDetailErrorMessageVisible != value)
                {
                    _isUserDetailErrorMessageVisible = value;
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
            ShowMessageCommand = new RelayCommand(ShowMessageAction(), CanShowMessage);
            ShowLoginWindowCommand = new RelayCommand(ShowLoginView(), CanShowMessage);
            Mediator.Mediator.LoginStatusChanged += UpdateStatus;
            SwitchThemeCommand = new RelayCommand(ToggleThemes);
            AddNewUserCommand = new RelayCommand(AddNewUser);
            SaveUserCommand = new RelayCommand(SaveUser);
            CancelCommand = new RelayCommand(ClearUserEditor);
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
        
        private void ToggleThemes(object obj)
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
        
        private void AddNewUser(object obj)
        {
            if (string.IsNullOrEmpty(_firstName) || string.IsNullOrEmpty(_lastName))
            {
                IsUserInputErrorMessageVisible = true;
            }
            else
            {
                int newUserId = Users.Last().Id + 1;
                Users.Add(new User{Id = newUserId, FirstName = FirstName, LastName = LastName});
                FirstName = "";
                LastName = "";
            }
        }
        
        private void SaveUser(object obj)
        {
            if (string.IsNullOrEmpty(_firstName) || string.IsNullOrEmpty(_lastName))
            {
                IsUserInputErrorMessageVisible = true;
            }
            else
            {
                var existingUser = Users.FirstOrDefault(u => u.Id == _selectedUser?.Id);
                if (existingUser != null)
                {
                    existingUser.FirstName = FirstName;
                    existingUser.LastName = LastName;
                    ModifyUserDetails(existingUser);
                    UnselectUserListAction?.Invoke();
                    SelectedUser = new User();
                    ResetUserProperties();
                    IsAddUserEnabled = true;
                    IsSaveUserEnabled = false;
                    IsUserDetailsErrorMessageVisible = false;
                }
            }
        }

        private void ClearUserEditor(object obj)
        {
            UnselectUserListAction?.Invoke();
            SelectedUser = new User();
            ResetUserProperties();
            IsUserDetailsErrorMessageVisible = false;
            IsAddUserEnabled = true;
            IsSaveUserEnabled = false;
        }
        
        private void HandleSelect(User user)
        {
            IsAddUserEnabled = false;
            IsSaveUserEnabled = true;
            FirstName = user.FirstName;
            LastName = user.LastName;
            _selectedUser = user;
            var selectedDetails = UserDetails.FirstOrDefault(d => d.UserId == user.Id);
            if (selectedDetails == null || (string.IsNullOrEmpty(selectedDetails.Address) && (string.IsNullOrEmpty(selectedDetails.Email) && (string.IsNullOrEmpty(selectedDetails.Phone)))))
            {
                EmailAddress = "";
                PhoneNumber = "";
                Address = "";
                IsUserDetailsErrorMessageVisible = true;
            }
            else
            {
                IsUserDetailsErrorMessageVisible = false;
                EmailAddress = selectedDetails.Email;
                PhoneNumber = selectedDetails.Phone;
                Address = selectedDetails.Address;
            }
        }

        private void ModifyUserDetails(User existingUser)
        {
            var userDetails = UserDetails.FirstOrDefault(d => d.UserId == existingUser.Id);
            if (userDetails == null)
            {
                var newDetailId = UserDetails.Last().Id + 1;
                var details = new UserDetails()
                {
                    Id = newDetailId, UserId = existingUser.Id, Address = Address, Email = EmailAddress,
                    Phone = PhoneNumber
                };
                UserDetails.Add(details);
            }
            else
            {
                userDetails.Address = Address;
                userDetails.Email = EmailAddress;
                userDetails.Phone = PhoneNumber;
            }
        }

        private void ResetUserProperties()
        {
            FirstName = "";
            LastName = "";
            EmailAddress = "";
            PhoneNumber = "";
            Address = "";
        }
    }
}
