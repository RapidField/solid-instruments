// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends objects with argument validation features.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Initializes an <see cref="ValidationTarget{TArgument}" /> instance that exposes argument validation mechanics for the
        /// target argument.
        /// </summary>
        /// <typeparam name="TArgument">
        /// The type of the evaluated argument.
        /// </typeparam>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <returns>
        /// An <see cref="ValidationTarget{TArgument}" /> instance that exposes argument validation mechanics for the target
        /// argument.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationTarget<TArgument> RejectIf<TArgument>(this TArgument target) => new(target);

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument satisfies the specified predicate.
        /// </summary>
        /// <typeparam name="TArgument">
        /// The type of the evaluated argument.
        /// </typeparam>
        /// <param name="target">
        /// The target argument.
        /// </param>
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
        public static ValidationResult<TArgument> RejectIf<TArgument>(this TArgument target, Predicate<TArgument> predicate) => target.RejectIf(predicate, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument satisfies the specified predicate.
        /// </summary>
        /// <typeparam name="TArgument">
        /// The type of the evaluated argument.
        /// </typeparam>
        /// <param name="target">
        /// The target argument.
        /// </param>
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
        public static ValidationResult<TArgument> RejectIf<TArgument>(this TArgument target, Predicate<TArgument> predicate, String targetParameterName) => target.RejectIf(predicate, targetParameterName, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument satisfies the specified predicate.
        /// </summary>
        /// <typeparam name="TArgument">
        /// The type of the evaluated argument.
        /// </typeparam>
        /// <param name="target">
        /// The target argument.
        /// </param>
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
        public static ValidationResult<TArgument> RejectIf<TArgument>(this TArgument target, Predicate<TArgument> predicate, String targetParameterName, String exceptionMessage)
        {
            if (predicate(target))
            {
                throw new ArgumentException(exceptionMessage, targetParameterName);
            }

            return new(new(target));
        }
    }
}