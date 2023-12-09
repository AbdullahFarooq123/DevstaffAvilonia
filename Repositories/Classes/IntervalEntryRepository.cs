using DataContext;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Classes;

public class IntervalEntryRepository : GenericRepository<IntervalEntry>, IIntervalEntryRepository
{
    public IntervalEntryRepository(DbContext context)
        : base(context ?? throw new ArgumentNullException(nameof(context)))
    {
    }
}