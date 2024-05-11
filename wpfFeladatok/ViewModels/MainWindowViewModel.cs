using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using wpfFeladatok.Models;
using wpfFeladatok.Models.Enums;
using wpfFeladatok.Repository;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    public int MaxValue => 255;
    public int MinValue => 0;
    public ObservableCollection<UserDetails> UserDetails { get; }
    public ObservableCollection<User> Users { get; }
    public RelayCommand ShowMessageCommand { get; }
    public RelayCommand ShowLoginWindowCommand { get; }
    public RelayCommand AddNewUserCommand { get; }
    public RelayCommand SaveUserCommand { get; }
    
    public RelayCommand DeleteUserCommand { get; }
    public RelayCommand CancelCommand { get; }
    public ObservableCollection<ThemeEnum> ThemeOptions { get; private set; } = new ((ThemeEnum[])Enum.GetValues(typeof(ThemeEnum)));
    
    private readonly AuthenticationService _authenticationService;
    private bool _isClosedByUser;
    private readonly LoginViewModel _loginViewModel;
    private LoginWindow? _loginWindow;
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
        set
        {
            SetProperty(ref _isPopupActive, value);
            ShowMessageCommand.RaiseCanExecuteChanged();
            ShowLoginWindowCommand.RaiseCanExecuteChanged();
        }
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
    
    public bool? LoginStatus
    {
        get => _loginLoginStatus;
        private set => SetProperty(ref _loginLoginStatus, value);
    }
    private bool? _loginLoginStatus;
    
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
    
    public string TopInputFieldTextContent
    {
        get => _topInputFieldTextContent;
        set => SetProperty(ref _topInputFieldTextContent, value);
    }
    private string _topInputFieldTextContent = "";
    
    public MainWindowViewModel()
    {
        _authenticationService = new AuthenticationService();
        _authenticationService.IsLoggedInChanged += AuthenticationService_IsLoggedInChanged;
        LoginStatus = _authenticationService.IsLoggedIn;
        AddNewUserCommand = new RelayCommand(AddNewUser);
        CancelCommand = new RelayCommand(ResetDefaultElementStates);
        SaveUserCommand = new RelayCommand(SaveUser);
        DeleteUserCommand = new RelayCommand(DeleteUser);
        _loginViewModel = new LoginViewModel(_authenticationService);
        _loginViewModel.IsLoginWindowClosed += IsClosedByUser;
        SelectedTheme = ThemeOptions.Count > 0 ? ThemeOptions[0] : ThemeEnum.Dark;
        ShowLoginWindowCommand = new RelayCommand(ShowLoginView, _ => !IsPopupActive);
        ShowMessageCommand = new RelayCommand(ShowMessageAction, _ => !IsPopupActive);
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
        var mainWindow = GetMainWindow();
        mainWindow.UserList.UnselectAll();
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
    
    private void ShowLoginView(object obj)
    {
        try
        {
            ShowLoginDialog();
        }
        catch (Exception ex)
        {
            HandleLoginError(ex);
        }
    }
    
    private void ShowLoginDialog(){
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
    
    private void ShowMessageAction(object obj)
    {
        MessageBox.Show("Operation has completed successfully");
    }
    
    private void Timer_Tick(object? sender, EventArgs e)
    {
        CurrentTime = DateTime.Now;
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

    private void HandleThemeChange(ThemeEnum theme)
    {
        Application.Current.Resources.MergedDictionaries.Clear();
        LoadResourceDictionary($"/Resources/Themes/{theme}/ColorResources.xaml");
        LoadResourceDictionary("/Resources/Themes/ImageResources.xaml");
    }
    
    private void HandleLoginError(Exception ex)
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
    
    private MainWindow GetMainWindow()
    {
        return Application.Current.Windows.OfType
            <MainWindow>().SingleOrDefault(x => x.IsActive)!;
    }
}

