// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Abstracts <see cref="IDependencyScope" /> for use by <see cref="IServiceCollection" /> instances.
    /// </summary>
    public sealed class ServiceScope : Instrument, IServiceScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceScope" /> class.
        /// </summary>
        /// <param name="dependencyScope">
        /// The scope that is abstracted by the service scope.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dependencyScope" /> is <see langword="null" />.
        /// </exception>
        public ServiceScope(IDependencyScope dependencyScope)
        {
            DependencyScope = dependencyScope.RejectIf().IsNull(nameof(dependencyScope)).TargetArgument;
            LazyProvider = new Lazy<IServiceProvider>(() => DependencyScope.Resolve<IServiceProvider>(), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ServiceScope" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    DependencyScope.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets the service provider that is used to resolve dependencies for the scope.
        /// </summary>
        /// <exception cref="DependencyResolutionException">
        /// An exception was raised while attempting to resolve the service provider.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IServiceProvider ServiceProvider
        {
            get
            {
                RejectIfDisposed();
                return LazyProvider.Value;
            }
        }

        /// <summary>
        /// Represents the scope that is abstracted by the service scope.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDependencyScope DependencyScope;

        /// <summary>
        /// Represents the lazily-initialized service provider that is used to resolve dependencies for the scope.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IServiceProvider> LazyProvider;
    }
}