// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Mathematics.Extensions
{
    /// <summary>
    /// Extends the <see cref="Int32" /> and <see cref="Int64" /> structures with mathematics features.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Returns the absolute value of the current <see cref="Int16" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int16" />.
        /// </param>
        /// <returns>
        /// The absolute value of the current <see cref="Int16" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The current value is equal to <see cref="Int16.MinValue" />.
        /// </exception>
        public static Int16 AbsoluteValue(this Int16 target) => Math.Abs(target);

        /// <summary>
        /// Returns the absolute value of the current <see cref="Int32" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int32" />.
        /// </param>
        /// <returns>
        /// The absolute value of the current <see cref="Int32" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The current value is equal to <see cref="Int32.MinValue" />.
        /// </exception>
        public static Int32 AbsoluteValue(this Int32 target) => Math.Abs(target);

        /// <summary>
        /// Returns the absolute value of the current <see cref="Int64" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int64" />.
        /// </param>
        /// <returns>
        /// The absolute value of the current <see cref="Int64" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The current value is equal to <see cref="Int64.MinValue" />.
        /// </exception>
        public static Int64 AbsoluteValue(this Int64 target) => Math.Abs(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int16" /> is a factor of the specified multiple.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int16" />.
        /// </param>
        /// <param name="multiple">
        /// The multiple.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int16" /> is a factor of <paramref name="multiple" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsAFactorOf(this Int16 target, Int16 multiple) => SignedIntegerIsAFactorOf(target, multiple);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int32" /> is a factor of the specified multiple.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int32" />.
        /// </param>
        /// <param name="multiple">
        /// The multiple.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int32" /> is a factor of <paramref name="multiple" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsAFactorOf(this Int32 target, Int32 multiple) => SignedIntegerIsAFactorOf(target, multiple);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int64" /> is a factor of the specified multiple.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int64" />.
        /// </param>
        /// <param name="multiple">
        /// The multiple.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int64" /> is a factor of <paramref name="multiple" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsAFactorOf(this Int64 target, Int64 multiple) => SignedIntegerIsAFactorOf(target, multiple);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt16" /> is a factor of the specified multiple.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt16" />.
        /// </param>
        /// <param name="multiple">
        /// The multiple.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt16" /> is a factor of <paramref name="multiple" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsAFactorOf(this UInt16 target, UInt16 multiple) => UnsignedIntegerIsAFactorOf(target, multiple);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt32" /> is a factor of the specified multiple.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt32" />.
        /// </param>
        /// <param name="multiple">
        /// The multiple.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt32" /> is a factor of <paramref name="multiple" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsAFactorOf(this UInt32 target, UInt32 multiple) => UnsignedIntegerIsAFactorOf(target, multiple);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt64" /> is a factor of the specified multiple.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt64" />.
        /// </param>
        /// <param name="multiple">
        /// The multiple.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt64" /> is a factor of <paramref name="multiple" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsAFactorOf(this UInt64 target, UInt64 multiple) => UnsignedIntegerIsAFactorOf(target, multiple);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int16" /> is divisible by the specified divisor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int16" />.
        /// </param>
        /// <param name="divisor">
        /// The divisor.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int16" /> is divisible by <paramref name="divisor" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsDivisibleBy(this Int16 target, Int16 divisor) => SignedIntegerIsDivisibleBy(target, divisor);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int32" /> is divisible by the specified divisor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int32" />.
        /// </param>
        /// <param name="divisor">
        /// The divisor.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int32" /> is divisible by <paramref name="divisor" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsDivisibleBy(this Int32 target, Int32 divisor) => SignedIntegerIsDivisibleBy(target, divisor);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int64" /> is divisible by the specified divisor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int64" />.
        /// </param>
        /// <param name="divisor">
        /// The divisor.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int64" /> is divisible by <paramref name="divisor" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsDivisibleBy(this Int64 target, Int64 divisor) => SignedIntegerIsDivisibleBy(target, divisor);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt16" /> is divisible by the specified divisor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt16" />.
        /// </param>
        /// <param name="divisor">
        /// The divisor.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt16" /> is divisible by <paramref name="divisor" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsDivisibleBy(this UInt16 target, UInt16 divisor) => UnsignedIntegerIsDivisibleBy(target, divisor);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt32" /> is divisible by the specified divisor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt32" />.
        /// </param>
        /// <param name="divisor">
        /// The divisor.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt32" /> is divisible by <paramref name="divisor" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsDivisibleBy(this UInt32 target, UInt32 divisor) => UnsignedIntegerIsDivisibleBy(target, divisor);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt64" /> is divisible by the specified divisor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt64" />.
        /// </param>
        /// <param name="divisor">
        /// The divisor.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt64" /> is divisible by <paramref name="divisor" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsDivisibleBy(this UInt64 target, UInt64 divisor) => UnsignedIntegerIsDivisibleBy(target, divisor);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int16" /> is an even number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int16" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int16" /> is an even number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsEven(this Int16 target) => SignedIntegerIsEven(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int32" /> is an even number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int32" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int32" /> is an even number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsEven(this Int32 target) => SignedIntegerIsEven(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int64" /> is an even number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int64" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int64" /> is an even number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsEven(this Int64 target) => SignedIntegerIsEven(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt16" /> is an even number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt16" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt16" /> is an even number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsEven(this UInt16 target) => UnsignedIntegerIsEven(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt32" /> is an even number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt32" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt32" /> is an even number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsEven(this UInt32 target) => UnsignedIntegerIsEven(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt64" /> is an even number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt64" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt64" /> is an even number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsEven(this UInt64 target) => UnsignedIntegerIsEven(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int16" /> is an odd number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int16" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int16" /> is an odd number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsOdd(this Int16 target) => SignedIntegerIsOdd(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int32" /> is an odd number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int32" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int32" /> is an odd number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsOdd(this Int32 target) => SignedIntegerIsOdd(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="Int64" /> is an odd number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int64" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Int64" /> is an odd number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsOdd(this Int64 target) => SignedIntegerIsOdd(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt16" /> is an odd number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt16" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt16" /> is an odd number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsOdd(this UInt16 target) => UnsignedIntegerIsOdd(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt32" /> is an odd number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt32" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt32" /> is an odd number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsOdd(this UInt32 target) => UnsignedIntegerIsOdd(target);

        /// <summary>
        /// Indicates whether or not the current <see cref="UInt64" /> is an odd number.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt64" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="UInt64" /> is an odd number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsOdd(this UInt64 target) => UnsignedIntegerIsOdd(target);

        /// <summary>
        /// Returns the position of the current <see cref="Int16" /> within the specified range, expressed as a percentage where
        /// zero is equal to the lower boundary and one is equal to the upper boundary.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int16" />.
        /// </param>
        /// <param name="lowerBoundary">
        /// The inclusive lower boundary of the range to evaluate.
        /// </param>
        /// <param name="upperBoundary">
        /// The inclusive upper boundary of the range to evaluate.
        /// </param>
        /// <returns>
        /// The position of the current <see cref="Int16" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The current value is less than <paramref name="lowerBoundary" /> -or- the current value is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="lowerBoundary" /> is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="upperBoundary" /> is equal to <paramref name="lowerBoundary" />.
        /// </exception>
        public static Double PositionInRange(this Int16 target, Int16 lowerBoundary, Int16 upperBoundary)
        {
            target = target.RejectIf().IsLessThan(lowerBoundary, nameof(lowerBoundary)).OrIf().IsGreaterThan(upperBoundary, nameof(upperBoundary));
            lowerBoundary = lowerBoundary.RejectIf().IsGreaterThan(upperBoundary, nameof(lowerBoundary));
            upperBoundary = upperBoundary.RejectIf().IsEqualToValue(lowerBoundary, nameof(upperBoundary));
            return (Convert.ToDouble(target - lowerBoundary) / Convert.ToDouble(upperBoundary - lowerBoundary));
        }

        /// <summary>
        /// Returns the position of the current <see cref="Int32" /> within the specified range, expressed as a percentage where
        /// zero is equal to the lower boundary and one is equal to the upper boundary.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int32" />.
        /// </param>
        /// <param name="lowerBoundary">
        /// The inclusive lower boundary of the range to evaluate.
        /// </param>
        /// <param name="upperBoundary">
        /// The inclusive upper boundary of the range to evaluate.
        /// </param>
        /// <returns>
        /// The position of the current <see cref="Int32" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The current value is less than <paramref name="lowerBoundary" /> -or- the current value is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="lowerBoundary" /> is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="upperBoundary" /> is equal to <paramref name="lowerBoundary" />.
        /// </exception>
        public static Double PositionInRange(this Int32 target, Int32 lowerBoundary, Int32 upperBoundary)
        {
            target = target.RejectIf().IsLessThan(lowerBoundary, nameof(lowerBoundary)).OrIf().IsGreaterThan(upperBoundary, nameof(upperBoundary));
            lowerBoundary = lowerBoundary.RejectIf().IsGreaterThan(upperBoundary, nameof(lowerBoundary));
            upperBoundary = upperBoundary.RejectIf().IsEqualToValue(lowerBoundary, nameof(upperBoundary));
            return (Convert.ToDouble(target - lowerBoundary) / Convert.ToDouble(upperBoundary - lowerBoundary));
        }

        /// <summary>
        /// Returns the position of the current <see cref="Int64" /> within the specified range, expressed as a percentage where
        /// zero is equal to the lower boundary and one is equal to the upper boundary.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int64" />.
        /// </param>
        /// <param name="lowerBoundary">
        /// The inclusive lower boundary of the range to evaluate.
        /// </param>
        /// <param name="upperBoundary">
        /// The inclusive upper boundary of the range to evaluate.
        /// </param>
        /// <returns>
        /// The position of the current <see cref="Int64" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The current value is less than <paramref name="lowerBoundary" /> -or- the current value is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="lowerBoundary" /> is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="upperBoundary" /> is equal to <paramref name="lowerBoundary" />.
        /// </exception>
        public static Double PositionInRange(this Int64 target, Int64 lowerBoundary, Int64 upperBoundary)
        {
            target = target.RejectIf().IsLessThan(lowerBoundary, nameof(lowerBoundary)).OrIf().IsGreaterThan(upperBoundary, nameof(upperBoundary));
            lowerBoundary = lowerBoundary.RejectIf().IsGreaterThan(upperBoundary, nameof(lowerBoundary));
            upperBoundary = upperBoundary.RejectIf().IsEqualToValue(lowerBoundary, nameof(upperBoundary));
            return (Convert.ToDouble(target - lowerBoundary) / Convert.ToDouble(upperBoundary - lowerBoundary));
        }

        /// <summary>
        /// Returns the position of the current <see cref="UInt16" /> within the specified range, expressed as a percentage where
        /// zero is equal to the lower boundary and one is equal to the upper boundary.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt16" />.
        /// </param>
        /// <param name="lowerBoundary">
        /// The inclusive lower boundary of the range to evaluate.
        /// </param>
        /// <param name="upperBoundary">
        /// The inclusive upper boundary of the range to evaluate.
        /// </param>
        /// <returns>
        /// The position of the current <see cref="UInt16" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The current value is less than <paramref name="lowerBoundary" /> -or- the current value is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="lowerBoundary" /> is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="upperBoundary" /> is equal to <paramref name="lowerBoundary" />.
        /// </exception>
        public static Double PositionInRange(this UInt16 target, UInt16 lowerBoundary, UInt16 upperBoundary)
        {
            target = target.RejectIf().IsLessThan(lowerBoundary, nameof(lowerBoundary)).OrIf().IsGreaterThan(upperBoundary, nameof(upperBoundary));
            lowerBoundary = lowerBoundary.RejectIf().IsGreaterThan(upperBoundary, nameof(lowerBoundary));
            upperBoundary = upperBoundary.RejectIf().IsEqualToValue(lowerBoundary, nameof(upperBoundary));
            return (Convert.ToDouble(target - lowerBoundary) / Convert.ToDouble(upperBoundary - lowerBoundary));
        }

        /// <summary>
        /// Returns the position of the current <see cref="UInt32" /> within the specified range, expressed as a percentage where
        /// zero is equal to the lower boundary and one is equal to the upper boundary.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt32" />.
        /// </param>
        /// <param name="lowerBoundary">
        /// The inclusive lower boundary of the range to evaluate.
        /// </param>
        /// <param name="upperBoundary">
        /// The inclusive upper boundary of the range to evaluate.
        /// </param>
        /// <returns>
        /// The position of the current <see cref="UInt32" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The current value is less than <paramref name="lowerBoundary" /> -or- the current value is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="lowerBoundary" /> is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="upperBoundary" /> is equal to <paramref name="lowerBoundary" />.
        /// </exception>
        public static Double PositionInRange(this UInt32 target, UInt32 lowerBoundary, UInt32 upperBoundary)
        {
            target = target.RejectIf().IsLessThan(lowerBoundary, nameof(lowerBoundary)).OrIf().IsGreaterThan(upperBoundary, nameof(upperBoundary));
            lowerBoundary = lowerBoundary.RejectIf().IsGreaterThan(upperBoundary, nameof(lowerBoundary));
            upperBoundary = upperBoundary.RejectIf().IsEqualToValue(lowerBoundary, nameof(upperBoundary));
            return (Convert.ToDouble(target - lowerBoundary) / Convert.ToDouble(upperBoundary - lowerBoundary));
        }

        /// <summary>
        /// Returns the position of the current <see cref="UInt64" /> within the specified range, expressed as a percentage where
        /// zero is equal to the lower boundary and one is equal to the upper boundary.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt64" />.
        /// </param>
        /// <param name="lowerBoundary">
        /// The inclusive lower boundary of the range to evaluate.
        /// </param>
        /// <param name="upperBoundary">
        /// The inclusive upper boundary of the range to evaluate.
        /// </param>
        /// <returns>
        /// The position of the current <see cref="UInt64" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The current value is less than <paramref name="lowerBoundary" /> -or- the current value is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="lowerBoundary" /> is greater than
        /// <paramref name="upperBoundary" /> -or- <paramref name="upperBoundary" /> is equal to <paramref name="lowerBoundary" />.
        /// </exception>
        public static Double PositionInRange(this UInt64 target, UInt64 lowerBoundary, UInt64 upperBoundary)
        {
            target = target.RejectIf().IsLessThan(lowerBoundary, nameof(lowerBoundary)).OrIf().IsGreaterThan(upperBoundary, nameof(upperBoundary));
            lowerBoundary = lowerBoundary.RejectIf().IsGreaterThan(upperBoundary, nameof(lowerBoundary));
            upperBoundary = upperBoundary.RejectIf().IsEqualToValue(lowerBoundary, nameof(upperBoundary));
            return (Convert.ToDouble(target - lowerBoundary) / Convert.ToDouble(upperBoundary - lowerBoundary));
        }

        /// <summary>
        /// Rounds the current <see cref="Int16" /> to the nearest multiple of the specified factor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int16" />.
        /// </param>
        /// <param name="factor">
        /// The factor to use for the rounding operation.
        /// </param>
        /// <param name="mode">
        /// Specifies the behavior for the rounding operation. The default value is
        /// <see cref="FactorRoundingMode.InwardOrOutward" />.
        /// </param>
        /// <returns>
        /// The nearest multiple of the specified factor.
        /// </returns>
        public static Int16 RoundedToFactor(this Int16 target, Int16 factor, FactorRoundingMode mode)
        {
            mode.RejectIf().IsEqualToValue(FactorRoundingMode.Unspecified, nameof(mode));

            return mode switch
            {
                FactorRoundingMode.InwardOrOutward => Convert.ToInt16(Math.Round((target / Convert.ToDouble(factor)), MidpointRounding.AwayFromZero) * factor),
                FactorRoundingMode.InwardOnly => Convert.ToInt16(target - (target % factor)),
                FactorRoundingMode.OutwardOnly => Convert.ToInt16((target % factor == 0) ? target : ((factor - (target % factor)) + target)),
                _ => throw new UnsupportedSpecificationException($"The specified factor rounding mode, {mode}, is not supported.")
            };
        }

        /// <summary>
        /// Rounds the current <see cref="UInt16" /> to the nearest multiple of the specified factor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt16" />.
        /// </param>
        /// <param name="factor">
        /// The factor to use for the rounding operation.
        /// </param>
        /// <param name="mode">
        /// Specifies the behavior for the rounding operation. The default value is
        /// <see cref="FactorRoundingMode.InwardOrOutward" />.
        /// </param>
        /// <returns>
        /// The nearest multiple of the specified factor.
        /// </returns>
        public static UInt16 RoundedToFactor(this UInt16 target, UInt16 factor, FactorRoundingMode mode)
        {
            mode.RejectIf().IsEqualToValue(FactorRoundingMode.Unspecified, nameof(mode));

            return mode switch
            {
                FactorRoundingMode.InwardOrOutward => Convert.ToUInt16(Math.Round((target / Convert.ToDouble(factor)), MidpointRounding.AwayFromZero) * factor),
                FactorRoundingMode.InwardOnly => Convert.ToUInt16(target - (target % factor)),
                FactorRoundingMode.OutwardOnly => Convert.ToUInt16((target % factor == 0) ? target : ((factor - (target % factor)) + target)),
                _ => throw new UnsupportedSpecificationException($"The specified factor rounding mode, {mode}, is not supported.")
            };
        }

        /// <summary>
        /// Rounds the current <see cref="Int32" /> to the nearest multiple of the specified factor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int32" />.
        /// </param>
        /// <param name="factor">
        /// The factor to use for the rounding operation.
        /// </param>
        /// <param name="mode">
        /// Specifies the behavior for the rounding operation. The default value is
        /// <see cref="FactorRoundingMode.InwardOrOutward" />.
        /// </param>
        /// <returns>
        /// The nearest multiple of the specified factor.
        /// </returns>
        public static Int32 RoundedToFactor(this Int32 target, Int32 factor, FactorRoundingMode mode)
        {
            mode.RejectIf().IsEqualToValue(FactorRoundingMode.Unspecified, nameof(mode));

            return mode switch
            {
                FactorRoundingMode.InwardOrOutward => Convert.ToInt32(Math.Round((target / Convert.ToDouble(factor)), MidpointRounding.AwayFromZero) * factor),
                FactorRoundingMode.InwardOnly => (target - (target % factor)),
                FactorRoundingMode.OutwardOnly => ((target % factor == 0) ? target : ((factor - (target % factor)) + target)),
                _ => throw new UnsupportedSpecificationException($"The specified factor rounding mode, {mode}, is not supported.")
            };
        }

        /// <summary>
        /// Rounds the current <see cref="UInt32" /> to the nearest multiple of the specified factor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt32" />.
        /// </param>
        /// <param name="factor">
        /// The factor to use for the rounding operation.
        /// </param>
        /// <param name="mode">
        /// Specifies the behavior for the rounding operation. The default value is
        /// <see cref="FactorRoundingMode.InwardOrOutward" />.
        /// </param>
        /// <returns>
        /// The nearest multiple of the specified factor.
        /// </returns>
        public static UInt32 RoundedToFactor(this UInt32 target, UInt32 factor, FactorRoundingMode mode)
        {
            mode.RejectIf().IsEqualToValue(FactorRoundingMode.Unspecified, nameof(mode));

            return mode switch
            {
                FactorRoundingMode.InwardOrOutward => Convert.ToUInt32(Math.Round((target / Convert.ToDouble(factor)), MidpointRounding.AwayFromZero) * factor),
                FactorRoundingMode.InwardOnly => (target - (target % factor)),
                FactorRoundingMode.OutwardOnly => (target % factor == 0) ? target : ((factor - (target % factor)) + target),
                _ => throw new UnsupportedSpecificationException($"The specified factor rounding mode, {mode}, is not supported.")
            };
        }

        /// <summary>
        /// Rounds the current <see cref="Int64" /> to the nearest multiple of the specified factor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Int64" />.
        /// </param>
        /// <param name="factor">
        /// The factor to use for the rounding operation.
        /// </param>
        /// <param name="mode">
        /// Specifies the behavior for the rounding operation. The default value is
        /// <see cref="FactorRoundingMode.InwardOrOutward" />.
        /// </param>
        /// <returns>
        /// The nearest multiple of the specified factor.
        /// </returns>
        public static Int64 RoundedToFactor(this Int64 target, Int64 factor, FactorRoundingMode mode)
        {
            mode.RejectIf().IsEqualToValue(FactorRoundingMode.Unspecified, nameof(mode));

            return mode switch
            {
                FactorRoundingMode.InwardOrOutward => Convert.ToInt64(Math.Round((target / Convert.ToDouble(factor)), MidpointRounding.AwayFromZero) * factor),
                FactorRoundingMode.InwardOnly => Convert.ToInt64(target - (target % factor)),
                FactorRoundingMode.OutwardOnly => Convert.ToInt64((target % factor == 0) ? target : ((factor - (target % factor)) + target)),
                _ => throw new UnsupportedSpecificationException($"The specified factor rounding mode, {mode}, is not supported.")
            };
        }

        /// <summary>
        /// Rounds the current <see cref="UInt64" /> to the nearest multiple of the specified factor.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="UInt64" />.
        /// </param>
        /// <param name="factor">
        /// The factor to use for the rounding operation.
        /// </param>
        /// <param name="mode">
        /// Specifies the behavior for the rounding operation. The default value is
        /// <see cref="FactorRoundingMode.InwardOrOutward" />.
        /// </param>
        /// <returns>
        /// The nearest multiple of the specified factor.
        /// </returns>
        public static UInt64 RoundedToFactor(this UInt64 target, UInt64 factor, FactorRoundingMode mode)
        {
            mode.RejectIf().IsEqualToValue(FactorRoundingMode.Unspecified, nameof(mode));

            return mode switch
            {
                FactorRoundingMode.InwardOrOutward => Convert.ToUInt64(Math.Round((target / Convert.ToDouble(factor)), MidpointRounding.AwayFromZero) * factor),
                FactorRoundingMode.InwardOnly => Convert.ToUInt64(target - (target % factor)),
                FactorRoundingMode.OutwardOnly => Convert.ToUInt64((target % factor == 0) ? target : ((factor - (target % factor)) + target)),
                _ => throw new UnsupportedSpecificationException($"The specified factor rounding mode, {mode}, is not supported.")
            };
        }

        /// <summary>
        /// Indicates whether or not the provided <see cref="Int64" /> is a factor of the specified multiple.
        /// </summary>
        /// <param name="target">
        /// A <see cref="Int64" /> value.
        /// </param>
        /// <param name="multiple">
        /// The multiple.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided <see cref="Int64" /> is a factor of <paramref name="multiple" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean SignedIntegerIsAFactorOf(Int64 target, Int64 multiple) => multiple.IsDivisibleBy(target);

        /// <summary>
        /// Indicates whether or not the provided <see cref="Int64" /> is divisible by the specified divisor.
        /// </summary>
        /// <param name="target">
        /// A <see cref="Int64" /> value.
        /// </param>
        /// <param name="divisor">
        /// The divisor.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided <see cref="Int64" /> is divisible by the <paramref name="divisor" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean SignedIntegerIsDivisibleBy(Int64 target, Int64 divisor) => ((divisor != 0) && (target % divisor == 0));

        /// <summary>
        /// Indicates whether or not the provided <see cref="Int64" /> is an even number.
        /// </summary>
        /// <param name="target">
        /// A <see cref="Int64" /> value.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided <see cref="Int64" /> is an even number, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean SignedIntegerIsEven(Int64 target) => ((target & 0x01) == 0x00);

        /// <summary>
        /// Indicates whether or not the provided <see cref="Int64" /> is an odd number.
        /// </summary>
        /// <param name="target">
        /// A <see cref="Int64" /> value.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided <see cref="Int64" /> is an odd number, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean SignedIntegerIsOdd(Int64 target) => ((target & 0x01) == 0x01);

        /// <summary>
        /// Indicates whether or not the provided <see cref="UInt64" /> is a factor of the specified multiple.
        /// </summary>
        /// <param name="target">
        /// A <see cref="UInt64" /> value.
        /// </param>
        /// <param name="multiple">
        /// The multiple.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided <see cref="UInt64" /> is a factor of <paramref name="multiple" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean UnsignedIntegerIsAFactorOf(UInt64 target, UInt64 multiple) => multiple.IsDivisibleBy(target);

        /// <summary>
        /// Indicates whether or not the provided <see cref="UInt64" /> is divisible by the specified divisor.
        /// </summary>
        /// <param name="target">
        /// A <see cref="UInt64" /> value.
        /// </param>
        /// <param name="divisor">
        /// The divisor.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided <see cref="UInt64" /> is divisible by <paramref name="divisor" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean UnsignedIntegerIsDivisibleBy(UInt64 target, UInt64 divisor) => ((divisor != 0) && (target % divisor == 0));

        /// <summary>
        /// Indicates whether or not the provided <see cref="UInt64" /> is an even number.
        /// </summary>
        /// <param name="target">
        /// A <see cref="UInt64" /> value.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided <see cref="UInt64" /> is an even number, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean UnsignedIntegerIsEven(UInt64 target) => ((target & 0x01) == 0x00);

        /// <summary>
        /// Indicates whether or not the provided <see cref="UInt64" /> is an odd number.
        /// </summary>
        /// <param name="target">
        /// A <see cref="UInt64" /> value.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided <see cref="UInt64" /> is an odd number, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean UnsignedIntegerIsOdd(UInt64 target) => ((target & 0x01) == 0x01);
    }
}