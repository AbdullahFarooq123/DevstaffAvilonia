using BackgroundJobs.ActivityListeners.Classes;
using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Classes;
using BackgroundJobs.Services.Interfaces;
using DataContext;
using DataModel;
using DependencyInjection;
using DevstaffAvilonia.ViewModels;
using GlobalExtensionMethods;
using HelperServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories;
using Repositories.Classes;
using Repositories.Interfaces;
using Services.Classes;
using Services.Interfaces;
using System;
using WinApi.Classes;
using WinApi.Interfaces;

namespace DevstaffAvilonia.Helpers;

public static class DIServices
{
	#region Service Extension Methods
	public static DIContainer RegisterServices(this DIServiceCollection serviceCollection)
	{
		var _configurationBuilder = GetAppSettings();
		var _connectionString = GetConnectionString(_configurationBuilder);

		serviceCollection.AddSingleton<IConfiguration>(_configurationBuilder);
		serviceCollection.AddSingleton(new DbContextOptionsBuilder<DevstaffDbContext>().UseSqlite(_connectionString).Options);
		serviceCollection.AddSingleton(_configurationBuilder.GetSection("WindowDimentions").Get<AppDimentions>());
		serviceCollection.AddSingleton(_configurationBuilder.GetSection("AppSettings").Get<AppSettings>());
		serviceCollection.AddSingleton<HomeViewModel>();

		serviceCollection.AddSingleton<DbContext, DevstaffDbContext>();

		serviceCollection.AddSingleton<IIntervalEntryRepository, IntervalEntryRepository>();
		serviceCollection.AddSingleton<IProjectRepository, ProjectRepository>();
		serviceCollection.AddSingleton<IScreenshotRepository, ScreenshotRepository>();
		serviceCollection.AddSingleton<IUserActivityRepository, UserActivityRepository>();
		serviceCollection.AddSingleton<IUserRepository, UserRepository>();

		serviceCollection.AddSingleton<IGenericRepository<User>, UserRepository>();
		serviceCollection.AddSingleton<IGenericRepository<UserActivity>, UserActivityRepository>();
		serviceCollection.AddSingleton<IGenericRepository<Screenshot>, ScreenshotRepository>();
		serviceCollection.AddSingleton<IGenericRepository<Project>, ProjectRepository>();
		serviceCollection.AddSingleton<IGenericRepository<IntervalEntry>, IntervalEntryRepository>();

		serviceCollection.AddSingleton<IIntervalEntryService, IntervalEntryService>();
		serviceCollection.AddSingleton<IProjectService, ProjectService>();
		serviceCollection.AddSingleton<IScreenshotService, ScreenshotService>();
		serviceCollection.AddSingleton<IUserActivityService, UserActivityService>();
		serviceCollection.AddSingleton<IUserService, UserService>();

		serviceCollection.AddSingleton<IKeyboardListener, WinKeyboardListener>();
		serviceCollection.AddSingleton<IMouseListener, WinMouseListener>();
		serviceCollection.AddSingleton<IScreenshotListener, WinScreenshotListener>();
		serviceCollection.AddSingleton<IScreenshotApi, ScreenshotApi>();

		serviceCollection.AddSingleton<ICleanupService, CleaupService>();
		serviceCollection.AddSingleton<IBackgroundJobService, BackgroundJobService>();
		serviceCollection.AddSingleton<IDataContextService, DataContextServices>();

		serviceCollection.AddTransient<ITimerUtilities, TimerUtilities>();
		serviceCollection.AddTransient<IHookApi, HookApi>();

		return serviceCollection.GetContainer();
	}
	#endregion Service Extension Methods

	#region Private Methods
	private static IConfigurationRoot GetAppSettings()
	{
		var appSettingsStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("DevstaffAvilonia.appsettings.json");
		if (appSettingsStream.HasNoValue())
			throw new InvalidOperationException("appsettings.json not found.");
		var configurationBuilder = new ConfigurationBuilder().AddJsonStream(appSettingsStream.Value()).Build();
		return configurationBuilder;
	}
	private static string GetConnectionString(IConfiguration _config)
	{
		var connectionString = _config.GetSection("Database:ConnectionString").Value;
		if (connectionString.HasNoValue())
			throw new InvalidOperationException("Section 'Database:ConnectionString' not found in appsettings.json");
		var userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		connectionString = connectionString.Value().Replace("{path}", $"{userHome}\\");
		return connectionString;
	}
	#endregion Private Methods
}
