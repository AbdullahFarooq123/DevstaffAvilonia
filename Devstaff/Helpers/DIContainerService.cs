using System;
using DataContext;
using DependencyInjection;
using GlobalExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace DevStaff.Helpers;

public static class DiContainerService
{
    public static async void EnsureDbMigration(this DiContainer container)
    {
        var dbContext = (DevStaffDbContext)container.GetService<DbContext>();
        if (dbContext.HasNoValue())
            throw new InvalidOperationException($"Service : {nameof(DbContext)} not found");
        await dbContext.Value().Database.EnsureCreatedAsync();
    }
}