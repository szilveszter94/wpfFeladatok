using wpfFeladatok.ViewModels;

namespace wpfFeladatok.Models;

public class User : BaseViewModel
{
    private readonly int _id;
    private string? _firstName;
    private string? _lastName;

    public int Id
    {
        get => _id;
        init => SetProperty(ref _id, value);
    }
    
    public string? FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }
    
    public string? LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }
    
};