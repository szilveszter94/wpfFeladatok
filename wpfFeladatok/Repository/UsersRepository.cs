using System.Collections.ObjectModel;
using wpfFeladatok.Models;

namespace wpfFeladatok.Repository;

public class UsersRepository : IUsersRepository
{
    public ObservableCollection<User> UserList { get; } = new ()
    {
        new User { Id =1, LastName = "Nagy", FirstName = "Kati"},
        new User{Id =2, LastName = "Kovács", FirstName = "Zoli"},
        new User{Id =3, LastName = "Kis", FirstName = "Pisti"}
    };
}