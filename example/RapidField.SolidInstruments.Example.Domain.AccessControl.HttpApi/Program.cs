// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.HttpApi
{
    /// <summary>
    /// Houses the entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Configures the application hosting environment.
        /// </summary>
        /// <param name="args">
        /// Command line arguments that are provided at runtime.
        /// </param>
        /// <returns>
        /// The resulting host configuration.
        /// </returns>
        public static IHostBuilder CreateHostBuilder(String[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

        /// <summary>
        /// Begins execution of the application.
        /// </summary>
        /// <param name="args">
        /// Command line arguments that are provided at runtime.
        /// </param>
        public static void Main(String[] args) => CreateHostBuilder(args).Build().Run();
    }
}