// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a utility that performs configurable, scaling action repetition.
    /// </summary>
    public interface IActionRepeater : IInstrument
    {
        /// <summary>
        /// Executes a configured action repeatedly until the specified predicate returns <see langword="false" /> or until the
        /// specified terminal conditions are met.
        /// </summary>
        /// <exception cref="Exception">
        /// The configured predicate or action raised an exception.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The maximum repetition count and/or timeout threshold were exceeded.
        /// </exception>
        public void Run();

        /// <summary>
        /// Asynchronously executes a configured action repeatedly until the specified predicate returns <see langword="false" /> or
        /// until the specified terminal conditions are met.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Exception">
        /// The configured predicate or action raised an exception.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The maximum repetition count and/or timeout threshold were exceeded.
        /// </exception>
        public Task RunAsync();
    }
}