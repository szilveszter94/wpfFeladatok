using wpfFeladatok.ViewModels;

namespace wpfFeladatok.Models;

public class UserDetails : BaseViewModel
{
    private readonly int _id;
    private readonly int _userId;
    private string? _email;
    private string? _phone;
    private string? _address;

    public int Id
    {
        get => _id;
        init => SetProperty(ref _id, value);
    }
    
    public int UserId
    {
        get => _userId;
        init => SetProperty(ref _userId, value);
    }
    
    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }
    
    public string? Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }
    
    public string? Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }
}