using System.Collections.ObjectModel;
using wpfFeladatok.Models;

namespace wpfFeladatok.Repository;

public interface IUsersRepository
{
    ObservableCollection<User> UserList { get; }
}