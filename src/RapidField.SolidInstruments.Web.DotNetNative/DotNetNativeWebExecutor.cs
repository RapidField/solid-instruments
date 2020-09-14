// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using System;

namespace RapidField.SolidInstruments.Web.DotNetNative
{
    /// <summary>
    /// Prepares for and performs execution of a web application using native .NET IoC tooling.
    /// </summary>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    public abstract class DotNetNativeWebExecutor<TDependencyPackage> : WebExecutor<TDependencyPackage, ServiceCollection, DotNetNativeDependencyEngine>
        where TDependencyPackage : class, IDependencyPackage<ServiceCollection, DotNetNativeDependencyEngine>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeWebExecutor{TDependencyPackage}" /> class.
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
        protected DotNetNativeWebExecutor(String applicationName)
            : base(applicationName)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DotNetNativeWebExecutor{TDependencyPackage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}