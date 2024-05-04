using System.Windows;
using System.Windows.Input;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _textContent = "";
        private bool _isPopupActive;
        public ICommand ShowMessage { get; }
        public ICommand ShowLoginWindow { get; }
        
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
                }
            }
        }
        

        public MainWindowViewModel()
        {
            ShowMessage = new RelayCommand(ShowMessageAction(), CanShowMessage);
            ShowLoginWindow = new RelayCommand(ShowLoginView(), CanShowMessage);
            Mediator.Mediator.LoginStatusChanged += UpdateStatus;
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
    }
}
