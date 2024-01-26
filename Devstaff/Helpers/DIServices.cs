using BackgroundJobs.ActivityListeners.Classes;
using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Classes;
using BackgroundJobs.Services.Interfaces;
using DataContext;
using DependencyInjection;
using DevStaff.ViewModels;
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
using System.Runtime.InteropServices;
using DataModels;
using WinApi.Classes;
using WinApi.Interfaces;
using XApi.Classes;
using XApi.Interfaces;

namespace DevStaff.Helpers;

public static class DiServices
{
    #region Service Extension Methods

    public static DiContainer RegisterServices(this DiServiceCollection serviceCollection)
    {
        var configurationBuilder = GetAppSettings();
        var connectionString = GetConnectionString(config: configurationBuilder);
        serviceCollection.AddSingleton<IConfiguration>(implementation: configurationBuilder);
        serviceCollection.AddSingleton(implementation: new DbContextOptionsBuilder<DevStaffDbContext>()
            .UseSqlite(connectionString: connectionString).Options);
        serviceCollection.AddSingleton(implementation: configurationBuilder.GetSection(key: "WindowDimensions")
            .Get<AppDimensions>());
        serviceCollection.AddSingleton(implementation: configurationBuilder.GetSection(key: "AppSettings")
            .Get<AppSettings>());
        serviceCollection.AddSingleton<HomeViewModel>();

        serviceCollection.AddSingleton<DbContext, DevStaffDbContext>();

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

        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            serviceCollection.AddSingleton<IKeyboardListener, WinKeyboardListener>();
            serviceCollection.AddSingleton<IMouseListener, WinMouseListener>();
            serviceCollection.AddSingleton<IScreenshotListener, WinScreenshotListener>();
        }
        else if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            serviceCollection.AddSingleton<IKeyboardListener, XKeyboardListener>();
            serviceCollection.AddSingleton<IMouseListener, XMouseListener>();
            serviceCollection.AddSingleton<IScreenshotListener, XScreenshotListener>();
        }

        serviceCollection.AddSingleton<ICleanupService, CleanupService>();
        serviceCollection.AddSingleton<IBackgroundJobService, BackgroundJobService>();
        serviceCollection.AddSingleton<IDataContextService, DataContextServices>();

        serviceCollection.AddTransient<ITimerUtilities, TimerUtilities>();
        serviceCollection.AddTransient<IHookApi, HookApi>();
        serviceCollection.AddTransient<IInputDeviceApi, InputDeviceApi>();
        serviceCollection.AddSingleton<IScreenshotApi, ScreenshotApi>();
        serviceCollection.AddSingleton<IXScreenshotApi, XScreenshotApi>();

        return serviceCollection.GetContainer();
    }

    #endregion Service Extension Methods

    #region Private Methods

    private static IConfigurationRoot GetAppSettings()
    {
        var appSettingsStream = System.Reflection.Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(name: "Devstaff.appsettings.json");
        if (appSettingsStream.HasNoValue())
            throw new InvalidOperationException(message: "appsettings.json not found.");
        var configurationBuilder = new ConfigurationBuilder().AddJsonStream(stream: appSettingsStream.Value()).Build();
        return configurationBuilder;
    }

    private static string GetConnectionString(IConfiguration config)
    {
        var connectionString = config.GetSection(key: "Database:ConnectionString").Value;
        if (connectionString.HasNoValue())
            throw new InvalidOperationException(
                message: "Section 'Database:ConnectionString' not found in appsettings.json");
        var userHome = Environment.GetFolderPath(folder: Environment.SpecialFolder.UserProfile);
        connectionString = connectionString.Value().Replace(oldValue: "{path}", newValue: $"");
        return connectionString;
    }

    #endregion Private Methods
}