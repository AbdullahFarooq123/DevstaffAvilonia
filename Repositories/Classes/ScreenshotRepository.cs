using DataContext;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Classes;

public class ScreenshotRepository : GenericRepository<Screenshot>, IScreenshotRepository
{
	public ScreenshotRepository(DbContext context)
		: base(context ?? throw new ArgumentNullException(nameof(context))) { }
}
