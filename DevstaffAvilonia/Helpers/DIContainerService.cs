using DataContext;
using DependencyInjection;
using GlobalExtensionMethods;
using Microsoft.EntityFrameworkCore;
using System;

namespace DevstaffAvilonia.Helpers;

public static class DIContainerService
{
	public static async void EnsureDbMigration(this DIContainer container)
	{
		var dbContext = (DevstaffDbContext)container.GetService<DbContext>();
		if (dbContext.HasNoValue())
			throw new InvalidOperationException($"Service : {nameof(DbContext)} not found");
		await dbContext.Value().Database.EnsureCreatedAsync();
	}
}
