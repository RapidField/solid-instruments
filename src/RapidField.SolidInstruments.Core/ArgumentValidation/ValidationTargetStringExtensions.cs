// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RapidField.SolidInstruments.Core.ArgumentValidation
{
    /// <summary>
    /// Extends the <see cref="ValidationTarget{TArgument}" /> class with validation features for <see cref="String" /> arguments.
    /// </summary>
    public static class ValidationTargetStringExtensions
    {
        /// <summary>
        /// Raises a new <see cref="StringArgumentPatternException" /> if the specified regular expression pattern does not find a
        /// match in the target argument.
        /// </summary>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="regularExpressionPattern">
        /// A regular expression pattern to search for a match against the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="StringArgumentPatternException">
        /// The specified regular expression pattern did not find a match in the target argument -or- an error occurred while trying
        /// to find a match.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> DoesNotMatchRegularExpression(this ValidationTarget<String> target, String regularExpressionPattern) => target.DoesNotMatchRegularExpression(regularExpressionPattern, null);

        /// <summary>
        /// Raises a new <see cref="StringArgumentPatternException" /> if the specified regular expression pattern does not find a
        /// match in the target argument.
        /// </summary>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="regularExpressionPattern">
        /// A regular expression pattern to search for a match against the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="StringArgumentPatternException">
        /// The specified regular expression pattern did not find a match in the target argument -or- an error occurred while trying
        /// to find a match.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> DoesNotMatchRegularExpression(this ValidationTarget<String> target, String regularExpressionPattern, String targetParameterName)
        {
            try
            {
                if (target.Argument.MatchesRegularExpression(regularExpressionPattern))
                {
                    return target.Result;
                }
            }
            catch (ArgumentException exception)
            {
                throw new StringArgumentPatternException(regularExpressionPattern, targetParameterName, null, exception);
            }
            catch (RegexMatchTimeoutException exception)
            {
                throw new StringArgumentPatternException(regularExpressionPattern, targetParameterName, null, exception);
            }

            throw new StringArgumentPatternException(regularExpressionPattern, targetParameterName);
        }

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument is <see langword="null" /> or empty.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
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
        public static ValidationResult<String> IsNullOrEmpty(this ValidationTarget<String> target) => target.IsNullOrEmpty(null);

        /// <summary>
        /// Raises a new <see cref="ArgumentException" /> if the target argument is <see langword="null" /> or empty.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
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
        public static ValidationResult<String> IsNullOrEmpty(this ValidationTarget<String> target, String targetParameterName)
        {
            if (target.Argument is null)
            {
                throw new ArgumentNullException(targetParameterName);
            }
            else if (target.Argument.Length == 0)
            {
                throw new ArgumentEmptyException(targetParameterName);
            }

            return target.Result;
        }

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the length of the target argument is greater than the
        /// specified boundary.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="exclusiveUpperBoundary">
        /// An exclusive maximum limit to compare with the length of the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the target argument is greater than <paramref name="exclusiveUpperBoundary" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> LengthIsGreaterThan(this ValidationTarget<String> target, Int32 exclusiveUpperBoundary) => target.LengthIsGreaterThan(exclusiveUpperBoundary, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the length of the target argument is greater than the
        /// specified boundary.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="exclusiveUpperBoundary">
        /// An exclusive maximum limit to compare with the length of the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the target argument is greater than <paramref name="exclusiveUpperBoundary" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> LengthIsGreaterThan(this ValidationTarget<String> target, Int32 exclusiveUpperBoundary, String targetParameterName)
        {
            if (target.Argument.Length.CompareTo(exclusiveUpperBoundary) == 1)
            {
                if (targetParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(null, $"The argument cannot have character length greater than {exclusiveUpperBoundary}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot have character length greater than {exclusiveUpperBoundary}.");
            }

            return target.Result;
        }

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the length of the target argument is greater than or equal to
        /// the specified boundary.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="inclusiveUpperBoundary">
        /// An inclusive maximum limit to compare with the length of the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the target argument is greater than or equal to <paramref name="inclusiveUpperBoundary" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> LengthIsGreaterThanOrEqualTo(this ValidationTarget<String> target, Int32 inclusiveUpperBoundary) => target.LengthIsGreaterThanOrEqualTo(inclusiveUpperBoundary, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the length of the target argument is greater than or equal to
        /// the specified boundary.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="inclusiveUpperBoundary">
        /// An inclusive maximum limit to compare with the length of the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the target argument is greater than or equal to <paramref name="inclusiveUpperBoundary" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> LengthIsGreaterThanOrEqualTo(this ValidationTarget<String> target, Int32 inclusiveUpperBoundary, String targetParameterName)
        {
            if (target.Argument?.Length.CompareTo(inclusiveUpperBoundary) != -1)
            {
                if (targetParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(null, $"The argument cannot have character length greater than or equal to {inclusiveUpperBoundary}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot have character length greater than or equal to {inclusiveUpperBoundary}.");
            }

            return target.Result;
        }

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the length of the target argument is less than the specified
        /// boundary.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="exclusiveLowerBoundary">
        /// An exclusive minimum limit to compare with the length of the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the target argument is less than <paramref name="exclusiveLowerBoundary" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> LengthIsLessThan(this ValidationTarget<String> target, Int32 exclusiveLowerBoundary) => target.LengthIsLessThan(exclusiveLowerBoundary, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the length of the target argument is less than the specified
        /// boundary.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="exclusiveLowerBoundary">
        /// An exclusive minimum limit to compare with the length of the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the target argument is less than <paramref name="exclusiveLowerBoundary" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> LengthIsLessThan(this ValidationTarget<String> target, Int32 exclusiveLowerBoundary, String targetParameterName)
        {
            if (target.Argument?.Length.CompareTo(exclusiveLowerBoundary) == -1)
            {
                if (targetParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(null, $"The argument cannot have character length less than {exclusiveLowerBoundary}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot have character length less than {exclusiveLowerBoundary}.");
            }

            return target.Result;
        }

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the length of the target argument is less than or equal to the
        /// specified boundary.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="inclusiveLowerBoundary">
        /// An inclusive minimum limit to compare with the length of the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the target argument is less than or equal to <paramref name="inclusiveLowerBoundary" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> LengthIsLessThanOrEqualTo(this ValidationTarget<String> target, Int32 inclusiveLowerBoundary) => target.LengthIsLessThanOrEqualTo(inclusiveLowerBoundary, null);

        /// <summary>
        /// Raises a new <see cref="ArgumentOutOfRangeException" /> if the length of the target argument is less than or equal to the
        /// specified boundary.
        /// </summary>
        /// <remarks>
        /// This method should be used for argument validation.
        /// </remarks>
        /// <param name="target">
        /// The target argument.
        /// </param>
        /// <param name="inclusiveLowerBoundary">
        /// An inclusive minimum limit to compare with the length of the target argument.
        /// </param>
        /// <param name="targetParameterName">
        /// The name of the method parameter for the target argument.
        /// </param>
        /// <returns>
        /// The result of the validation operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the target argument is less than or equal to <paramref name="inclusiveLowerBoundary" />.
        /// </exception>
        [DebuggerHidden]
        public static ValidationResult<String> LengthIsLessThanOrEqualTo(this ValidationTarget<String> target, Int32 inclusiveLowerBoundary, String targetParameterName)
        {
            if (target.Argument?.Length.CompareTo(inclusiveLowerBoundary) != 1)
            {
                if (targetParameterName is null)
                {
                    throw new ArgumentOutOfRangeException(null, $"The argument cannot have character length less than or equal to {inclusiveLowerBoundary}.");
                }

                throw new ArgumentOutOfRangeException(targetParameterName, $"The argument for {targetParameterName} cannot have character length less than or equal to {inclusiveLowerBoundary}.");
            }

            return target.Result;
        }
    }
}