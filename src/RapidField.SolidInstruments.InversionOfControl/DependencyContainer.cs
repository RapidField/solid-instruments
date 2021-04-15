// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents an abstraction for a utility that creates, destroys, and manages scoping for dependencies.
    /// </summary>
    /// <remarks>
    /// <see cref="DependencyContainer{TContainer, TConfigurator}" /> is the default implementation of
    /// <see cref="IDependencyContainer" />.
    /// </remarks>
    /// <typeparam name="TContainer">
    /// The type of the container that is abstracted by the <see cref="DependencyContainer{TContainer, TConfigurator}" />.
    /// </typeparam>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures the container.
    /// </typeparam>
    public abstract class DependencyContainer<TContainer, TConfigurator> : ConfigurableInstrument<DependencyContainerConfiguration<TConfigurator>>, IDependencyContainer
        where TContainer : IDisposable
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainer{TContainer, TConfigurator}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="configureAction">
        /// An action that configures the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="configureAction" /> is
        /// <see langword="null" />.
        /// </exception>
        protected DependencyContainer(IConfiguration applicationConfiguration, Action<IConfiguration, TConfigurator> configureAction)
            : this(applicationConfiguration, new NullOperationServiceInjector<TConfigurator>(), configureAction)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainer{TContainer, TConfigurator}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceInjector">
        /// An object that adds service descriptors to the container.
        /// </param>
        /// <param name="configureAction">
        /// An action that configures the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="serviceInjector" /> is
        /// <see langword="null" /> -or- <paramref name="configureAction" /> is <see langword="null" />.
        /// </exception>
        protected DependencyContainer(IConfiguration applicationConfiguration, IServiceInjector<TConfigurator> serviceInjector, Action<IConfiguration, TConfigurator> configureAction)
            : base(applicationConfiguration)
        {
            ConfigureAction = configureAction.RejectIf().IsNull(nameof(configureAction));
            LazyReferenceManager = new(() => new ReferenceManager(), LazyThreadSafetyMode.ExecutionAndPublication);
            LazyRootScope = new(CreateRootScope, LazyThreadSafetyMode.ExecutionAndPublication);
            LazySourceContainer = new(BuildSourceContainer, LazyThreadSafetyMode.ExecutionAndPublication);
            ServiceInjector = serviceInjector.RejectIf().IsNull(nameof(serviceInjector)).TargetArgument;
        }

        /// <summary>
        /// Creates a new initialization and disposal scope for the current
        /// <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        /// <returns>
        /// A new initialization and disposal scope for the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </returns>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create the scope.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IDependencyScope CreateScope() => WithStateControl(() =>
        {
            try
            {
                var scope = RootScope.CreateChildScope();
                ReferenceManager.AddObject(scope);
                return scope;
            }
            catch (CreateDependencyScopeException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CreateDependencyScopeException(exception);
            }
        });

        /// <summary>
        /// Requests an object of specified type from the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        /// <param name="type">
        /// The type of the object to resolve.
        /// </param>
        /// <returns>
        /// An object of specified type from the associated container.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="DependencyResolutionException">
        /// An exception was raised while attempting to resolve the dependency.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Object Resolve(Type type)
        {
            RejectIfDisposed();
            return RootScope.Resolve(type);
        }

        /// <summary>
        /// Builds the container that is abstracted by the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        /// <returns>
        /// The container.
        /// </returns>
        protected abstract TContainer BuildSourceContainer(TConfigurator configurator);

        /// <summary>
        /// Configures the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </param>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure the container.
        /// </exception>
        protected sealed override void Configure(DependencyContainerConfiguration<TConfigurator> configuration)
        {
            try
            {
                RegisterMediator(configuration.Configurator);
                ConfigureAction(configuration.Application, configuration.Configurator);
                RegisterFallbackTypes(configuration.Configurator);
            }
            catch (ContainerConfigurationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new ContainerConfigurationException(exception);
            }
            finally
            {
                base.Configure(configuration);
            }
        }

        /// <summary>
        /// Creates a new initialization and disposal scope for the current
        /// <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        /// <param name="sourceContainer">
        /// The container.
        /// </param>
        /// <returns>
        /// A new initialization and disposal scope for the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </returns>
        protected abstract IDependencyScope CreateScope(TContainer sourceContainer);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                try
                {
                    LazySourceContainer?.Dispose();
                    LazyRootScope?.Dispose();
                }
                finally
                {
                    LazyReferenceManager?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Conditionally registers important dependency types if they are missing following user-defined registrations.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        protected abstract void RegisterFallbackTypes(TConfigurator configurator);

        /// <summary>
        /// Registers a command mediator with the configurator.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        protected abstract void RegisterMediator(TConfigurator configurator);

        /// <summary>
        /// Builds the container that is abstracted by the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        /// <returns>
        /// The container.
        /// </returns>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while building the container.
        /// </exception>
        [DebuggerHidden]
        private TContainer BuildSourceContainer()
        {
            try
            {
                var configurator = Configuration.Configurator;
                ServiceInjector.Inject(configurator);
                return BuildSourceContainer(configurator);
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
        /// Creates the root scope for the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        /// <returns>
        /// The root scope for the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </returns>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while building the container.
        /// </exception>
        [DebuggerHidden]
        private IDependencyScope CreateRootScope() => CreateScope(SourceContainer);

        /// <summary>
        /// Gets a utility that disposes of the object references that are managed by the current
        /// <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal IReferenceManager ReferenceManager => LazyReferenceManager.Value;

        /// <summary>
        /// Gets the root scope for the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDependencyScope RootScope => LazyRootScope.Value;

        /// <summary>
        /// Gets the container that is abstracted by the current <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TContainer SourceContainer => LazySourceContainer.Value;

        /// <summary>
        /// Represents an action that configures the container.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action<IConfiguration, TConfigurator> ConfigureAction;

        /// <summary>
        /// Represents the lazily-initialized utility that disposes of the object references that are managed by the current
        /// <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IReferenceManager> LazyReferenceManager;

        /// <summary>
        /// Represents the lazily-initialized root scope for the current
        /// <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDependencyScope> LazyRootScope;

        /// <summary>
        /// Represents the lazily-initialized container that is abstracted by the current
        /// <see cref="DependencyContainer{TContainer, TConfigurator}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<TContainer> LazySourceContainer;

        /// <summary>
        /// Represents an object that adds service descriptors to the container.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IServiceInjector<TConfigurator> ServiceInjector;
    }
}