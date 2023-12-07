using DataContext;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Classes;

public class UserActivityRepository : GenericRepository<UserActivity>, IUserActivityRepository
{
	public UserActivityRepository(DbContext context) 
		: base(context ?? throw new ArgumentNullException(nameof(context))) { }
}
