// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends the <see cref="ValidationTarget{TArgument}" /> class with validation features for <see cref="Type" /> arguments.
    /// </summary>
    public static class ValidationTargetTypeExtensions
    {
        /// <summary>
        /// Raises a new <see cref="UnsupportedTypeArgumentException" /> if the target argument is not a subclass or implementation
        /// of the specified base type.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="supportedBaseType">
        /// A base type or interface defining the types supported by the invoking method.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="supportedBaseType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// The target argument is not a subclass of <paramref name="supportedBaseType" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<Type> IsNotSupportedType(this ValidationTarget<Type> target, Type supportedBaseType) => target.IsNotSupportedType(supportedBaseType, null);

        /// <summary>
        /// Raises a new <see cref="UnsupportedTypeArgumentException" /> if the target argument is not contained within the specified
        /// collection of supported types.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="supportedTypes">
        /// A collection of types supported by the invoking method.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="supportedTypes" /> does not contain any elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="supportedTypes" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// The target argument is not contained within <paramref name="supportedTypes" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<Type> IsNotSupportedType(this ValidationTarget<Type> target, IEnumerable<Type> supportedTypes) => target.IsNotSupportedType(supportedTypes, null);

        /// <summary>
        /// Raises a new <see cref="UnsupportedTypeArgumentException" /> if the target argument is not a subclass or implementation
        /// of the specified base type.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="supportedBaseType">
        /// A base type or interface defining the types supported by the invoking method.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="supportedBaseType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// The target argument is not a subclass of <paramref name="supportedBaseType" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<Type> IsNotSupportedType(this ValidationTarget<Type> target, Type supportedBaseType, String targetParameterName)
        {
            supportedBaseType.RejectIf().IsNull(nameof(supportedBaseType));

            if (supportedBaseType.GetTypeInfo().IsAssignableFrom(target.Argument))
            {
                return target.Result;
            }

            throw new UnsupportedTypeArgumentException(target.Argument, targetParameterName);
        }

        /// <summary>
        /// Raises a new <see cref="UnsupportedTypeArgumentException" /> if the target argument is not contained within the specified
        /// collection of supported types.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="supportedTypes">
        /// A collection of types supported by the invoking method.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="supportedTypes" /> does not contain any elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="supportedTypes" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// The target argument is not contained within <paramref name="supportedTypes" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<Type> IsNotSupportedType(this ValidationTarget<Type> target, IEnumerable<Type> supportedTypes, String targetParameterName)
        {
            supportedTypes.RejectIf().IsNullOrEmpty(nameof(supportedTypes));

            if (supportedTypes.Contains(target.Argument))
            {
                return target.Result;
            }

            throw new UnsupportedTypeArgumentException(target.Argument, targetParameterName);
        }
    }
}