// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac
{
    /// <summary>
    /// Represents a shared initialization and disposal scope for Autofac container-resolved objects.
    /// </summary>
    public sealed class AutofacDependencyScope : DependencyScope<ILifetimeScope>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyScope" /> class.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceScope" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal AutofacDependencyScope(ILifetimeScope sourceScope)
            : base(sourceScope)
        {
            return;
        }

        /// <summary>
        /// Creates a new child initialization and disposal scope for the current <see cref="AutofacDependencyScope" />.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <returns>
        /// A new child initialization and disposal scope for the current <see cref="AutofacDependencyScope" />.
        /// </returns>
        protected override IDependencyScope CreateChildScope(ILifetimeScope sourceScope) => new AutofacDependencyScope(sourceScope.BeginLifetimeScope());

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AutofacDependencyScope" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
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
        protected override Object Resolve(ILifetimeScope sourceScope, Type type) => sourceScope.Resolve(type);

        /// <summary>
        /// Requests an object of specified type from the associated container for the current
        /// <see cref="AutofacDependencyScope" />.
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
        protected override T Resolve<T>(ILifetimeScope sourceScope) => sourceScope.Resolve<T>();
    }
}