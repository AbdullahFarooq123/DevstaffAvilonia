using DataContext;

namespace Services.Interfaces;

public interface IProjectService : IGenericService<Project>
{
	public IQueryable<Project> GetByUserId(int userId);
}
