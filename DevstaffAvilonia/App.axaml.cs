using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DependencyInjection;
using DevstaffAvilonia.Helpers;
using DevstaffAvilonia.ViewModels;
using DevstaffAvilonia.Views;

namespace DevstaffAvilonia;

public partial class App : Application
{
	private readonly DIContainer _servicesContainer;
	public App() => _servicesContainer = new DIServiceCollection().RegisterServices();
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
