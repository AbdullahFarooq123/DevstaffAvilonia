using DataContext;
using Repositories;
using Services.Interfaces;

namespace Services.Classes;

public class UserActivityService : GenericService<UserActivity>, IUserActivityService
{
	public UserActivityService(IGenericRepository<UserActivity> genericRepository)
		: base(genericRepository ?? throw new ArgumentNullException(nameof(genericRepository))) { }
}
