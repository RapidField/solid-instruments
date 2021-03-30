// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    /// <summary>
    /// Represents an equivalent pair of a <see cref="Number" /> and its underlying <see cref="Decimal" /> which is made available
    /// for testing.
    /// </summary>
    internal sealed class DecimalNumericTestPair : NumericTestPair<Decimal>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalNumericTestPair" /> class.
        /// </summary>
        /// <param name="numericValue">
        /// The primitive numeric data value defining the numeric test pair.
        /// </param>
        [DebuggerHidden]
        internal DecimalNumericTestPair(Decimal numericValue)
            : base(numericValue)
        {
            return;
        }

        /// <summary>
        /// Converts the specified numeric value to its <see cref="BigRational" /> equivalent.
        /// </summary>
        /// <param name="value">
        /// The numeric value to convert.
        /// </param>
        /// <returns>
        /// The <see cref="BigRational" /> equivalent of the specified numeric value.
        /// </returns>
        protected sealed override BigRational ConvertValueToBigRational(Decimal value) => value.ToBigRational();

        /// <summary>
        /// Returns a value indicating whether or not the primitive numeric value for the current
        /// <see cref="DecimalNumericTestPair" /> is equal to and comparatively equivalent to its <see cref="Number" />.
        /// </summary>
        /// <param name="value">
        /// The primitive numeric value.
        /// </param>
        /// <param name="number">
        /// The corresponding <see cref="Number" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the primitive numeric value for the current <see cref="DecimalNumericTestPair" /> is equal to
        /// and comparatively equivalent to its <see cref="Number" />, otherwise <see langword="false" />.
        /// </returns>
        protected sealed override Boolean VerifyStateConsistency(Decimal value, Number number) =>
            number == value &&
            number.Equals(ConvertValueToBigRational(value)) &&
            number < value is false &&
            number > value is false &&
            number.IsFractional == value.IsFractional() &&
            number.IsInteger == value.IsInteger() &&
            number.IsNegative == (value < 0m) &&
            number.IsPositive == (value > 0m) &&
            number.AbsoluteValue() == (number.IsPositive ? number : -number) &&
            number.CountSignificantFigures() == value.CountSignificantFigures();

        /// <summary>
        /// Represents a static collection of <see cref="DecimalNumericTestPair" /> objects that are used for testing.
        /// </summary>
        public static readonly IEnumerable<NumericTestPair> Cases = new DecimalNumericTestPair[]
        {
            new(-3m),
            new(-2m),
            new(-1m),
            new(default),
            new(1m),
            new(2m),
            new(3m)
        };
    }
}