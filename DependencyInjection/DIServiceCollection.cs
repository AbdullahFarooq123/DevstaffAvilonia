namespace DependencyInjection;

public class DiServiceCollection
{
    #region Properties

    private readonly IList<ServiceItem> _items = new List<ServiceItem>();

    #endregion Properties

    #region Service Registration Methods

    public void AddSingleton<TService>(TService implementation) => RegisterService(
        serviceType: typeof(TService),
        implementationType: typeof(TService),
        implementation: implementation,
        lifetime: ServiceLifetime.Singleton);

    public void AddTransient<TService>(TService implementation) => RegisterService(
        serviceType: typeof(TService),
        implementationType: typeof(TService),
        implementation: implementation,
        lifetime: ServiceLifetime.Transient);

    public void AddSingleton<TService>() => RegisterService<TService>(
        serviceType: typeof(TService),
        implementationType: typeof(TService),
        lifetime: ServiceLifetime.Singleton);

    public void AddTransient<TService>() => RegisterService<TService>(
        serviceType: typeof(TService),
        implementationType: typeof(TService),
        lifetime: ServiceLifetime.Transient);

    public void AddSingleton<IService, TService>()
        where TService : IService => RegisterService<TService>(
        serviceType: typeof(IService),
        implementationType: typeof(TService),
        lifetime: ServiceLifetime.Singleton);

    public void AddTransient<IService, TService>()
        where TService : IService => RegisterService<TService>(
        serviceType: typeof(IService),
        implementationType: typeof(TService),
        lifetime: ServiceLifetime.Transient);

    public void AddSingleton<IService, TService>(TService implementation)
        where TService : IService => RegisterService(
        serviceType: typeof(IService),
        implementationType: typeof(TService),
        implementation: implementation,
        lifetime: ServiceLifetime.Singleton);

    public void AddTransient<IService, TService>(TService implementation)
        where TService : IService => RegisterService(
        serviceType: typeof(IService),
        implementationType: typeof(TService),
        implementation: implementation,
        lifetime: ServiceLifetime.Transient);

    #endregion Service Registration Methods

    public DiContainer GetContainer() => new DiContainer(_items);

    #region Private Methods

    private void RegisterService<TService>(
        Type serviceType,
        Type implementationType,
        ServiceLifetime lifetime,
        TService? implementation = default) => _items
        .Add(
            new ServiceItem(
                implementationType: implementationType,
                serviceType: serviceType,
                implementation: implementation,
                lifetime: lifetime
            ));

    #endregion Private Methods
}