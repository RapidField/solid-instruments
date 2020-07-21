// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Service
{
    /// <summary>
    /// Prepares for and performs execution of a service.
    /// </summary>
    /// <remarks>
    /// <see cref="ServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> is the default implementation
    /// of <see cref="IServiceExecutor" />.
    /// </remarks>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    /// <typeparam name="TDependencyConfigurator">
    /// The type of the object that configures the dependency container.
    /// </typeparam>
    /// <typeparam name="TDependencyEngine">
    /// The type of the dependency engine that is produced by the dependency package.
    /// </typeparam>
    public abstract class ServiceExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine> : Instrument, IServiceExecutor
        where TDependencyPackage : class, IDependencyPackage<TDependencyConfigurator, TDependencyEngine>, new()
        where TDependencyConfigurator : class, new()
        where TDependencyEngine : class, IDependencyEngine
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="serviceName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceName" /> is <see langword="null" />.
        /// </exception>
        protected ServiceExecutor(String serviceName)
            : base()
        {
            ExecutionLifetime = null;
            LazyApplicationConfiguration = new Lazy<IConfiguration>(CreateApplicationConfiguration, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyDependencyEngine = new Lazy<IDependencyEngine>(CreateDependencyEngine, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyRootDependencyScope = new Lazy<IDependencyScope>(DependencyEngine.Container.CreateScope, LazyThreadSafetyMode.ExecutionAndPublication);
            ReferenceManager = new ReferenceManager();
            ServiceName = serviceName.Trim().RejectIf().IsNullOrEmpty(nameof(serviceName));
        }

        /// <summary>
        /// Begins execution of the service and performs the service operations.
        /// </summary>
        /// <exception cref="ServiceExectuionException">
        /// An exception was raised during execution of the service.
        /// </exception>
        public void Execute()
        {
            try
            {
                using (var executionLifetime = new ServiceExecutionLifetime())
                {
                    ExecutionLifetime = executionLifetime;

                    using (var dependencyScope = CreateDependencyScope())
                    {
                        Execute(dependencyScope, ApplicationConfiguration, executionLifetime);
                    }
                }
            }
            catch (ServiceExectuionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new ServiceExectuionException($"An exception was raised during execution of the service: \"{ServiceName}\". See inner exception.", exception);
            }
        }

        /// <summary>
        /// Converts the value of the current
        /// <see cref="ServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> to its equivalent string
        /// representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current
        /// <see cref="ServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(ServiceName)}\": \"{ServiceName}\" }}";

        /// <summary>
        /// Builds the application configuration for the service.
        /// </summary>
        /// <param name="configurationBuilder">
        /// An object that is used to build the configuration.
        /// </param>
        protected virtual void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            return;
        }

        /// <summary>
        /// Creates a new dependency scope.
        /// </summary>
        /// <returns>
        /// A new dependency scope.
        /// </returns>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create the scope.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected IDependencyScope CreateDependencyScope() => RootDependencyScope.CreateChildScope();

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="ServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
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
                    LazyRootDependencyScope.Dispose();
                    LazyDependencyEngine.Dispose();
                    ReferenceManager.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Begins execution of the service and performs the service operations.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        /// <param name="executionLifetime">
        /// An object that provides control over execution lifetime.
        /// </param>
        protected virtual void Execute(IDependencyScope dependencyScope, IConfiguration applicationConfiguration, IServiceExecutionLifetime executionLifetime)
        {
            return;
        }

        /// <summary>
        /// Creates configuration information for the service.
        /// </summary>
        /// <returns>
        /// Configuration information for the service.
        /// </returns>
        [DebuggerHidden]
        private IConfiguration CreateApplicationConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            BuildConfiguration(configurationBuilder);
            return configurationBuilder.Build();
        }

        /// <summary>
        /// Creates the dependency engine for the service.
        /// </summary>
        /// <returns>
        /// The dependency engine for the service.
        /// </returns>
        [DebuggerHidden]
        private IDependencyEngine CreateDependencyEngine()
        {
            var dependencyPackage = new TDependencyPackage();
            return dependencyPackage.CreateEngine(ApplicationConfiguration);
        }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public String ServiceName
        {
            get;
        }

        /// <summary>
        /// Gets the execution lifetime object for the current
        /// <see cref="ServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />, or
        /// <see langword="null" /> if execution has not yet started.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal IServiceExecutionLifetime ExecutionLifetime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets configuration information for the service.
        /// </summary>
        protected IConfiguration ApplicationConfiguration => LazyApplicationConfiguration.Value;

        /// <summary>
        /// Gets a utility that disposes of the object references that are managed by the current
        /// <see cref="ServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        protected IReferenceManager ReferenceManager
        {
            get;
        }

        /// <summary>
        /// Gets the dependency engine for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDependencyEngine DependencyEngine => LazyDependencyEngine.Value;

        /// <summary>
        /// Gets the root dependency scope for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDependencyScope RootDependencyScope => LazyRootDependencyScope.Value;

        /// <summary>
        /// Represents lazily-initialized configuration information for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IConfiguration> LazyApplicationConfiguration;

        /// <summary>
        /// Represents the lazily-initialized dependency engine for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDependencyEngine> LazyDependencyEngine;

        /// <summary>
        /// Represents the lazily-initialized root dependency scope for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDependencyScope> LazyRootDependencyScope;
    }
}