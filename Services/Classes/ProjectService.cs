using DataContext;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes;

public class ProjectService : GenericService<Project>, IProjectService
{
	public ProjectService(IGenericRepository<Project> genericRepository)
		: base(genericRepository ?? throw new ArgumentNullException(nameof(genericRepository))) { }
	public IQueryable<Project> GetByUserId(int userId) => ((IProjectRepository)_repository).GetByUserId(userId);
}
