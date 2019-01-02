// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.UnitTests
{
    /// <summary>
    /// Represents a <see cref="DependencyScope{TScope}" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedDependencyScope : DependencyScope<SimulatedSourceScope>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDependencyScope" /> class.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceScope" /> is <see langword="null" />.
        /// </exception>
        public SimulatedDependencyScope(SimulatedSourceScope sourceScope)
            : base(sourceScope)
        {
            return;
        }

        /// <summary>
        /// Creates a new child initialization and disposal scope for the current <see cref="SimulatedDependencyScope" />.
        /// </summary>
        /// <param name="sourceScope">
        /// The underlying scope.
        /// </param>
        /// <returns>
        /// A new child initialization and disposal scope for the current <see cref="SimulatedDependencyScope" />.
        /// </returns>
        protected override IDependencyScope CreateChildScope(SimulatedSourceScope sourceScope) => new SimulatedDependencyScope(sourceScope.CreateNewScope());

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedDependencyScope" />.
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
        protected override Object Resolve(SimulatedSourceScope sourceScope, Type type)
        {
            var instrument = new SimulatedInstrument(ConcurrencyControlMode.Unconstrained);
            instrument.StoreIntegerValue(sourceScope.ParentContainer.TestValue);
            return instrument;
        }

        /// <summary>
        /// Requests an object of specified type from the associated container for the current
        /// <see cref="SimulatedDependencyScope" />.
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
        protected override T Resolve<T>(SimulatedSourceScope sourceScope)
        {
            var instrument = new SimulatedInstrument(ConcurrencyControlMode.Unconstrained);
            instrument.StoreIntegerValue(sourceScope.ParentContainer.TestValue);
            return instrument as T;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SimulatedDependecyContainer" /> manages and disposes of
        /// resolved references on behalf of the scope.
        /// </summary>
        protected override sealed Boolean ManagesReferencesForSourceScope => true;
    }
}