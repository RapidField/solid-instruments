// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            CommandLineArguments = null;
            ExecutionLifetime = null;
            LazyApplicationConfiguration = new(CreateApplicationConfiguration, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyDependencyEngine = new(CreateDependencyEngine, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyRootDependencyScope = new(DependencyEngine.Container.CreateScope, LazyThreadSafetyMode.ExecutionAndPublication);
            ReferenceManager = new ReferenceManager();
            ServiceName = serviceName.RejectIf().IsNullOrEmpty(nameof(serviceName)).TargetArgument.Trim();
        }

        /// <summary>
        /// Begins execution of the service and performs the service operations.
        /// </summary>
        /// <exception cref="ServiceExectuionException">
        /// An exception was raised during execution of the service.
        /// </exception>
        public void Execute() => Execute(Array.Empty<String>());

        /// <summary>
        /// Begins execution of the service and performs the service operations.
        /// </summary>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        /// <exception cref="ServiceExectuionException">
        /// An exception was raised during execution of the service.
        /// </exception>
        public void Execute(String[] commandLineArguments)
        {
            CommandLineArguments = commandLineArguments ?? Array.Empty<String>();

            try
            {
                if (SupressStandardConsoleOutput is false)
                {
                    var productName = ProductName?.Trim();
                    var serviceName = ServiceName?.Trim();
                    var copyrightNotice = CopyrightNotice?.Trim();

                    if (productName.IsNullOrEmpty() is false)
                    {
                        Console.WriteLine(productName);
                    }

                    if (serviceName.IsNullOrEmpty() is false)
                    {
                        Console.WriteLine(serviceName);
                    }

                    if (copyrightNotice.IsNullOrEmpty() is false)
                    {
                        Console.WriteLine(copyrightNotice);
                    }

                    Console.WriteLine($"{Environment.NewLine}Service execution starting.");
                }

                try
                {
                    using (var executionLifetime = new ServiceExecutionLifetime())
                    {
                        ExecutionLifetime = executionLifetime;

                        using (var dependencyScope = CreateDependencyScope())
                        {
                            Execute(dependencyScope, ApplicationConfiguration, CommandLineArguments, executionLifetime);
                        }
                    }
                }
                catch (ServiceExectuionException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new ServiceExectuionException($"An exception was raised during execution of \"{ServiceName}\". See inner exception.", exception);
                }
            }
            catch (ServiceExectuionException exception)
            {
                if (SupressStandardConsoleOutput is false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{exception.Message} : {exception.StackTrace}{Environment.NewLine}");
                    Console.ResetColor();
                }

                throw;
            }
            finally
            {
                if (SupressStandardConsoleOutput is false)
                {
                    Console.WriteLine("Service execution finished.");
                }
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
        /// <remarks>
        /// The default implementation of the this method adds:
        /// - environment variables with prefixes matching any of <see cref="EnvironmentVariableConfigurationPrefixes" />,
        /// - all command line arguments supplied to <see cref="Execute(String[])" /> and
        /// - the appsettings.json file in the root application path.
        /// </remarks>
        /// <param name="configurationBuilder">
        /// An object that is used to build the configuration.
        /// </param>
        protected virtual void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            foreach (var prefix in EnvironmentVariableConfigurationPrefixes)
            {
                _ = configurationBuilder.AddEnvironmentVariables(prefix);
            }

            if (CommandLineArguments.IsNullOrEmpty() is false)
            {
                _ = configurationBuilder.AddCommandLine(CommandLineArguments);
            }

            _ = configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(DefaultAppSettingsJsonFileName, true, true);
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
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                LazyRootDependencyScope?.Dispose();
                LazyDependencyEngine?.Dispose();
                ReferenceManager?.Dispose();
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
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        /// <param name="executionLifetime">
        /// An object that provides control over execution lifetime.
        /// </param>
        protected virtual void Execute(IDependencyScope dependencyScope, IConfiguration applicationConfiguration, String[] commandLineArguments, IServiceExecutionLifetime executionLifetime)
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
        /// When overridden by a derived class, gets a copyright notice which is written to the console at the start of service
        /// execution.
        /// </summary>
        protected virtual String CopyrightNotice => null;

        /// <summary>
        /// When overridden by a derived class, gets a collection of textual prefixes which are used by
        /// <see cref="BuildConfiguration(IConfigurationBuilder)" /> to find and add environment variables to
        /// <see cref="ApplicationConfiguration" />.
        /// </summary>
        protected virtual IEnumerable<String> EnvironmentVariableConfigurationPrefixes => Array.Empty<String>();

        /// <summary>
        /// When overridden by a derived class, gets a product name associated with the service which is written to the console at
        /// the start of service execution.
        /// </summary>
        protected virtual String ProductName => null;

        /// <summary>
        /// Gets a utility that disposes of the object references that are managed by the current
        /// <see cref="ServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        protected IReferenceManager ReferenceManager
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not to suppress the console output that lists the product name, service name and
        /// copyright notice; as well as the startup, finalization and exception notifications.
        /// </summary>
        protected virtual Boolean SupressStandardConsoleOutput => false;

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
        /// Represents the command line arguments that were provided at runtime, if any.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal String[] CommandLineArguments;

        /// <summary>
        /// Represents the default JSON settings file name which is used by <see cref="BuildConfiguration(IConfigurationBuilder)" />
        /// to add file-based configuration to <see cref="ApplicationConfiguration" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultAppSettingsJsonFileName = "appsettings.json";

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