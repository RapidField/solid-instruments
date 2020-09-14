// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WebHostInitializer = Microsoft.AspNetCore.WebHost;

namespace RapidField.SolidInstruments.Web
{
    /// <summary>
    /// Prepares for and performs execution of a web application.
    /// </summary>
    /// <remarks>
    /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> is the default implementation of
    /// <see cref="IWebExecutor" />.
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
    public abstract class WebExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine> : Instrument, IWebExecutor
        where TDependencyPackage : class, IDependencyPackage<TDependencyConfigurator, TDependencyEngine>, new()
        where TDependencyConfigurator : class, new()
        where TDependencyEngine : class, IDependencyEngine
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the web application.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationName" /> is <see langword="null" />.
        /// </exception>
        protected WebExecutor(String applicationName)
            : base()
        {
            ApplicationName = applicationName.Trim().RejectIf().IsNullOrEmpty(nameof(applicationName));
            CommandLineArguments = null;
            LazyApplicationConfiguration = new Lazy<IConfiguration>(CreateApplicationConfiguration, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyDependencyEngine = new Lazy<IDependencyEngine>(CreateDependencyEngine, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyRootDependencyScope = new Lazy<IDependencyScope>(DependencyEngine.Container.CreateScope, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyWebHost = new Lazy<IWebHost>(CreateWebHost, LazyThreadSafetyMode.ExecutionAndPublication);
            ReferenceManager = new ReferenceManager();
        }

        /// <summary>
        /// Begins execution of the web application.
        /// </summary>
        /// <exception cref="WebExectuionException">
        /// An exception was raised during execution of the web application.
        /// </exception>
        public void Execute() => Execute(Array.Empty<String>());

        /// <summary>
        /// Begins execution of the web application.
        /// </summary>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        /// <exception cref="WebExectuionException">
        /// An exception was raised during execution of the web application.
        /// </exception>
        public void Execute(String[] commandLineArguments)
        {
            CommandLineArguments = commandLineArguments ?? Array.Empty<String>();

            try
            {
                var productName = ProductName?.Trim();
                var applicationName = ApplicationName?.Trim();
                var copyrightNotice = CopyrightNotice?.Trim();

                if (productName.IsNullOrEmpty() == false)
                {
                    Console.WriteLine(productName);
                }

                if (applicationName.IsNullOrEmpty() == false)
                {
                    Console.WriteLine(applicationName);
                }

                if (copyrightNotice.IsNullOrEmpty() == false)
                {
                    Console.WriteLine(copyrightNotice);
                }

                Console.WriteLine($"{Environment.NewLine}Web application execution starting.");

                try
                {
                    var webHost = WebHost; // Do not move this line.

                    using (var dependencyScope = CreateDependencyScope())
                    {
                        Execute(webHost, dependencyScope, ApplicationConfiguration, CommandLineArguments);
                    }
                }
                catch (WebExectuionException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new WebExectuionException($"An exception was raised during execution of the web application: \"{ApplicationName}\". See inner exception.", exception);
                }
            }
            catch (WebExectuionException exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{exception.Message} : {exception.StackTrace}{Environment.NewLine}");
                Console.ResetColor();
                throw;
            }
            finally
            {
                Console.WriteLine("Web application execution finished.");
            }
        }

        /// <summary>
        /// Converts the value of the current
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> to its equivalent string
        /// representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(ApplicationName)}\": \"{ApplicationName}\" }}";

        /// <summary>
        /// Builds the application configuration for the web application.
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

            if (CommandLineArguments.IsNullOrEmpty() == false)
            {
                _ = configurationBuilder.AddCommandLine(CommandLineArguments);
            }

            _ = configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(DefaultAppSettingsJsonFileName);
        }

        /// <summary>
        /// Configures the application's request pipeline.
        /// </summary>
        /// <param name="application">
        /// An object that configures the application's request pipeline.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        protected virtual void ConfigureApplication(IApplicationBuilder application, IConfiguration applicationConfiguration, String[] commandLineArguments)
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
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
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
        /// Begins execution of the web application.
        /// </summary>
        /// <param name="webHost">
        /// A configured web host.
        /// </param>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve web application dependencies.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        protected virtual void Execute(IWebHost webHost, IDependencyScope dependencyScope, IConfiguration applicationConfiguration, String[] commandLineArguments) => webHost.Start();

        /// <summary>
        /// Creates configuration information for the web application.
        /// </summary>
        /// <returns>
        /// Configuration information for the web application.
        /// </returns>
        [DebuggerHidden]
        private IConfiguration CreateApplicationConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            BuildConfiguration(configurationBuilder);
            return configurationBuilder.Build();
        }

        /// <summary>
        /// Creates the dependency engine for the web application.
        /// </summary>
        /// <returns>
        /// The dependency engine for the web application.
        /// </returns>
        [DebuggerHidden]
        private IDependencyEngine CreateDependencyEngine()
        {
            var dependencyPackage = new TDependencyPackage();
            return dependencyPackage.CreateEngine(ApplicationConfiguration, ServiceDescriptors);
        }

        /// <summary>
        /// Creates the web application host.
        /// </summary>
        /// <returns>
        /// The web application host.
        /// </returns>
        [DebuggerHidden]
        private IWebHost CreateWebHost() => WebHostInitializer.CreateDefaultBuilder()
            .UseConfiguration(ApplicationConfiguration)
            .Configure(application =>
            {
                ConfigureApplication(application, ApplicationConfiguration, CommandLineArguments);
            })
            .ConfigureServices(services =>
            {
                foreach (var serviceDescritor in services)
                {
                    ServiceDescriptors.TryAdd(serviceDescritor);
                }
            })
            .Configure(application =>
            {
                application.ApplicationServices = DependencyEngine.Provider;
            })
            .Build();

        /// <summary>
        /// Gets the name of the web application.
        /// </summary>
        public String ApplicationName
        {
            get;
        }

        /// <summary>
        /// Gets configuration information for the web application.
        /// </summary>
        protected IConfiguration ApplicationConfiguration => LazyApplicationConfiguration.Value;

        /// <summary>
        /// When overridden by a derived class, gets a copyright notice which is written to the console at the start of web
        /// application execution.
        /// </summary>
        protected virtual String CopyrightNotice => null;

        /// <summary>
        /// When overridden by a derived class, gets a collection of textual prefixes which are used by
        /// <see cref="BuildConfiguration(IConfigurationBuilder)" /> to find and add environment variables to
        /// <see cref="ApplicationConfiguration" />.
        /// </summary>
        protected virtual IEnumerable<String> EnvironmentVariableConfigurationPrefixes => Array.Empty<String>();

        /// <summary>
        /// When overridden by a derived class, gets a product name associated with the web application which is written to the
        /// console at the start of web application execution.
        /// </summary>
        protected virtual String ProductName => null;

        /// <summary>
        /// Gets a utility that disposes of the object references that are managed by the current
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        protected IReferenceManager ReferenceManager
        {
            get;
        }

        /// <summary>
        /// Gets the dependency engine for the web application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDependencyEngine DependencyEngine => LazyDependencyEngine.Value;

        /// <summary>
        /// Gets the root dependency scope for the web application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDependencyScope RootDependencyScope => LazyRootDependencyScope.Value;

        /// <summary>
        /// Gets the web application host.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IWebHost WebHost => LazyWebHost.Value;

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
        /// Represents lazily-initialized configuration information for the web application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IConfiguration> LazyApplicationConfiguration;

        /// <summary>
        /// Represents the lazily-initialized dependency engine for the web application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDependencyEngine> LazyDependencyEngine;

        /// <summary>
        /// Represents the lazily-initialized root dependency scope for the web application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDependencyScope> LazyRootDependencyScope;

        /// <summary>
        /// Represents the lazily-initialized web application host.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IWebHost> LazyWebHost;

        /// <summary>
        /// Represents a collection of configured service descriptors.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IServiceCollection ServiceDescriptors = new ServiceCollection();
    }
}