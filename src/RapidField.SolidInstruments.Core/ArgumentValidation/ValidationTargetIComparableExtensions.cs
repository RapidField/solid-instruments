// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends the <see cref="ValidationTarget{TArgument}" /> class with validation features for <see cref="IComparable{T}" />
    /// arguments.
    /// </summary>
    public static class ValidationTargetIComparableExtensions
    {
        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is greater than the specified boundary.
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
        /// <param name="exclusiveUpperBoundary">
        /// An object to compare with the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is greater than <paramref name="exclusiveUpperBoundary" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsGreaterThan<TArgument>(this ValidationTarget<TArgument> target, TArgument exclusiveUpperBoundary)
            where TArgument : IComparable<TArgument> => target.IsGreaterThan(exclusiveUpperBoundary, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is greater than the specified boundary.
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
        /// <param name="exclusiveUpperBoundary">
        /// An object to compare with the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is greater than <paramref name="exclusiveUpperBoundary" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsGreaterThan<TArgument>(this ValidationTarget<TArgument> target, TArgument exclusiveUpperBoundary, String targetParameterName)
            where TArgument : IComparable<TArgument> => target.IsGreaterThan(exclusiveUpperBoundary, targetParameterName, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is greater than the specified argument.
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
        /// The target argument is greater than <paramref name="otherArgument" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsGreaterThan<TArgument>(this ValidationTarget<TArgument> target, TArgument otherArgument, String targetParameterName, String otherParameterName)
            where TArgument : IComparable<TArgument>
        {
            if (target.Argument.CompareTo(otherArgument) == 1)
            {
                if (targetParameterName is null)
                {
                    if (otherParameterName is null)
                    {
                        throw new ArgumentOutOfRangeException(null, $"The argument cannot be greater than {otherArgument}.");
                    }

                    throw new ArgumentOutOfRangeException(null, $"The argument cannot be greater than the argument for {otherParameterName}.");
                }
                else if (otherParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be greater than {otherArgument}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be greater than the argument for {otherParameterName}.");
            }

            return target.Result;
        }

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is greater than or equal to the specified
        /// boundary.
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
        /// <param name="inclusiveUpperBoundary">
        /// An object to compare with the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is greater than or equal to <paramref name="inclusiveUpperBoundary" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsGreaterThanOrEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument inclusiveUpperBoundary)
            where TArgument : IComparable<TArgument> => target.IsGreaterThanOrEqualTo(inclusiveUpperBoundary, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is greater than or equal to the specified
        /// boundary.
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
        /// <param name="inclusiveUpperBoundary">
        /// An object to compare with the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is greater than or equal to <paramref name="inclusiveUpperBoundary" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsGreaterThanOrEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument inclusiveUpperBoundary, String targetParameterName)
            where TArgument : IComparable<TArgument> => target.IsGreaterThanOrEqualTo(inclusiveUpperBoundary, targetParameterName, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is greater than or equal to the specified
        /// argument.
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
        /// The target argument is greater than or equal to <paramref name="otherArgument" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsGreaterThanOrEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument otherArgument, String targetParameterName, String otherParameterName)
            where TArgument : IComparable<TArgument>
        {
            if (target.Argument.CompareTo(otherArgument) != -1)
            {
                if (targetParameterName is null)
                {
                    if (otherParameterName is null)
                    {
                        throw new ArgumentOutOfRangeException(null, $"The argument cannot be greater than or equal to {otherArgument}.");
                    }

                    throw new ArgumentOutOfRangeException(null, $"The argument cannot be greater than or equal to the argument for {otherParameterName}.");
                }
                else if (otherParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be greater than or equal to {otherArgument}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be greater than or equal to the argument for {otherParameterName}.");
            }

            return target.Result;
        }

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is less than the specified boundary.
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
        /// <param name="exclusiveLowerBoundary">
        /// An object to compare with the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is less than <paramref name="exclusiveLowerBoundary" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsLessThan<TArgument>(this ValidationTarget<TArgument> target, TArgument exclusiveLowerBoundary)
            where TArgument : IComparable<TArgument> => target.IsLessThan(exclusiveLowerBoundary, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is less than the specified boundary.
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
        /// <param name="exclusiveLowerBoundary">
        /// An object to compare with the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is less than <paramref name="exclusiveLowerBoundary" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsLessThan<TArgument>(this ValidationTarget<TArgument> target, TArgument exclusiveLowerBoundary, String targetParameterName)
            where TArgument : IComparable<TArgument> => target.IsLessThan(exclusiveLowerBoundary, targetParameterName, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is less than the specified argument.
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
        /// The target argument is less than <paramref name="otherArgument" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsLessThan<TArgument>(this ValidationTarget<TArgument> target, TArgument otherArgument, String targetParameterName, String otherParameterName)
            where TArgument : IComparable<TArgument>
        {
            if (target.Argument.CompareTo(otherArgument) == -1)
            {
                if (targetParameterName is null)
                {
                    if (otherParameterName is null)
                    {
                        throw new ArgumentOutOfRangeException(null, $"The argument cannot be less than {otherArgument}.");
                    }

                    throw new ArgumentOutOfRangeException(null, $"The argument cannot be less than the argument for {otherParameterName}.");
                }
                else if (otherParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be less than {otherArgument}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be less than the argument for {otherParameterName}.");
            }

            return target.Result;
        }

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is less than or equal to the specified
        /// boundary.
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
        /// <param name="inclusiveLowerBoundary">
        /// An object to compare with the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is less than or equal to <paramref name="inclusiveLowerBoundary" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsLessThanOrEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument inclusiveLowerBoundary)
            where TArgument : IComparable<TArgument> => target.IsLessThanOrEqualTo(inclusiveLowerBoundary, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is less than or equal to the specified
        /// boundary.
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
        /// <param name="inclusiveLowerBoundary">
        /// An object to compare with the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target argument is less than or equal to <paramref name="inclusiveLowerBoundary" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsLessThanOrEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument inclusiveLowerBoundary, String targetParameterName)
            where TArgument : IComparable<TArgument> => target.IsLessThanOrEqualTo(inclusiveLowerBoundary, targetParameterName, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the target argument is less than or equal to the specified
        /// argument.
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
        /// The target argument is less than or equal to <paramref name="otherArgument" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsLessThanOrEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument otherArgument, String targetParameterName, String otherParameterName)
            where TArgument : IComparable<TArgument>
        {
            if (target.Argument.CompareTo(otherArgument) != 1)
            {
                if (targetParameterName is null)
                {
                    if (otherParameterName is null)
                    {
                        throw new ArgumentOutOfRangeException(null, $"The argument cannot be less than or equal to {otherArgument}.");
                    }

                    throw new ArgumentOutOfRangeException(null, $"The argument cannot be less than or equal to the argument for {otherParameterName}.");
                }
                else if (otherParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be less than or equal to {otherArgument}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot be less than or equal to the argument for {otherParameterName}.");
            }

            return target.Result;
        }
    }
}