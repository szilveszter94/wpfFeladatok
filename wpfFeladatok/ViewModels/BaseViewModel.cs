using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace wpfFeladatok.ViewModels
{
    public class BaseViewModel : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}



















































































