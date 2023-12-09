using DataContext;
using Repositories;
using Services.Interfaces;

namespace Services.Classes;

public class UserService : GenericService<User>, IUserService
{
    public UserService(IGenericRepository<User> genericRepository)
        : base(genericRepository ?? throw new ArgumentNullException(nameof(genericRepository)))
    {
    }
}