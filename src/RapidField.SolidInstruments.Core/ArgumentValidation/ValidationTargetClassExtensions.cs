// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends the <see cref="ValidationTarget{TArgument}" /> class with validation features for class arguments.
    /// </summary>
    public static class ValidationTargetClassExtensions
    {
        /// <summary>
        /// Raises a new <see cref="ArgumentNullException" /> if the target argument is <see langword="null" />.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <typeparam name="TArgument">
        /// The type of the target argument.
        /// </typeparam>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The target argument is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsNull<TArgument>(this ValidationTarget<TArgument> target)
            where TArgument : class => target.IsNull(null);

        /// <summary>
        /// Raises a new <see cref="ArgumentNullException" /> if the target argument is <see langword="null" />.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <typeparam name="TArgument">
        /// The type of the target argument.
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
        /// <exception cref="ArgumentNullException">
        /// The target argument is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsNull<TArgument>(this ValidationTarget<TArgument> target, String targetParameterName)
            where TArgument : class => target.Argument is null ? throw new ArgumentNullException(targetParameterName) : target.Result;
    }
}