using System.Windows;
using System.Windows.Input;
using wpfFeladatok.Service;

namespace wpfFeladatok.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _textContent = "";
        private bool _isToggleActive = false;
        public ICommand ShowMessage;

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

        public bool IsToggleActive
        {
            get { return _isToggleActive; }
            set
            {
                if (_isToggleActive != value)
                {
                    _isToggleActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindowViewModel()
        {
            ShowMessage = new RelayCommand(ShowMessageAction(), CanShowMessage);
        }

        public static Action<object> ShowMessageAction()
        {
            return (obj) => MessageBox.Show("Operation has completed successfully", "Message");
        }

        private bool CanShowMessage(object parameter)
        {
            return !IsToggleActive;
        }
    }
}
