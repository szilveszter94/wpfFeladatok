using System.Collections.ObjectModel;
using wpfFeladatok.Models;

namespace wpfFeladatok.Repository;

public interface IUserDetailsRepository
{
    ObservableCollection<UserDetails> UserDetailsList { get; }
}