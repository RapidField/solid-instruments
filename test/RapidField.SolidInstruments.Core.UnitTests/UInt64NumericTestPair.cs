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
    /// Represents an equivalent pair of a <see cref="Number" /> and its underlying <see cref="UInt64" /> which is made available
    /// for testing.
    /// </summary>
    internal sealed class UInt64NumericTestPair : NumericTestPair<UInt64>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UInt64NumericTestPair" /> class.
        /// </summary>
        /// <param name="numericValue">
        /// The primitive numeric data value defining the numeric test pair.
        /// </param>
        [DebuggerHidden]
        internal UInt64NumericTestPair(UInt64 numericValue)
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
        protected sealed override BigRational ConvertValueToBigRational(UInt64 value) => value.ToBigRational();

        /// <summary>
        /// Returns a value indicating whether or not the primitive numeric value for the current
        /// <see cref="UInt64NumericTestPair" /> is equal to and comparatively equivalent to its <see cref="Number" />.
        /// </summary>
        /// <param name="value">
        /// The primitive numeric value.
        /// </param>
        /// <param name="number">
        /// The corresponding <see cref="Number" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the primitive numeric value for the current <see cref="UInt64NumericTestPair" /> is equal to
        /// and comparatively equivalent to its <see cref="Number" />, otherwise <see langword="false" />.
        /// </returns>
        protected sealed override Boolean VerifyStateConsistency(UInt64 value, Number number) => number == value && number.Equals(ConvertValueToBigRational(value)) && number < value is false && number > value is false;

        /// <summary>
        /// Represents a static collection of <see cref="UInt64NumericTestPair" /> objects that are used for testing.
        /// </summary>
        public static readonly IEnumerable<NumericTestPair> Cases = new UInt64NumericTestPair[]
        {
            new(default),
            new(1),
            new(2),
            new(3),
            new(UInt64.MaxValue)
        };
    }
}