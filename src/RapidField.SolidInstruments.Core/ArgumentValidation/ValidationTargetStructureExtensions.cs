// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends the <see cref="ValidationTarget{TArgument}" /> class with validation features for structure arguments.
    /// </summary>
    public static class ValidationTargetStructureExtensions
    {
        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the current value is equal to the specified value.
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
        /// <param name="otherValue">
        /// A value to compare with the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target value is equal to <paramref name="otherValue" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<TArgument> IsEqualToValue<TArgument>(this ValidationTarget<TArgument> target, TArgument otherValue)
            where TArgument : struct => target.IsEqualToValue(otherValue, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the current value is equal to the specified value.
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
        /// <param name="otherValue">
        /// A value to compare with the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target value is equal to <paramref name="otherValue" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<TArgument> IsEqualToValue<TArgument>(this ValidationTarget<TArgument> target, TArgument otherValue, String targetParameterName)
            where TArgument : struct => target.IsEqualToValue(otherValue, targetParameterName, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the current value is equal to the specified method argument
        /// value.
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
        /// <param name="otherArgument">
        /// Another argument to compare with the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <param name="otherParameterName">
        /// The name of the method parameter for the other argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target value is equal to <paramref name="otherArgument" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<TArgument> IsEqualToValue<TArgument>(this ValidationTarget<TArgument> target, TArgument otherArgument, String targetParameterName, String otherParameterName)
            where TArgument : struct
        {
            if (target.Argument.Equals(otherArgument))
            {
                if (targetParameterName is null)
                {
                    if (otherParameterName is null)
                    {
                        throw new ArgumentOutOfRangeException(null, $"The argument cannot be equal to {otherArgument}.");
                    }

                    throw new ArgumentOutOfRangeException(null, $"The argument cannot be equal to the argument for {otherParameterName}.");
                }
                else if (otherParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be equal to {otherArgument}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be equal to the argument for {otherParameterName}.");
            }

            return target.Result;
        }
    }
}