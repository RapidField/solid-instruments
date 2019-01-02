// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.InversionOfControl.UnitTests
{
    /// <summary>
    /// Represents a dependency injection container that is used for testing.
    /// </summary>
    internal sealed class SimulatedSourceContainer : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedSourceContainer" /> class.
        /// </summary>
        /// <param name="testValue">
        /// Represents a test value that is injected into resolved objects.
        /// </param>
        public SimulatedSourceContainer(Int32 testValue)
        {
            IsDisposed = false;
            TestValue = testValue;
        }

        /// <summary>
        /// Returns a new <see cref="SimulatedSourceScope" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="SimulatedSourceScope" />.
        /// </returns>
        public SimulatedSourceScope CreateNewScope()
        {
            var newScope = new SimulatedSourceScope(this);
            ChildScopeCollection.Add(newScope);
            return newScope;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedSourceContainer" />.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            foreach (var childScope in ChildScopeCollection)
            {
                childScope?.Dispose();
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SimulatedSourceContainer" /> has been disposed.
        /// </summary>
        public Boolean IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents a test value that is injected into resolved objects.
        /// </summary>
        public readonly Int32 TestValue;

        /// <summary>
        /// Represents the child scopes for the current <see cref="SimulatedSourceContainer" />.
        /// </summary>
        public IList<SimulatedSourceScope> ChildScopeCollection = new List<SimulatedSourceScope>();
    }
}