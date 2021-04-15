// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument?> IsEqualToValue<TArgument>(this ValidationTarget<TArgument?> target, TArgument otherValue)
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
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target value is equal to <paramref name="otherValue" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument?> IsEqualToValue<TArgument>(this ValidationTarget<TArgument?> target, TArgument otherValue, String targetParameterName)
            where TArgument : struct => target.IsEqualToValue(otherValue, targetParameterName, null);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument?> IsEqualToValue<TArgument>(this ValidationTarget<TArgument?> target, TArgument otherArgument, String targetParameterName, String otherParameterName)
            where TArgument : struct => target.Argument.HasValue ? new ValidationResult<TArgument?>(new ValidationTarget<TArgument?>(target.Argument.Value.RejectIf().IsEqualToValue(otherArgument, targetParameterName, otherParameterName).TargetArgument)) : target.Result;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        public static ValidationResult<TArgument?> IsNull<TArgument>(this ValidationTarget<TArgument?> target)
            where TArgument : struct => target.IsNull(null);

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
        public static ValidationResult<TArgument?> IsNull<TArgument>(this ValidationTarget<TArgument?> target, String targetParameterName)
            where TArgument : struct => target.Argument is null ? throw new ArgumentNullException(targetParameterName) : target.Result;

        /// <summary>
        /// Raises a new <see cref="ArgumentNullException" /> if the target argument is <see langword="null" /> or raises a new
        /// <see cref="ArgumentOutOfRangeException" /> if the target argument is equal to <see langword="default" />.
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is equal to <see langword="default" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument?> IsNullOrDefault<TArgument>(this ValidationTarget<TArgument?> target)
            where TArgument : struct => target.IsNullOrDefault(null);

        /// <summary>
        /// Raises a new <see cref="ArgumentNullException" /> if the target argument is <see langword="null" /> or raises a new
        /// <see cref="ArgumentOutOfRangeException" /> if the target argument is equal to <see langword="default" />.
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is equal to <see langword="default" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument?> IsNullOrDefault<TArgument>(this ValidationTarget<TArgument?> target, String targetParameterName)
            where TArgument : struct
        {
            _ = target.Argument.RejectIf().IsNull(targetParameterName).OrIf().IsEqualToValue(default, targetParameterName);
            return target.Result;
        }
    }
}