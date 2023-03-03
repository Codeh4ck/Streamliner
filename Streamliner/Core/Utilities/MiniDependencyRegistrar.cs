using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Streamliner.Core.Utilities;

public sealed class DependencyRegistrar : DependencyRegistrar.IScope
{
    public interface IScope : IDisposable, IServiceProvider { }

    public interface IRegisteredType
    {
        void AsSingleton();
        void PerScope();
    }
        
    private readonly Dictionary<Type, Func<ILifetime, object>> _registeredTypes = new();

    private readonly ContainerLifetime _lifetime;

    public DependencyRegistrar() => _lifetime = new(t => _registeredTypes[t]);

    public IRegisteredType Register(Type @interface, Func<object> factory)
        => RegisterType(@interface, _ => factory());

    public IRegisteredType Register(Type @interface, Type implementation)
        => RegisterType(@interface, FactoryFromType(implementation));

    private IRegisteredType RegisterType(Type itemType, Func<ILifetime, object> factory)
        => new RegisteredType(itemType, f => _registeredTypes[itemType] = f, factory);

    public object GetService(Type type) => 
        !_registeredTypes.TryGetValue(type, out var registeredType) ? null : registeredType(_lifetime);

    public IScope CreateScope() => new ScopeLifetime(_lifetime);

    public void Dispose() => _lifetime.Dispose();

    interface ILifetime : IScope
    {
        object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory);

        object GetServicePerScope(Type type, Func<ILifetime, object> factory);
    }

    abstract class ObjectCache
    {
        private readonly ConcurrentDictionary<Type, object> _instanceCache = new();

        protected object GetCached(Type type, Func<ILifetime, object> factory, ILifetime lifetime)
            => _instanceCache.GetOrAdd(type, _ => factory(lifetime));

        public void Dispose()
        {
            foreach (var obj in _instanceCache.Values)
                (obj as IDisposable)?.Dispose();
        }
    }

    class ContainerLifetime : ObjectCache, ILifetime
    {
        public Func<Type, Func<ILifetime, object>> GetFactory { get; private set; }
        public ContainerLifetime(Func<Type, Func<ILifetime, object>> getFactory) => GetFactory = getFactory;
        public object GetService(Type type) => GetFactory(type)(this);
        public object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory)
            => GetCached(type, factory, this);
        public object GetServicePerScope(Type type, Func<ILifetime, object> factory)
            => GetServiceAsSingleton(type, factory);
    }

    class ScopeLifetime : ObjectCache, ILifetime
    {
        private readonly ContainerLifetime _parentLifetime;
        public ScopeLifetime(ContainerLifetime parentContainer) => _parentLifetime = parentContainer;
        public object GetService(Type type) => _parentLifetime.GetFactory(type)(this);
        public object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory) 
            => _parentLifetime.GetServiceAsSingleton(type, factory);
        public object GetServicePerScope(Type type, Func<ILifetime, object> factory) => GetCached(type, factory, this);
    }

    // Compiles a lambda that calls the given type's first constructor resolving arguments
    private static Func<ILifetime, object> FactoryFromType(Type itemType)
    {
        ConstructorInfo[] constructors = itemType.GetConstructors();

        if (constructors.Length == 0)
            constructors = itemType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            
        ConstructorInfo constructor = constructors.First();

        ParameterExpression arg = Expression.Parameter(typeof(ILifetime));

        return (Func<ILifetime, object>)Expression.Lambda(
            Expression.New(constructor, constructor.GetParameters().Select(
                param =>
                {
                    Func<ILifetime, object> resolve = lifetime => lifetime.GetService(param.ParameterType);

                    return Expression.Convert(
                        Expression.Call(Expression.Constant(resolve.Target), resolve.Method, arg),
                        param.ParameterType);
                })),
            arg).Compile();
    }

    class RegisteredType : IRegisteredType
    {
        private readonly Type _itemType;
        private readonly Action<Func<ILifetime, object>> _registerFactory;
        private readonly Func<ILifetime, object> _factory;

        public RegisteredType(Type itemType, Action<Func<ILifetime, object>> registerFactory, Func<ILifetime, object> factory)
        {
            _itemType = itemType;
            _registerFactory = registerFactory;
            _factory = factory;

            registerFactory(_factory);
        }

        public void AsSingleton() => _registerFactory(lifetime => lifetime.GetServiceAsSingleton(_itemType, _factory));
        public void PerScope() => _registerFactory(lifetime => lifetime.GetServicePerScope(_itemType, _factory));
    }
}

public static class ContainerExtensions
{
    public static DependencyRegistrar.IRegisteredType Register<T>(this DependencyRegistrar dependencyRegistrar, Type type)
        => dependencyRegistrar.Register(typeof(T), type);

    public static DependencyRegistrar.IRegisteredType Register<TInterface, TImplementation>(this DependencyRegistrar dependencyRegistrar)
        where TImplementation : TInterface
        => dependencyRegistrar.Register(typeof(TInterface), typeof(TImplementation));

    public static DependencyRegistrar.IRegisteredType Register<T>(this DependencyRegistrar dependencyRegistrar, Func<T> factory)
        => dependencyRegistrar.Register(typeof(T), () => factory());

    public static DependencyRegistrar.IRegisteredType Register<T>(this DependencyRegistrar dependencyRegistrar)
        => dependencyRegistrar.Register(typeof(T), typeof(T));

    public static T Resolve<T>(this DependencyRegistrar.IScope scope) => (T)scope.GetService(typeof(T));

    public static object Resolve(this DependencyRegistrar.IScope scope, Type type) => scope.GetService(type);
}