// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RapidField.SolidInstruments.Web.DotNetNative;
using System;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.HttpApi
{
    /// <summary>
    /// Prepares for and performs execution of the <see cref="AccessControl" /> HTTP API application.
    /// </summary>
    public sealed class ApplicationWebExecutor : DotNetNativeWebExecutor<ApplicationDependencyPackage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationWebExecutor" /> class.
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
        public ApplicationWebExecutor(String applicationName)
            : base(applicationName)
        {
            return;
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
        protected override void ConfigureApplication(IApplicationBuilder application, IConfiguration applicationConfiguration)
        {
            try
            {
                application = application
                    .UseRouting()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
            }
            finally
            {
                base.ConfigureApplication(application, applicationConfiguration);
            }
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
        protected override IHostBuilder ConfigureHost(IHostBuilder host, Action<IWebHostBuilder> configureWebHostAction) => host.ConfigureWebHostDefaults(configureWebHostAction);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ApplicationWebExecutor" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// When overridden by a derived class, gets a copyright notice which is written to the console at the start of application
        /// execution.
        /// </summary>
        protected override sealed String CopyrightNotice => "Copyright (c) RapidField LLC. All rights reserved.";

        /// <summary>
        /// When overridden by a derived class, gets a product name associated with the application which is written to the console
        /// at the start of application execution.
        /// </summary>
        protected override sealed String ProductName => "Solid Instruments";
    }
}