// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Prototype.DatabaseModel.Commands
{
    /// <summary>
    /// Represents a data access command that returns the numeric values for the Fibonacci series.
    /// </summary>
    public sealed class GetFibonacciNumberValuesCommand : DataAccessCommand<IEnumerable<Int64>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFibonacciNumberValuesCommand" /> class.
        /// </summary>
        [DebuggerHidden]
        internal GetFibonacciNumberValuesCommand()
            : base()
        {
            return;
        }
    }
}