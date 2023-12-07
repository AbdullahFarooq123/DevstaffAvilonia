using DataContext;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Classes;

public class UserRepository : GenericRepository<User>, IUserRepository
{
	public UserRepository(DbContext context)
		: base(context ?? throw new ArgumentNullException(nameof(context))) { }
}
