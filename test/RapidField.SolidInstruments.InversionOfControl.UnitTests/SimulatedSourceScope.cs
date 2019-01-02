// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.InversionOfControl.UnitTests
{
    /// <summary>
    /// Represents a dependency injection scope that is used for testing.
    /// </summary>
    internal sealed class SimulatedSourceScope : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedSourceScope" /> class.
        /// </summary>
        /// <param name="parentContainer">
        /// The parent container.
        /// </param>
        public SimulatedSourceScope(SimulatedSourceContainer parentContainer)
        {
            ParentContainer = parentContainer;
            IsDisposed = false;
        }

        /// <summary>
        /// Returns a new <see cref="SimulatedSourceScope" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="SimulatedSourceScope" />.
        /// </returns>
        public SimulatedSourceScope CreateNewScope() => ParentContainer.CreateNewScope();

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedSourceScope" />.
        /// </summary>
        public void Dispose() => IsDisposed = true;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SimulatedSourceScope" /> has been disposed.
        /// </summary>
        public Boolean IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents the parent container.
        /// </summary>
        public readonly SimulatedSourceContainer ParentContainer;
    }
}