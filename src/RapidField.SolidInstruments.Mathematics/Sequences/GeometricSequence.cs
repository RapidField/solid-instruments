// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Mathematics.Sequences
{
    /// <summary>
    /// Represents a unidirectional sequence of numbers for which each term is found by multiplying the previous term by a constant
    /// factor.
    /// </summary>
    /// <remarks>
    /// <see cref="GeometricSequence" /> is the default implementation of <see cref="IGeometricSequence" />.
    /// </remarks>
    public class GeometricSequence : InfiniteSequence<Double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricSequence" /> class.
        /// </summary>
        /// <param name="firstTerm">
        /// The first number in the sequence. The argument cannot be zero.
        /// </param>
        /// <param name="commonRatio">
        /// The constant factor between the terms in the sequence. The argument cannot be one and cannot be less than or equal to
        /// zero.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="firstTerm" /> is zero -or- <paramref name="commonRatio" /> is less than or equal to zero, or
        /// <paramref name="commonRatio" /> is one.
        /// </exception>
        public GeometricSequence(Double firstTerm, Double commonRatio)
            : base(firstTerm.RejectIf().IsEqualToValue(0d, nameof(firstTerm)))
        {
            CommonRatio = commonRatio.RejectIf().IsEqualToValue(1d, nameof(commonRatio)).OrIf().IsLessThanOrEqualTo(0d, nameof(commonRatio));
        }

        /// <summary>
        /// Calculates the next term in the sequence.
        /// </summary>
        /// <param name="calculatedTerms">
        /// The ordered, calculated terms for the current <see cref="GeometricSequence" />.
        /// </param>
        /// <returns>
        /// The next term in the sequence.
        /// </returns>
        protected override Double CalculateNext(IEnumerable<Double> calculatedTerms) => calculatedTerms.Last() * CommonRatio;

        /// <summary>
        /// Gets the constant factor between the terms in the sequence.
        /// </summary>
        public Double CommonRatio
        {
            get;
        }
    }
}