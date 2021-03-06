﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.DataAccess;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Example.DatabaseModel.Commands
{
    /// <summary>
    /// Represents a data access command that adds a specified numeric value to the Fibonacci series.
    /// </summary>
    [DataContract]
    public sealed class AddFibonacciNumberCommand : DataAccessCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddFibonacciNumberCommand" /> class.
        /// </summary>
        /// <param name="numberValue">
        /// The value of the Fibonacci number that is added to the series.
        /// </param>
        [DebuggerHidden]
        internal AddFibonacciNumberCommand(Int64 numberValue)
            : base()
        {
            NumberValue = numberValue;
        }

        /// <summary>
        /// Gets or sets the value of the Fibonacci number that is added to the series.
        /// </summary>
        [DataMember]
        public Int64 NumberValue
        {
            get;
            set;
        }
    }
}