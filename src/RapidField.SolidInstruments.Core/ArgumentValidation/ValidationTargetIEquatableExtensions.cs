// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends the <see cref="ValidationTarget{TArgument}" /> class with validation features for <see cref="IEquatable{T}" />
    /// arguments.
    /// </summary>
    public static class ValidationTargetIEquatableExtensions
    {
        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the current object is equal to the specified object.
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
        /// <param name="otherObject">
        /// An object to compare with the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target object is equal to <paramref name="otherObject" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument otherObject)
            where TArgument : class, IEquatable<TArgument> => target.IsEqualTo(otherObject, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the current object is equal to the specified object.
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
        /// <param name="otherObject">
        /// An object to compare with the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The target object is equal to <paramref name="otherObject" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument otherObject, String targetParameterName)
            where TArgument : class, IEquatable<TArgument> => target.IsEqualTo(otherObject, targetParameterName, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the current object is equal to the specified argument.
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
        /// The target object is equal to <paramref name="otherArgument" />.
        /// </exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValidationResult<TArgument> IsEqualTo<TArgument>(this ValidationTarget<TArgument> target, TArgument otherArgument, String targetParameterName, String otherParameterName)
            where TArgument : class, IEquatable<TArgument>
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