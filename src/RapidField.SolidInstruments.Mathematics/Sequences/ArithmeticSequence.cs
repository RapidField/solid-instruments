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
    /// Represents a unidirectional sequence of numbers that are equidistant at each step.
    /// </summary>
    /// <remarks>
    /// <see cref="ArithmeticSequence" /> is the default implementation of <see cref="IArithmeticSequence" />.
    /// </remarks>
    public class ArithmeticSequence : InfiniteSequence<Double>, IArithmeticSequence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArithmeticSequence" /> class.
        /// </summary>
        /// <param name="firstTerm">
        /// The first term in the sequence.
        /// </param>
        /// <param name="commonDifference">
        /// The constant difference between the terms in the sequence.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="commonDifference" /> is zero.
        /// </exception>
        public ArithmeticSequence(Double firstTerm, Double commonDifference)
            : base(firstTerm)
        {
            CommonDifference = commonDifference.RejectIf().IsEqualToValue(0, nameof(commonDifference));
        }

        /// <summary>
        /// Calculates the next term in the sequence.
        /// </summary>
        /// <param name="calculatedTerms">
        /// The ordered, calculated terms for the current <see cref="InfiniteSequence{T}" />.
        /// </param>
        /// <returns>
        /// The next term in the sequence.
        /// </returns>
        protected override Double CalculateNext(IEnumerable<Double> calculatedTerms) => calculatedTerms.Last() + CommonDifference;

        /// <summary>
        /// Gets the constant difference between the terms in the sequence.
        /// </summary>
        public Double CommonDifference
        {
            get;
        }
    }
}