// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends the <see cref="ValidationTarget{TArgument}" /> class with validation features for <see cref="IEnumerable{T}" />
    /// arguments.
    /// </summary>
    public static class ValidationTargetIEnumerableExtensions
    {
        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument is <see langword="null" /> or empty.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <typeparam name="T">
        /// The element type of the target argument.
        /// </typeparam>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// The target argument is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The target argument is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<IEnumerable<T>> IsNullOrEmpty<T>(this ValidationTarget<IEnumerable<T>> target) => target.IsNullOrEmpty(null);

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument is <see langword="null" /> or empty.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <typeparam name="T">
        /// The element type of the target argument.
        /// </typeparam>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// The target argument is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The target argument is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<IEnumerable<T>> IsNullOrEmpty<T>(this ValidationTarget<IEnumerable<T>> target, String targetParameterName) => target.RejectIfIsNullOrEmpty<IEnumerable<T>, T>(targetParameterName);

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument is <see langword="null" /> or empty.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <typeparam name="TArgument">
        /// The type of the target argument.
        /// </typeparam>
        /// <typeparam name="T">
        /// The element type of the target argument.
        /// </typeparam>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// The target argument is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The target argument is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ValidationResult<TArgument> RejectIfIsNullOrEmpty<TArgument, T>(this ValidationTarget<TArgument> target, String targetParameterName)
            where TArgument : IEnumerable<T>
        {
            if (target.Argument is null)
            {
                throw new ArgumentNullException(targetParameterName);
            }
            else if (target.Argument.Any() is false)
            {
                throw new ArgumentEmptyException(targetParameterName);
            }

            return target.Result;
        }
    }
}