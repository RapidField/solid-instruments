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

namespace RapidField.SolidInstruments.Example.WebApplication
{
    /// <summary>
    /// Houses application configuration information and mechanics.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the application.
        /// </param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures the application prior to startup.
        /// </summary>
        /// <param name="app">
        /// An object that configures the application's request pipeline.
        /// </param>
        /// <param name="env">
        /// Information about the application's hosting environment.
        /// </param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }

        /// <summary>
        /// Configures dependencies for the application.
        /// </summary>
        /// <param name="services">
        /// A collection of service descriptors to which dependencies are added.
        /// </param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddControllersAsServices();
            services.AddDependencyPackage<ApplicationDependencyPackage>(Configuration, out var serviceProvider);
            return serviceProvider;
        }

        /// <summary>
        /// Gets configuration information for the application.
        /// </summary>
        public IConfiguration Configuration
        {
            get;
        }
    }
}