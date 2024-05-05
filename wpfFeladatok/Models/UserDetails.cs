using wpfFeladatok.ViewModels;

namespace wpfFeladatok.Models;

public class UserDetails : BaseViewModel
{
    private int _id;
    private int _userId;
    private string? _email;
    private string? _phone;
    private string? _address;

    public int Id
    {
        get { return _id; }
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged();
            }
        }
    }
    
    public int UserId
    {
        get { return _userId; }
        set
        {
            if (_userId != value)
            {
                _userId = value;
                OnPropertyChanged();
            }
        }
    }
    
    public string Email
    {
        get { return _email; }
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged();
            }
        }
    }
    
    public string Phone
    {
        get { return _phone; }
        set
        {
            if (_phone != value)
            {
                _phone = value;
                OnPropertyChanged();
            }
        }
    }
    
    public string Address
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
}