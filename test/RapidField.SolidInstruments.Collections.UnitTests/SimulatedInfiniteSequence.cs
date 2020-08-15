// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Collections.UnitTests
{
    /// <summary>
    /// Represents an <see cref="InfiniteSequence{T}" /> derivative that is used for testing.
    /// </summary>
    internal class SimulatedInfiniteSequence : InfiniteSequence<Int32>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedInfiniteSequence" /> class.
        /// </summary>
        /// <param name="seedTerm">
        /// The first term in the sequence.
        /// </param>
        public SimulatedInfiniteSequence(Int32 seedTerm)
            : base(seedTerm)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedInfiniteSequence" /> class.
        /// </summary>
        /// <param name="seedTerms">
        /// The first terms in the sequence.
        /// </param>
        public SimulatedInfiniteSequence(Int32[] seedTerms)
            : base(seedTerms)
        {
            return;
        }

        /// <summary>
        /// Calculates the next term in the sequence.
        /// </summary>
        /// <param name="calculatedTerms">
        /// The ordered, calculated terms for the current <see cref="SimulatedInfiniteSequence" />.
        /// </param>
        /// <returns>
        /// The next term in the sequence.
        /// </returns>
        protected override Int32 CalculateNext(IEnumerable<Int32> calculatedTerms) => calculatedTerms.Last() + 1;
    }
}