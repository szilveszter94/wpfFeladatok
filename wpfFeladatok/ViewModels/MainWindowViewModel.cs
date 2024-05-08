using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using wpfFeladatok.Models;
using wpfFeladatok.Models.Enums;
using wpfFeladatok.Repository;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public int MaxValue => 255;
        public int MinValue => 0;
        public ObservableCollection<UserDetails> UserDetails { get; }
        public ObservableCollection<User> Users { get; }
        public ICommand ShowMessageCommand { get; }
        public ICommand ShowLoginWindowCommand { get; }
        public ICommand AddNewUserCommand { get; }
        public ICommand SaveUserCommand { get; }
        
        public ICommand DeleteUserCommand { get; }
        public ICommand CancelCommand { get; }
        public Action? UnselectUserListAction { get; set; }
        public ObservableCollection<ThemeEnum> ThemeOptions { get; private set; } = new ((ThemeEnum[])Enum.GetValues(typeof(ThemeEnum)));
        
        private readonly DispatcherTimer _timer;
        
        public string? Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        private string? _address;
        
        public int BlueValue
        {
            get => _blueValue;
            set => SetProperty(ref _blueValue, value);
        }
        private int _blueValue;
        
        public DateTime CurrentTime
        {
            get => _currentTime;
            private set => SetProperty(ref _currentTime, value);
        }
        private DateTime _currentTime = DateTime.Now;
        
        public string? EmailAddress
        {
            get => _emailAddress;
            set => SetProperty(ref _emailAddress, value);
        }
        private string? _emailAddress;
        
        public string? FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        private string? _firstName;
        
        public int GreenValue
        {
            get => _greenValue;
            set => SetProperty(ref _greenValue, value);
        }
        private int _greenValue;
        
        public bool IsAddUserEnabled
        {
            get => _isAddUserEnabled;
            set => SetProperty(ref _isAddUserEnabled, value);
        }
        private bool _isAddUserEnabled = true;
        
        public bool IsAnimationCheckBoxChecked
        {
            get => _isAnimationCheckBoxChecked;
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
                    SetProperty(ref _isAnimationCheckBoxChecked, value);
                }
            }
        }
        private bool _isAnimationCheckBoxChecked;
        
        public bool IsPopupActive
        {
            get => _isPopupActive;
            set => SetProperty(ref _isPopupActive, value);
        }
        private bool _isPopupActive;
        
        public bool IsSaveAndDeleteUserEnabled
        {
            get => _isSaveAndDeleteAndDeleteUserEnabled;
            set => SetProperty(ref _isSaveAndDeleteAndDeleteUserEnabled, value);
        }
        private bool _isSaveAndDeleteAndDeleteUserEnabled;
        
        public bool IsSwitchThemeEnabled
        {
            get => _isSwitchThemeEnabled;
            set => SetProperty(ref _isSwitchThemeEnabled, value);
        }
        private bool _isSwitchThemeEnabled;
        
        public bool IsThemeChangeEnableCheckBoxChecked
        {
            get => _isThemeChangeEnableCheckBoxChecked;
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
                    SetProperty(ref _isThemeChangeEnableCheckBoxChecked, value);
                }
            }
        }
        private bool _isThemeChangeEnableCheckBoxChecked;
        
        public bool IsUserDetailsErrorMessageVisible
        {
            get => _isUserDetailErrorMessageVisible;
            set => SetProperty(ref _isUserDetailErrorMessageVisible, value);
        }
        private bool _isUserDetailErrorMessageVisible;
        
        public bool IsUserInputErrorMessageVisible
        {
            get => _isUserInputErrorMessageVisible;
            set => SetProperty(ref _isUserInputErrorMessageVisible, value);
        }
        private bool _isUserInputErrorMessageVisible;
        
        public string? LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        private string? _lastName;
        
        public string LoginStatus
        {
            get => _loginLoginStatus;
            private set => SetProperty(ref _loginLoginStatus, value);
        }
        private string _loginLoginStatus = "";
        
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }
        private string? _phoneNumber;
        
        public ThemeEnum SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (value != _selectedTheme)
                {
                    HandleThemeChange(value);
                    SetProperty(ref _selectedTheme, value);
                }
            }
        }
        private ThemeEnum _selectedTheme;

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    HandleSelect(value);
                    SetProperty(ref _selectedUser, value);
                }
            }
        }
        private User? _selectedUser;
        
        public int RedValue
        {
            get => _redValue;
            set => SetProperty(ref _redValue, value);
        }
        private int _redValue;
        
        public string TextContent
        {
            get => _textContent;
            set => SetProperty(ref _textContent, value);
        }
        private string _textContent = "";
        
        public MainWindowViewModel()
        {
            AddNewUserCommand = new RelayCommand(AddNewUser);
            CancelCommand = new RelayCommand(ResetDefaultElementStates);
            Mediator.Mediator.LoginStatusChanged += UpdateStatus;
            SaveUserCommand = new RelayCommand(SaveUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            SelectedTheme = ThemeOptions.Count > 0 ? ThemeOptions[0] : ThemeEnum.Dark;
            ShowLoginWindowCommand = new RelayCommand(ShowLoginView(), CanShowMessage);
            ShowMessageCommand = new RelayCommand(ShowMessageAction(), CanShowMessage);
            IUsersRepository usersRepository = new UsersRepository();
            IUserDetailsRepository userDetailsRepository = new UserDetailsRepository();
            Users = usersRepository.UserList;
            UserDetails = userDetailsRepository.UserDetailsList;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        
        private void AddNewUser(object obj)
        {
            if (!ValidateInput())
            {
                IsUserInputErrorMessageVisible = true;
                return;
            }

            int newUserId = GetNextUserId();
            CreateUser(newUserId);
            ResetDefaultElementStates();
        }
        
        private void CreateUser(int newUserId)
        {
            var newUser = new User { Id = newUserId, FirstName = _firstName ?? "", LastName = _lastName ?? "" };
            Users.Add(newUser);
            UpdateUserDetails(newUser);
        }
        
        private void CreateUserDetails(int userId)
        {
            var newDetailId = UserDetails.Any() ? UserDetails.Last().Id + 1 : 1;
            var details = new UserDetails
            {
                Id = newDetailId,
                UserId = userId,
                Address = Address ?? "",
                Email = EmailAddress ?? "",
                Phone = PhoneNumber ?? ""
            };
            UserDetails.Add(details);
        }
        
        private void SaveUser(object obj)
        {
            if (!ValidateInput())
            {
                IsUserInputErrorMessageVisible = true;
                return;
            }

            var existingUser = Users.FirstOrDefault(u => u.Id == _selectedUser?.Id);
            if (existingUser != null)
            {
                ModifyExistingUser(existingUser);
                UpdateUserDetails(existingUser);
                ResetDefaultElementStates();
            }
        }
        
        private void DeleteUser(object obj)
        {
            if (!ValidateInput())
            {
                IsUserInputErrorMessageVisible = true;
                return;
            }

            var existingUser = Users.FirstOrDefault(u => u.Id == _selectedUser?.Id);
            if (existingUser != null)
            {
                DeleteExistingUser(existingUser);
                UpdateUserDetails(existingUser);
                ResetDefaultElementStates();
            }
        }
        
        private void UpdateUserDetails(User existingUser)
        {
            var userDetails = GetUserDetails(existingUser);
            if (userDetails == null)
            {
                CreateUserDetails(existingUser.Id);
            }
            else
            {
                userDetails.Address = Address ?? "";
                userDetails.Email = EmailAddress ?? "";
                userDetails.Phone = PhoneNumber ?? "";
            }
        }
        
        private UserDetails? GetUserDetails(User user)
        {
            return UserDetails.FirstOrDefault(d => d.UserId == user.Id);
        }
        
        private void HandleSelect(User? user)
        {
            IsAddUserEnabled = false;
            IsSaveAndDeleteUserEnabled = true;
            FirstName = user?.FirstName;
            LastName = user?.LastName;
            _selectedUser = user;
            var selectedDetails = UserDetails.FirstOrDefault(d => d.UserId == user?.Id);
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
        
        private int GetNextUserId()
        {
            return Users.Any() ? Users.Last().Id + 1 : 1;
        }
        
        private void ResetInputFields()
        {
            FirstName = "";
            LastName = "";
            EmailAddress = "";
            PhoneNumber = "";
            Address = "";
        }
        
        private void UnselectSelectedUser()
        {
            UnselectUserListAction?.Invoke();
            SelectedUser = new User();
        }
        
        private void ModifyExistingUser(User existingUser)
        {
            existingUser.FirstName = FirstName ?? "";
            existingUser.LastName = LastName ?? "";
        }
        
        private void DeleteExistingUser(User existingUser)
        {
            Users.Remove(existingUser);
        }
        
        private void ResetDefaultElementStates(object? obj = null)
        {
            UnselectSelectedUser();
            ResetInputFields();
            SetDefaultValuesForButtonsAndErrorMessages();
        }
        
        private bool ValidateInput()
        {
            return !string.IsNullOrEmpty(_firstName) && !string.IsNullOrEmpty(_lastName);
        }
        
        private Action<object?> ShowLoginView()
        {
            return _ =>
            {
                try
                {
                    ShowLoginDialog();
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                }
            };
        }
        
        private void ShowLoginDialog(){
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
        
        private Action<object?> ShowMessageAction()
        {
            return _ => MessageBox.Show("Operation has completed successfully");
        }
        
        private void Timer_Tick(object? sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }
        
        private bool CanShowMessage(object? parameter)
        {
            return !_isPopupActive;
        }
        
        private void UpdateStatus(string status)
        {
            LoginStatus = status;
        }

        private void HandleThemeChange(ThemeEnum theme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            LoadResourceDictionary($"/Resources/Themes/{theme}/ColorResources.xaml");
            LoadResourceDictionary("/Resources/Themes/ImageResources.xaml");
        }
        
        private void HandleError(Exception ex)
        {
            MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        private void SetDefaultValuesForButtonsAndErrorMessages()
        {
            IsUserDetailsErrorMessageVisible = false;
            IsUserInputErrorMessageVisible = false;
            IsAddUserEnabled = true;
            IsSaveAndDeleteUserEnabled = false;
        }
        
        private void LoadResourceDictionary(string resourcePath)
        {
            var uri = new Uri(resourcePath, UriKind.Relative);
            var resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
