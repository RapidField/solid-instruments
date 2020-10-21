// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using System;
using System.Diagnostics;
using System.IO;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.HttpApi
{
    /// <summary>
    /// Houses the entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Begins execution of the application.
        /// </summary>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime.
        /// </param>
        public static void Main(String[] commandLineArguments)
        {
            var applicationConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddCommandLine(commandLineArguments)
                .Build();
            var host = Host.CreateDefaultBuilder(commandLineArguments)
               .ConfigureWebHostDefaults(webHost =>
               {
                   webHost.UseConfiguration(applicationConfiguration);
                   webHost.ConfigureServices(services =>
                   {
                       services.AddDependencyPackage(new ApplicationDependencyPackage(), applicationConfiguration);
                   });
                   webHost.Configure(application =>
                   {
                       application.UseAuthorization();
                       application.UseRouting();
                       application.UseEndpoints(endpoints =>
                       {
                           endpoints.MapControllers();
                       });
                   });
               })
               .Build();
            host.Run();
        }

        /// <summary>
        /// Represents the name of this application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly String ApplicationName = $"{nameof(AccessControl)} HTTP API";
    }
}