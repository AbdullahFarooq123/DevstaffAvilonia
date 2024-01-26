namespace DependencyInjection;

public class DiServiceCollection
{
    #region Properties

    private readonly IList<ServiceItem> _items = new List<ServiceItem>();

    #endregion Properties

    #region Service Registration Methods

    public void AddSingleton<TService>(TService implementation) =>
        RegisterService(
            serviceType: typeof(TService),
            implementationType: typeof(TService),
            implementation: implementation,
            lifetime: ServiceLifetime.Singleton);

    public void AddTransient<TService>(TService implementation) =>
        RegisterService(
            serviceType: typeof(TService),
            implementationType: typeof(TService),
            implementation: implementation,
            lifetime: ServiceLifetime.Transient);

    public void AddSingleton<TService>() =>
        RegisterService<TService>(
            serviceType: typeof(TService),
            implementationType: typeof(TService),
            lifetime: ServiceLifetime.Singleton);

    public void AddTransient<TService>() =>
        RegisterService<TService>(
            serviceType: typeof(TService),
            implementationType: typeof(TService),
            lifetime: ServiceLifetime.Transient);

    public void AddSingleton<TService1, TService2>() where TService2 : TService1 =>
        RegisterService<TService2>(
            serviceType: typeof(TService1),
            implementationType: typeof(TService2),
            lifetime: ServiceLifetime.Singleton);

    public void AddTransient<TService1, TService2>() where TService2 : TService1 =>
        RegisterService<TService2>(
            serviceType: typeof(TService1),
            implementationType: typeof(TService2),
            lifetime: ServiceLifetime.Transient);

    public void AddSingleton<TService1, TService2>(TService2 implementation) where TService2 : TService1 =>
        RegisterService(
            serviceType: typeof(TService1),
            implementationType: typeof(TService2),
            implementation: implementation,
            lifetime: ServiceLifetime.Singleton);

    public void AddTransient<TService1, TService2>(TService2 implementation) where TService2 : TService1 =>
        RegisterService(
            serviceType: typeof(TService1),
            implementationType: typeof(TService2),
            implementation: implementation,
            lifetime: ServiceLifetime.Transient);

    #endregion Service Registration Methods

    public DiContainer GetContainer() => new(_items);

    #region Private Methods

    private void RegisterService<TService>(
        Type serviceType,
        Type implementationType,
        ServiceLifetime lifetime,
        TService? implementation = default) => _items.Add(
        new ServiceItem(
            implementationType: implementationType,
            serviceType: serviceType,
            implementation: implementation,
            lifetime: lifetime
        ));

    #endregion Private Methods
}