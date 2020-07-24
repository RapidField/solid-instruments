// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with inversion of control features.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers the specified <see cref="IConfiguration" /> instance as a singleton.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        public static void RegisterApplicationConfiguration(this ContainerBuilder target, IConfiguration applicationConfiguration) => target.RegisterInstance(applicationConfiguration).SingleInstance();
    }
}