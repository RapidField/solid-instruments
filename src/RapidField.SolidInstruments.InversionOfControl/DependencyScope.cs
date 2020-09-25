// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents a shared initialization and disposal scope for container-resolved objects.
    /// </summary>
    /// <remarks>
    /// <see cref="DependencyScope{TScope}" /> is the default implementation of <see cref="IDependencyScope" />.
    /// </remarks>
    /// <typeparam name="TScope">
    /// The type of the scope that is abstracted by the <see cref="DependencyScope{TScope}" />.
    /// </typeparam>
    public abstract class DependencyScope<TScope> : Instrument, IDependencyScope
        where TScope : class, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyScope{TScope}" /> class.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceScope" /> is <see langword="null" />.
        /// </exception>
        protected DependencyScope(TScope sourceScope)
            : base()
        {
            LazyReferenceManager = new Lazy<IReferenceManager>(() => new ReferenceManager(), LazyThreadSafetyMode.ExecutionAndPublication);
            SourceScope = sourceScope.RejectIf().IsNull(nameof(sourceScope));
        }

        /// <summary>
        /// Creates a new child initialization and disposal scope for the current <see cref="DependencyScope{TScope}" />.
        /// </summary>
        /// <returns>
        /// A new child initialization and disposal scope for the current <see cref="DependencyScope{TScope}" />.
        /// </returns>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create the scope.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IDependencyScope CreateChildScope()
        {
            RejectIfDisposed();

            try
            {
                var childScope = CreateChildScope(SourceScope);
                ReferenceManager.AddObject(childScope);
                return childScope;
            }
            catch (Exception exception)
            {
                throw new CreateDependencyScopeException(exception);
            }
        }

        /// <summary>
        /// Requests an object of specified type from the associated container for the current <see cref="IDependencyScope" />.
        /// </summary>
        /// <param name="type">
        /// The type of the object to resolve.
        /// </param>
        /// <returns>
        /// An object of specified type from the associated container.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="DependencyResolutionException">
        /// An exception was raised while attempting to resolve the dependency.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Object Resolve(Type type)
        {
            RejectIfDisposed();

            try
            {
                var resolvedObject = Resolve(SourceScope, type.RejectIf().IsNull(nameof(type)));

                if (ManagesReferencesForSourceScope)
                {
                    ReferenceManager.AddObject(resolvedObject);
                }

                return resolvedObject;
            }
            catch (Exception exception)
            {
                throw new DependencyResolutionException(type, exception);
            }
        }

        /// <summary>
        /// Requests an object of specified type from the associated container for the current
        /// <see cref="DependencyScope{TScope}" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to resolve.
        /// </typeparam>
        /// <returns>
        /// An object of specified type from the associated container.
        /// </returns>
        /// <exception cref="DependencyResolutionException">
        /// An exception was raised while attempting to resolve the dependency.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public T Resolve<T>()
            where T : class
        {
            RejectIfDisposed();

            try
            {
                var resolvedObject = Resolve<T>(SourceScope);

                if (ManagesReferencesForSourceScope)
                {
                    ReferenceManager.AddObject(resolvedObject);
                }

                return resolvedObject;
            }
            catch (Exception exception)
            {
                throw new DependencyResolutionException(typeof(T), exception);
            }
        }

        /// <summary>
        /// Creates a new child initialization and disposal scope for the current <see cref="DependencyScope{TScope}" />.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <returns>
        /// A new child initialization and disposal scope for the current <see cref="DependencyScope{TScope}" />.
        /// </returns>
        protected abstract IDependencyScope CreateChildScope(TScope sourceScope);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DependencyScope{TScope}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                SourceScope?.Dispose();
                LazyReferenceManager?.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

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
        protected abstract Object Resolve(TScope sourceScope, Type type);

        /// <summary>
        /// Requests an object of specified type from the associated container for the current
        /// <see cref="DependencyScope{TScope}" />.
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
        protected abstract T Resolve<T>(TScope sourceScope)
            where T : class;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="DependencyScope{TScope}" /> manages and disposes of
        /// resolved references on behalf of the scope.
        /// </summary>
        protected virtual Boolean ManagesReferencesForSourceScope => DefaultManagesReferencesForSourceScope;

        /// <summary>
        /// Gets a utility that disposes of the object references that are managed by the current
        /// <see cref="DependencyScope{TScope}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IReferenceManager ReferenceManager => LazyReferenceManager.Value;

        /// <summary>
        /// Represents the default value indicating whether or not the current <see cref="DependencyScope{TScope}" /> manages and
        /// disposes of resolved references on behalf of the source scope.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultManagesReferencesForSourceScope = false;

        /// <summary>
        /// Represents the lazily-initialized utility that disposes of the object references that are managed by the current
        /// <see cref="DependencyScope{TScope}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IReferenceManager> LazyReferenceManager;

        /// <summary>
        /// Gets the scope that is abstracted by the current <see cref="DependencyScope{TScope}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TScope SourceScope;
    }
}