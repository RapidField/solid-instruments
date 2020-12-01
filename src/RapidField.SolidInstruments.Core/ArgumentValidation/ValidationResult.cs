// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Represents the result of an argument validation operation.
    /// </summary>
    /// <typeparam name="TArgument">
    /// The type of the target argument.
    /// </typeparam>
    public sealed class ValidationResult<TArgument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult{TArgument}" /> class.
        /// </summary>
        /// <param name="target">
        /// The validation target.
        /// </param>
        [DebuggerHidden]
        internal ValidationResult(ValidationTarget<TArgument> target)
        {
            Target = target;
        }

        /// <summary>
        /// Facilitates implicit <see cref="ValidationResult{TArgument}" /> to <typeparamref name="TArgument" /> array casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator TArgument(ValidationResult<TArgument> target) => target is null ? default : target.TargetArgument;

        /// <summary>
        /// Returns the validation target to facilitate chained argument validation.
        /// </summary>
        /// <returns>
        /// The validation target.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValidationTarget<TArgument> OrIf() => Target;

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument satisfies the specified predicate.
        /// </summary>
        /// <param name="predicate">
        /// A function to test the target argument for a condition. When the result is <see langword="true" />, a new
        /// <see cref="ArgumentException" /> is raised.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The target argument satisfies the specified predicate.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValidationResult<TArgument> OrIf(Predicate<TArgument> predicate) => OrIf(predicate, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument satisfies the specified predicate.
        /// </summary>
        /// <param name="predicate">
        /// A function to test the target argument for a condition. When the result is <see langword="true" />, a new
        /// <see cref="ArgumentException" /> is raised.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The target argument satisfies the specified predicate.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValidationResult<TArgument> OrIf(Predicate<TArgument> predicate, String targetParameterName) => OrIf(predicate, targetParameterName, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument satisfies the specified predicate.
        /// </summary>
        /// <param name="predicate">
        /// A function to test the target argument for a condition. When the result is <see langword="true" />, a new
        /// <see cref="ArgumentException" /> is raised.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <param name="exceptionMessage">
        /// The message that explains the reason for the exception.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The target argument satisfies the specified predicate.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValidationResult<TArgument> OrIf(Predicate<TArgument> predicate, String targetParameterName, String exceptionMessage)
        {
            if (predicate(TargetArgument))
            {
                throw new ArgumentException(exceptionMessage, targetParameterName);
            }

            return new(Target);
        }

        /// <summary>
        /// Gets the target argument.
        /// </summary>
        public TArgument TargetArgument => Target.Argument;

        /// <summary>
        /// Represents the validation target.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ValidationTarget<TArgument> Target;
    }
}