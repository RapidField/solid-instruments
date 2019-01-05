// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace RapidField.SolidInstruments.Example.WebApplication
{
    /// <summary>
    /// Houses the entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Configures the hosting environment.
        /// </summary>
        /// <param name="args">
        /// Command line arguments provided at runtime.
        /// </param>
        /// <returns>
        /// The configured hosting environment.
        /// </returns>
        public static IWebHost BuildWebHost(String[] args) => WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel()
            .UseStartup<Startup>()
            .Build();

        /// <summary>
        /// Begins execution of the application.
        /// </summary>
        /// <param name="args">
        /// Command line arguments that are provided at runtime.
        /// </param>
        public static void Main(String[] args) => BuildWebHost(args).Run();
    }
}