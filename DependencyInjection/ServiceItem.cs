using GlobalExtensionMethods;

namespace DependencyInjection;

public class ServiceItem
{
	#region Prop
	public Type _serviceType { get; private set; }
	public Type _implementationType { get; private set; }
	public object? _implementation { get; private set; }
	public ServiceLifetime _lifetime { get; private set; }
	#endregion Prop

	#region Ctor
	public ServiceItem(Type serviceType, Type implementationType, object? implementation, ServiceLifetime lifetime)
	{
		_serviceType = serviceType;
		_implementationType = implementationType;
		_implementation = implementation;
		_lifetime = lifetime;
	}
	#endregion Ctor

	#region Public Methods
	public void SetImplementation(object? implementation)
	{
		if (implementation.HasNoValue())
			throw new ArgumentNullException(nameof(implementation));
		if (implementation.Value().GetType() != _implementationType)
			throw new InvalidOperationException(
				$"Implementation type mismatch for obj : {nameof(implementation)}, type : {implementation.Value().GetType()}.\n" +
				$"The registered implementation type is : {_implementationType.Name}");
		_implementation = implementation;
	}
	public static bool operator ==(ServiceItem serviceItem, Type type) =>
		serviceItem.Equals(type);
	public static bool operator !=(ServiceItem serviceItem, Type type) =>
		serviceItem.Equals(type);
	public override bool Equals(object? type)
	{
		if (type.HasNoValue())
			return false;
		return _serviceType == (Type)type.Value() || _implementationType == (Type)type.Value();
	}
	public override int GetHashCode() => 0;
	#endregion Public Methods
}
