using DataContext;
using Repositories;
using Services.Interfaces;

namespace Services.Classes;

public class IntervalEntryService : GenericService<IntervalEntry>, IIntervalEntryService
{
    public IntervalEntryService(IGenericRepository<IntervalEntry> genericRepository)
        : base(genericRepository: genericRepository ?? throw new ArgumentNullException(nameof(genericRepository)))
    {
    }
}