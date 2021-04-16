// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using HostInitializer = Microsoft.Extensions.Hosting.Host;

namespace RapidField.SolidInstruments.Web
{
    /// <summary>
    /// Prepares for and performs execution of a web application.
    /// </summary>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    /// <typeparam name="TDependencyConfigurator">
    /// The type of the object that configures the dependency container.
    /// </typeparam>
    /// <typeparam name="TDependencyEngine">
    /// The type of the dependency engine that is produced by the dependency package.
    /// </typeparam>
    /// <typeparam name="TServiceProviderFactory">
    /// The type of the service provider factory that is used by the dependency engine.
    /// </typeparam>
    /// <typeparam name="TStartup">
    /// The type of the user-defined startup class that configures the web host.
    /// </typeparam>
    public abstract class WebExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory, TStartup> : WebExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory>
        where TDependencyPackage : class, IDependencyPackage<TDependencyConfigurator, TDependencyEngine>, new()
        where TDependencyConfigurator : class, new()
        where TDependencyEngine : class, IDependencyEngine
        where TServiceProviderFactory : class, IServiceProviderFactory<TDependencyConfigurator>
        where TStartup : class
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory, TStartup}" />
        /// class.
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
            : base(applicationName, true)
        {
            return;
        }

        /// <summary>
        /// Configures the web host using a user-defined startup class.
        /// </summary>
        /// <param name="webHost">
        /// An object that configures the web host.
        /// </param>
        /// <returns>
        /// The configured web host builder.
        /// </returns>
        [DebuggerHidden]
        internal sealed override IWebHostBuilder ConfigureWebHostUsingStartupClass(IWebHostBuilder webHost)
        {
            webHost = webHost.UseStartup<TStartup>();
            return base.ConfigureWebHostUsingStartupClass(webHost);
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override void ConfigureApplication(IApplicationBuilder application, IConfiguration applicationConfiguration) => base.ConfigureApplication(application, applicationConfiguration);

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory, TStartup}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Prepares for and performs execution of a web application.
    /// </summary>
    /// <remarks>
    /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory}" /> is the
    /// default implementation of <see cref="IWebExecutor" />.
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
    /// <typeparam name="TServiceProviderFactory">
    /// The type of the service provider factory that is used by the dependency engine.
    /// </typeparam>
    public abstract class WebExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory> : Instrument, IWebExecutor
        where TDependencyPackage : class, IDependencyPackage<TDependencyConfigurator, TDependencyEngine>, new()
        where TDependencyConfigurator : class, new()
        where TDependencyEngine : class, IDependencyEngine
        where TServiceProviderFactory : class, IServiceProviderFactory<TDependencyConfigurator>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory}" />
        /// class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the web application.
        /// </param>
        /// <param name="usesStartupClass">
        /// A value indicating whether or not the executor is configured using a user-defined startup class.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationName" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal WebExecutor(String applicationName, Boolean usesStartupClass)
            : base()
        {
            ApplicationName = applicationName.RejectIf().IsNullOrEmpty(nameof(applicationName)).TargetArgument.Trim();
            CommandLineArguments = null;
            UsesStartupClass = usesStartupClass;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory}" />
        /// class.
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
            : this(applicationName, false)
        {
            return;
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

                if (productName.IsNullOrEmpty() is false)
                {
                    Console.WriteLine(productName);
                }

                if (applicationName.IsNullOrEmpty() is false)
                {
                    Console.WriteLine(applicationName);
                }

                if (copyrightNotice.IsNullOrEmpty() is false)
                {
                    Console.WriteLine(copyrightNotice);
                }

                Console.WriteLine($"{Environment.NewLine}Web application execution starting.");

                try
                {
                    ApplicationConfiguration = CreateApplicationConfiguration();

                    using (var host = CreateHost())
                    {
                        host.Run();
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
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory}" /> to
        /// its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(ApplicationName)}\": \"{ApplicationName}\" }}";

        /// <summary>
        /// Configures the application's request pipeline.
        /// </summary>
        /// <param name="application">
        /// An object that configures the application's request pipeline.
        /// </param>
        /// <exception cref="WebExectuionException">
        /// An exception was raised while configuring the application.
        /// </exception>
        [DebuggerHidden]
        internal void ConfigureApplication(IApplicationBuilder application)
        {
            try
            {
                ConfigureApplication(application, ApplicationConfiguration);
            }
            catch (WebExectuionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new WebExectuionException("An exception was raised while configuring the application. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Configures the host.
        /// </summary>
        /// <param name="host">
        /// An object that configures the host.
        /// </param>
        /// <returns>
        /// The configured host builder.
        /// </returns>
        /// <exception cref="WebExectuionException">
        /// An exception was raised while configuring the host.
        /// </exception>
        [DebuggerHidden]
        internal IHostBuilder ConfigureHost(IHostBuilder host)
        {
            try
            {
                return ConfigureHost(host, ConfigureWebHost);
            }
            catch (WebExectuionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new WebExectuionException("An exception was raised while configuring the host. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Configures the web host.
        /// </summary>
        /// <param name="webHost">
        /// An object that configures the web host.
        /// </param>
        /// <exception cref="WebExectuionException">
        /// An exception was raised while configuring the web host.
        /// </exception>
        [DebuggerHidden]
        internal void ConfigureWebHost(IWebHostBuilder webHost)
        {
            try
            {
                webHost = webHost.UseConfiguration(ApplicationConfiguration).ConfigureServices(ConfigureServices);
                webHost = UsesStartupClass ? ConfigureWebHostUsingStartupClass(webHost) : webHost.Configure(ConfigureApplication);
            }
            catch (WebExectuionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new WebExectuionException("An exception was raised while configuring the web host. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Configures the web host using a user-defined startup class.
        /// </summary>
        /// <param name="webHost">
        /// An object that configures the web host.
        /// </param>
        /// <returns>
        /// The configured web host builder.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal virtual IWebHostBuilder ConfigureWebHostUsingStartupClass(IWebHostBuilder webHost) => webHost;

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
                configurationBuilder = configurationBuilder.AddEnvironmentVariables(prefix);
            }

            if (CommandLineArguments.IsNullOrEmpty() is false)
            {
                configurationBuilder = configurationBuilder.AddCommandLine(CommandLineArguments);
            }

            configurationBuilder = configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(DefaultAppSettingsJsonFileName);
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
        protected virtual void ConfigureApplication(IApplicationBuilder application, IConfiguration applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the host.
        /// </summary>
        /// <param name="host">
        /// An object that configures the host.
        /// </param>
        /// <param name="configureWebHostAction">
        /// An action that configures the web host.
        /// </param>
        /// <returns>
        /// The configured host builder.
        /// </returns>
        protected abstract IHostBuilder ConfigureHost(IHostBuilder host, Action<IWebHostBuilder> configureWebHostAction);

        /// <summary>
        /// Configures application dependencies.
        /// </summary>
        /// <param name="services">
        /// An object that configures application dependencies.
        /// </param>
        /// <param name="dependencyPackage">
        /// A package that defines the configured dependencies.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        protected abstract void ConfigureServices(IServiceCollection services, TDependencyPackage dependencyPackage, IConfiguration applicationConfiguration);

        /// <summary>
        /// Creates the dependency package.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        /// <returns>
        /// The dependency package.
        /// </returns>
        protected virtual TDependencyPackage CreateDependencyPackage(IConfiguration applicationConfiguration) => new();

        /// <summary>
        /// Creates the service provider factory.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        /// <returns>
        /// The service provider factory.
        /// </returns>
        protected abstract TServiceProviderFactory CreateServiceProviderFactory(IConfiguration applicationConfiguration);

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Configures application dependencies.
        /// </summary>
        /// <param name="services">
        /// An object that configures application dependencies.
        /// </param>
        /// <exception cref="WebExectuionException">
        /// An exception was raised while configuring application dependencies.
        /// </exception>
        [DebuggerHidden]
        private void ConfigureServices(IServiceCollection services)
        {
            try
            {
                var applicationConfiguration = ApplicationConfiguration;
                var dependencyPackage = CreateDependencyPackage(applicationConfiguration);
                ConfigureServices(services, dependencyPackage, applicationConfiguration);
            }
            catch (Exception exception)
            {
                throw new WebExectuionException("An exception was raised while configuring application dependencies. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Creates configuration information for the web application.
        /// </summary>
        /// <returns>
        /// Configuration information for the web application.
        /// </returns>
        /// <exception cref="WebExectuionException">
        /// An exception was raised while creating the application configuration.
        /// </exception>
        [DebuggerHidden]
        private IConfiguration CreateApplicationConfiguration()
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder();
                BuildConfiguration(configurationBuilder);
                return configurationBuilder.Build();
            }
            catch (Exception exception)
            {
                throw new WebExectuionException("An exception was raised while creating the application configuration. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Creates the web application host.
        /// </summary>
        /// <returns>
        /// The web application host.
        /// </returns>
        /// <exception cref="WebExectuionException">
        /// An exception was raised while creating the host.
        /// </exception>
        [DebuggerHidden]
        private IHost CreateHost()
        {
            try
            {
                var hostBuilder = CreateHostBuilder();
                return hostBuilder.Build();
            }
            catch (WebExectuionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new WebExectuionException("An exception was raised while creating the host. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Creates the web application host builder.
        /// </summary>
        /// <returns>
        /// The web application host builder.
        /// </returns>
        /// <exception cref="WebExectuionException">
        /// An exception was raised while creating the host builder.
        /// </exception>
        [DebuggerHidden]
        private IHostBuilder CreateHostBuilder()
        {
            try
            {
                var hostBuilder = HostInitializer.CreateDefaultBuilder(CommandLineArguments);
                return ConfigureHost(hostBuilder);
            }
            catch (WebExectuionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new WebExectuionException("An exception was raised while creating the host builder. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Creates the service provider factory.
        /// </summary>
        /// <returns>
        /// The service provider factory.
        /// </returns>
        /// <exception cref="WebExectuionException">
        /// An exception was raised while creating the service provider factory.
        /// </exception>
        [DebuggerHidden]
        private TServiceProviderFactory CreateServiceProviderFactory()
        {
            try
            {
                return CreateServiceProviderFactory(ApplicationConfiguration);
            }
            catch (WebExectuionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new WebExectuionException("An exception was raised while creating the service provider factory. See inner exception.", exception);
            }
        }

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
        /// <exception cref="WebExectuionException">
        /// An exception was raised while creating the application configuration.
        /// </exception>
        protected IConfiguration ApplicationConfiguration
        {
            get;
            private set;
        }

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
        /// Represents a value indicating whether or not the current
        /// <see cref="WebExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine, TServiceProviderFactory}" /> is
        /// configured using a user-defined startup class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean UsesStartupClass;
    }
}