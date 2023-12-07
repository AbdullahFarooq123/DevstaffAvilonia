using GlobalExtensionMethods;

namespace DependencyInjection;

public class DIContainer
{
	private readonly IEnumerable<ServiceItem> _services;

	public DIContainer(IEnumerable<ServiceItem> services) =>
		_services = services;

	#region Public Methods
	public object GetService(Type serviceType)
	{
		var serviceItem = _services
			.Where(item => item == serviceType)
			.FirstOrDefault();

		if (serviceItem.HasNoValue())
			throw new InvalidOperationException($"Service with type {serviceType} not found.");

		if (serviceItem.Value()._implementation.HasNoValue() || serviceItem.Value()._lifetime is ServiceLifetime.Transient)
			InstantiateCtor(serviceItem.Value());

		return serviceItem.Value()._implementation.Value();
	}
	public ServiceType GetService<ServiceType>() => (ServiceType)GetService(typeof(ServiceType));
	#endregion Public Methods

	#region Private Methods
	private void InstantiateCtor(ServiceItem serviceItem)
	{
		var type = serviceItem._implementationType;
		if (type.IsNotInstantiable())
			throw new InvalidOperationException("The service you are trying to initialize is either abstract or interface");
		var ctorInfo = type
			.GetConstructors()
			.First();
		var cparams = ctorInfo
			.GetParameters()
			.Select(
				x =>
					GetService(x.ParameterType) ??
					throw new InvalidOperationException($"One of the params {x.Name} for {type.Name} doesn't exist in the container.")
			)
			.ToArray();
		var implementation = Activator.CreateInstance(serviceItem._implementationType, cparams);
		serviceItem.SetImplementation(implementation);
	}
	#endregion Private Methods
}