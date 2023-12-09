using DataContext;

namespace Repositories.Interfaces;

public interface IProjectRepository : IGenericRepository<Project>
{
    public IQueryable<Project> GetByUserId(int userId);
}