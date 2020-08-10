// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents a configurable dependency resolution system.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    /// <typeparam name="TServiceInjector">
    /// The type of the object that adds service descriptors to containers.
    /// </typeparam>
    public abstract class DependencyEngine<TConfigurator, TServiceInjector> : DependencyEngine<TConfigurator>
        where TConfigurator : class, new()
        where TServiceInjector : class, IServiceInjector<TConfigurator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyEngine{TConfigurator, TServiceInjector}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="package">
        /// A package that configures dependencies for the engine.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors to which the engine will supply dependencies.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="package" /> is
        /// <see langword="null" /> -or- <paramref name="serviceDescriptors" /> is <see langword="null" /> .
        /// </exception>
        protected DependencyEngine(IConfiguration applicationConfiguration, IDependencyPackage<TConfigurator> package, IServiceCollection serviceDescriptors)
            : base(applicationConfiguration, package)
        {
            LazyProviderFactory = new Lazy<ServiceProviderFactory>(CreateProviderFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            ServiceDescriptors = serviceDescriptors.RejectIf().IsNull(nameof(serviceDescriptors)).TargetArgument;
        }

        /// <summary>
        /// Initializes and configures all of the engine's components and enforces operational state.
        /// </summary>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create a new scope.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The engine is in a corrupt state.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The engine and/or its container are disposed.
        /// </exception>
        [DebuggerHidden]
        internal sealed override void Start()
        {
            try
            {
                base.Start();

                if (ProviderFactory is null)
                {
                    throw new InvalidOperationException("The dependency engine is in a corrupt state. The provider factory is a null reference.");
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("The dependency engine is in a corrupt state. See inner exception for details.", exception);
            }
        }

        /// <summary>
        /// Creates a new service provider.
        /// </summary>
        /// <returns>
        /// A new service provider.
        /// </returns>
        protected sealed override IServiceProvider CreateProvider() => ProviderFactory.CreateServiceProvider(Nix.Instance);

        /// <summary>
        /// Creates a new service injector.
        /// </summary>
        /// <returns>
        /// A new service injector.
        /// </returns>
        protected sealed override IServiceInjector<TConfigurator> CreateServiceInjector() => CreateServiceInjector(ServiceDescriptors);

        /// <summary>
        /// Creates a new service injector.
        /// </summary>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors that are injected to a configurator.
        /// </param>
        /// <returns>
        /// A new service injector.
        /// </returns>
        protected abstract TServiceInjector CreateServiceInjector(IServiceCollection serviceDescriptors);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DependencyEngine{TConfigurator, TServiceInjector}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Creates a new service provider factory.
        /// </summary>
        /// <returns>
        /// A new service provider factory.
        /// </returns>
        [DebuggerHidden]
        private ServiceProviderFactory CreateProviderFactory()
        {
            var providerFactory = new ServiceProviderFactory(Container);
            ServiceDescriptors.AddSingleton<IServiceProviderFactory<Nix>>(providerFactory);
            return providerFactory;
        }

        /// <summary>
        /// Gets the engine's service provider factory.
        /// </summary>
        protected ServiceProviderFactory ProviderFactory => LazyProviderFactory.Value;

        /// <summary>
        /// Represents a lazily-initialized service provider factory.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<ServiceProviderFactory> LazyProviderFactory;

        /// <summary>
        /// Represents a collection of service descriptors to which the engine will supply dependencies.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IServiceCollection ServiceDescriptors;
    }

    /// <summary>
    /// Represents a configurable dependency resolution system.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public abstract class DependencyEngine<TConfigurator> : DependencyEngine
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyEngine{TConfigurator}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="package">
        /// A package that configures dependencies for the engine.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="package" /> is
        /// <see langword="null" />.
        /// </exception>
        protected DependencyEngine(IConfiguration applicationConfiguration, IDependencyPackage<TConfigurator> package)
            : base()
        {
            ApplicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
            ConfigureContainerAction = (configuration, configurator) => ConfigureContainer(configuration, configurator);
            LazyServiceInjector = new Lazy<IServiceInjector<TConfigurator>>(CreateServiceInjector, LazyThreadSafetyMode.ExecutionAndPublication);
            Package = package.RejectIf().IsNull(nameof(package)).TargetArgument;
        }

        /// <summary>
        /// Initializes and configures all of the engine's components and enforces operational state.
        /// </summary>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create a new scope.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The engine is in a corrupt state.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The engine and/or its container are disposed.
        /// </exception>
        [DebuggerHidden]
        internal override void Start()
        {
            try
            {
                base.Start();

                if (ServiceInjector is null)
                {
                    throw new InvalidOperationException("The dependency engine is in a corrupt state. The service injector is a null reference.");
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("The dependency engine is in a corrupt state. See inner exception for details.", exception);
            }
        }

        /// <summary>
        /// Creates a new dependency container.
        /// </summary>
        /// <returns>
        /// A new dependency container.
        /// </returns>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while configuring the container.
        /// </exception>
        protected sealed override IDependencyContainer CreateContainer()
        {
            try
            {
                return CreateContainer(ApplicationConfiguration, ServiceInjector, ConfigureContainerAction);
            }
            catch (ContainerConfigurationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new ContainerConfigurationException(exception);
            }
        }

        /// <summary>
        /// Creates a new dependency container.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceInjector">
        /// An object that injects service descriptors into a container configurator.
        /// </param>
        /// <param name="configureAction">
        /// An action that configures the container.
        /// </param>
        /// <returns>
        /// A new dependency container.
        /// </returns>
        protected abstract IDependencyContainer CreateContainer(IConfiguration applicationConfiguration, IServiceInjector<TConfigurator> serviceInjector, Action<IConfiguration, TConfigurator> configureAction);

        /// <summary>
        /// Creates a new service injector.
        /// </summary>
        /// <returns>
        /// A new service injector.
        /// </returns>
        protected virtual IServiceInjector<TConfigurator> CreateServiceInjector() => new NullOperationServiceInjector<TConfigurator>();

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DependencyEngine{TConfigurator}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Configures a new container.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="configurator">
        /// An object that configures the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="configurator" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure the container.
        /// </exception>
        [DebuggerHidden]
        private void ConfigureContainer(IConfiguration applicationConfiguration, TConfigurator configurator)
        {
            var modules = Package.GetModules(applicationConfiguration);

            foreach (var module in modules)
            {
                module.Configure(configurator);
            }
        }

        /// <summary>
        /// Gets the engine's service injector.
        /// </summary>
        protected IServiceInjector<TConfigurator> ServiceInjector => LazyServiceInjector.Value;

        /// <summary>
        /// Represents configuration information for the application.
        /// </summary>
        protected readonly IConfiguration ApplicationConfiguration;

        /// <summary>
        /// Represents an action that configures a container.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action<IConfiguration, TConfigurator> ConfigureContainerAction;

        /// <summary>
        /// Represents a lazily-initialized service injector.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IServiceInjector<TConfigurator>> LazyServiceInjector;

        /// <summary>
        /// Represents a package that configures dependencies for the engine.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDependencyPackage<TConfigurator> Package;
    }

    /// <summary>
    /// Represents a configurable dependency resolution system.
    /// </summary>
    /// <remarks>
    /// <see cref="DependencyEngine" /> is the default implementation of <see cref="IDependencyEngine" />.
    /// </remarks>
    public abstract class DependencyEngine : Instrument, IDependencyEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyEngine" /> class.
        /// </summary>
        protected DependencyEngine()
            : base()
        {
            LazyContainer = new Lazy<IDependencyContainer>(CreateContainer, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyProvider = new Lazy<IServiceProvider>(CreateProvider, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Creates a new dependency engine.
        /// </summary>
        /// <typeparam name="TConfigurator">
        /// The type of the object that configures containers.
        /// </typeparam>
        /// <typeparam name="TEngine">
        /// The type of the dependency engine that is produced by the package.
        /// </typeparam>
        /// <typeparam name="TPackage">
        /// The type of the package that configures the engine.
        /// </typeparam>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// A new dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        public static TEngine New<TConfigurator, TEngine, TPackage>(IConfiguration applicationConfiguration)
            where TConfigurator : class, new()
            where TEngine : class, IDependencyEngine
            where TPackage : class, IDependencyPackage<TConfigurator, TEngine>, new() => New<TConfigurator, TEngine, TPackage>(applicationConfiguration, EmptyServiceCollection);

        /// <summary>
        /// Creates a new dependency engine.
        /// </summary>
        /// <typeparam name="TConfigurator">
        /// The type of the object that configures containers.
        /// </typeparam>
        /// <typeparam name="TEngine">
        /// The type of the dependency engine that is produced by the package.
        /// </typeparam>
        /// <typeparam name="TPackage">
        /// The type of the package that configures the engine.
        /// </typeparam>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors to which the engine will supply dependencies.
        /// </param>
        /// <returns>
        /// A new dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="serviceDescriptors" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        [DebuggerHidden]
        internal static TEngine New<TConfigurator, TEngine, TPackage>(IConfiguration applicationConfiguration, IServiceCollection serviceDescriptors)
            where TConfigurator : class, new()
            where TEngine : class, IDependencyEngine
            where TPackage : class, IDependencyPackage<TConfigurator, TEngine>, new()
        {
            var package = new TPackage();
            var engine = package.CreateEngine(applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument, serviceDescriptors);
            var abstractEngine = engine as DependencyEngine;

            if (abstractEngine is null == false)
            {
                abstractEngine.Start();
            }

            return engine;
        }

        /// <summary>
        /// Initializes and configures all of the engine's components and enforces operational state.
        /// </summary>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create a new scope.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The engine is in a corrupt state.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The engine and/or its container are disposed.
        /// </exception>
        [DebuggerHidden]
        internal virtual void Start()
        {
            try
            {
                if (Container is null)
                {
                    throw new InvalidOperationException("The dependency engine is in a corrupt state. The container is a null reference.");
                }

                using (var scope = Container.CreateScope())
                {
                    if (scope is null)
                    {
                        throw new InvalidOperationException("The dependency engine is in a corrupt state. The container produced a null reference scope.");
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("The dependency engine is in a corrupt state. See inner exception for details.", exception);
            }
        }

        /// <summary>
        /// Creates a new dependency container.
        /// </summary>
        /// <returns>
        /// A new dependency container.
        /// </returns>
        protected abstract IDependencyContainer CreateContainer();

        /// <summary>
        /// Creates a new service provider.
        /// </summary>
        /// <returns>
        /// A new service provider.
        /// </returns>
        protected virtual IServiceProvider CreateProvider() => new ServiceProvider(Container);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DependencyEngine" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    LazyContainer.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets the engine's dependency container.
        /// </summary>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while configuring the container.
        /// </exception>
        public IDependencyContainer Container => LazyContainer.Value;

        /// <summary>
        /// Gets the engine's service provider.
        /// </summary>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while configuring the container.
        /// </exception>
        public IServiceProvider Provider => LazyProvider.Value;

        /// <summary>
        /// Represents a connection string value that instructs dependency modules to register in-memory connections.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String InMemoryConnectionStringValue = "InMemory";

        /// <summary>
        /// Represents an empty service descriptor collection.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly IServiceCollection EmptyServiceCollection = new ServiceCollection();

        /// <summary>
        /// Represents the engine's lazily-initialized dependency container.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDependencyContainer> LazyContainer;

        /// <summary>
        /// Represents the engine's lazily-initialized service provider.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IServiceProvider> LazyProvider;
    }
}