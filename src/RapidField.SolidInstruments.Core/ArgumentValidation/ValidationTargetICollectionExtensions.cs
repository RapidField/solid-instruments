// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends the <see cref="ValidationTarget{TArgument}" /> class with validation features for <see cref="ICollection{T}" />
    /// arguments.
    /// </summary>
    public static class ValidationTargetICollectionExtensions
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
        public static ValidationResult<ICollection<T>> IsNullOrEmpty<T>(this ValidationTarget<ICollection<T>> target) => target.IsNullOrEmpty(null);

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
        public static ValidationResult<ICollection<T>> IsNullOrEmpty<T>(this ValidationTarget<ICollection<T>> target, String targetParameterName) => target.RejectIfIsNullOrEmpty<ICollection<T>, T>(targetParameterName);
    }
}