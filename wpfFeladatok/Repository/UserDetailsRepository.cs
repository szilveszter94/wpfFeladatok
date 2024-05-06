using System.Collections.ObjectModel;
using wpfFeladatok.Models;

namespace wpfFeladatok.Repository;

public class UserDetailsRepository : IUserDetailsRepository
{
    public ObservableCollection<UserDetails> UserDetailsList { get; } = new ()
    {
        new UserDetails { Id = 1, UserId = 1, Address = "Main st. 123", Phone = "075684954", Email = "email@qewqada.com" },
        new UserDetails { Id = 2, UserId = 2, Address = "Main st. 153", Phone = "231223134", Email = "email@afqew.com" },
        new UserDetails { Id = 3, UserId = 3, Address = "Main st. 2321", Phone = "1249123154", Email = "email@qwe13.com" }
    };
}