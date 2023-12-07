using DataContext;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Classes;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
	public ProjectRepository(DbContext context)
		: base(context ?? throw new ArgumentNullException(nameof(context))) { }
	public IQueryable<Project> GetByUserId(int userId)
		=> _dbSet
		.AsQueryable()
		.Where(
			project =>
				project.UserId == userId
			)
		.Include(
			project =>
				project.UserActivity
			);
}
