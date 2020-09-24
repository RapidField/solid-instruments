// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative
{
    /// <summary>
    /// Represents a shared initialization and disposal scope for native .NET container-resolved objects.
    /// </summary>
    public sealed class DotNetNativeDependencyScope : DependencyScope<IServiceScope>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeDependencyScope" /> class.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceScope" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal DotNetNativeDependencyScope(IServiceScope sourceScope)
            : base(sourceScope)
        {
            return;
        }

        /// <summary>
        /// Creates a new child initialization and disposal scope for the current <see cref="DotNetNativeDependencyScope" />.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <returns>
        /// A new child initialization and disposal scope for the current <see cref="DotNetNativeDependencyScope" />.
        /// </returns>
        protected override IDependencyScope CreateChildScope(IServiceScope sourceScope) => new DotNetNativeDependencyScope(sourceScope.ServiceProvider.CreateScope());

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DotNetNativeDependencyScope" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected sealed override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Requests an object of specified type from the associated container for the current
        /// <see cref="DependencyScope{TScope}" />.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <param name="type">
        /// The type of the object to resolve.
        /// </param>
        /// <returns>
        /// An object of specified type from the associated container.
        /// </returns>
        protected override Object Resolve(IServiceScope sourceScope, Type type) => sourceScope.ServiceProvider.GetRequiredService(type);

        /// <summary>
        /// Requests an object of specified type from the associated container for the current
        /// <see cref="DotNetNativeDependencyScope" />.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <typeparam name="T">
        /// The type of the object to resolve.
        /// </typeparam>
        /// <returns>
        /// An object of specified type from the associated container.
        /// </returns>
        protected override T Resolve<T>(IServiceScope sourceScope) => sourceScope.ServiceProvider.GetRequiredService<T>();
    }
}