// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Facilitates validation for an argument of a given type.
    /// </summary>
    /// <typeparam name="TArgument">
    /// The type of the target argument.
    /// </typeparam>
    public sealed class ValidationTarget<TArgument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationTarget{TArgument}" /> class.
        /// </summary>
        /// <param name="argument">
        /// The target argument.
        /// </param>
        [DebuggerHidden]
        internal ValidationTarget(TArgument argument)
        {
            Argument = argument;
        }

        /// <summary>
        /// Gets the type of the target argument.
        /// </summary>
        public Type ArgumentType => typeof(TArgument);

        /// <summary>
        /// Gets the result of the validation operation.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal ValidationResult<TArgument> Result => new(this);

        /// <summary>
        /// Represents the target argument.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly TArgument Argument;
    }
}