using DataContext;
using Repositories;
using Services.Interfaces;

namespace Services.Classes;

public class ScreenshotService : GenericService<Screenshot>, IScreenshotService
{
    public ScreenshotService(IGenericRepository<Screenshot> genericRepository)
        : base(genericRepository: genericRepository ?? throw new ArgumentNullException(nameof(genericRepository)))
    {
    }
}