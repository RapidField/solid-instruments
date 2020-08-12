// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.DataAccess;
using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Commands;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests
{
    /// <summary>
    /// Exposes a collection of data access commands for the Simulated database.
    /// </summary>
    public static class SimulatedCommands
    {
        /// <summary>
        /// Creates a data access command that adds a specified numeric value to the Fibonacci series.
        /// </summary>
        /// <param name="numberValue">
        /// The value of the Fibonacci number that is added to the series.
        /// </param>
        public static IDataAccessCommand<Nix> AddFibonacciNumber(Int64 numberValue) => new AddFibonacciNumberCommand(numberValue);

        /// <summary>
        /// Creates a data access command that returns the numeric values for the Fibonacci series.
        /// </summary>
        /// <returns>
        /// The numeric values for the Fibonacci series.
        /// </returns>
        public static IDataAccessCommand<IEnumerable<Int64>> GetFibonacciNumberValues() => new GetFibonacciNumberValuesCommand();
    }
}