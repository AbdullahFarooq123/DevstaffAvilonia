using GlobalExtensionMethods;


namespace DependencyInjection;

public class ServiceItem
{
    #region Prop

    private Type ServiceType { get; }
    public Type ImplementationType { get; }
    public object? Implementation { get; private set; }
    public ServiceLifetime Lifetime { get; private set; }

    #endregion Prop

    #region Ctor

    public ServiceItem(Type serviceType, Type implementationType, object? implementation, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Implementation = implementation;
        Lifetime = lifetime;
    }

    #endregion Ctor

    #region Public Methods

    public void SetImplementation(object? implementation)
    {
        if (implementation.HasNoValue())
            throw new ArgumentNullException(nameof(implementation));
        if (implementation.Value().GetType() != ImplementationType)
            throw new InvalidOperationException(
                $"Implementation type mismatch for obj : {nameof(implementation)}, type : {implementation.Value().GetType()}.\n" +
                $"The registered implementation type is : {ImplementationType.Name}");
        Implementation = implementation;
    }

    public static bool operator ==(ServiceItem serviceItem, Type type) =>
        serviceItem.Equals(type);

    public static bool operator !=(ServiceItem serviceItem, Type type) =>
        serviceItem.Equals(type);

    public override bool Equals(object? type)
    {
        if (type.HasNoValue()) return false;
        return ServiceType == (Type)type.Value() || ImplementationType == (Type)type.Value();
    }

    public override int GetHashCode() => 0;

    #endregion Public Methods
}