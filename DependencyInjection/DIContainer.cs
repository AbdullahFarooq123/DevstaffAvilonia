using GlobalExtensionMethods;

namespace DependencyInjection;

public class DiContainer
{
    private readonly IEnumerable<ServiceItem> _services;

    public DiContainer(IEnumerable<ServiceItem> services) =>
        _services = services;

    #region Public Methods

    private object GetService(Type serviceType)
    {
        var serviceItem = _services
            .FirstOrDefault(item => item == serviceType);

        if (serviceItem.HasNoValue())
            throw new InvalidOperationException($"Service with type {serviceType} not found.");

        if (serviceItem.Value().Implementation.HasNoValue() ||
            serviceItem.Value().Lifetime is ServiceLifetime.Transient)
            InstantiateCtor(serviceItem.Value());

        return serviceItem.Value().Implementation.Value();
    }

    public TServiceType GetService<TServiceType>() => (TServiceType)GetService(typeof(TServiceType));

    #endregion Public Methods

    #region Private Methods

    private void InstantiateCtor(ServiceItem serviceItem)
    {
        var type = serviceItem.ImplementationType;
        if (type.IsNotInstantiable())
            throw new InvalidOperationException(
                "The service you are trying to initialize is either abstract or interface");
        var ctorInfo = type
            .GetConstructors()
            .First();
        var cParams = ctorInfo
            .GetParameters()
            .Select(
                x =>
                    GetService(x.ParameterType) ??
                    throw new InvalidOperationException(
                        $"One of the params {x.Name} for {type.Name} doesn't exist in the container.")
            )
            .ToArray();
        var implementation = Activator.CreateInstance(serviceItem.ImplementationType, cParams);
        serviceItem.SetImplementation(implementation);
    }

    #endregion Private Methods
}