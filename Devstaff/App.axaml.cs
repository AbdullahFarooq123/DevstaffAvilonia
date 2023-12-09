using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DependencyInjection;
using DevStaff.Helpers;
using DevStaff.ViewModels;
using DevStaff.Views;

namespace DevStaff;

public partial class App : Application
{
    private readonly DiContainer _servicesContainer = new DiServiceCollection().RegisterServices();
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        _servicesContainer.EnsureDbMigration();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new Home()
            {
                DataContext = _servicesContainer.GetService<HomeViewModel>()
            };
        base.OnFrameworkInitializationCompleted();
    }
}