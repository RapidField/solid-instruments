// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RapidField.SolidInstruments.Mathematics.Sequences
{
    /// <summary>
    /// Represents a unidirectional, positive sequence of Fibonacci numbers.
    /// </summary>
    /// <remarks>
    /// <see cref="FibonacciSequence" /> is the default implementation of <see cref="IFibonacciSequence" />.
    /// </remarks>
    public class FibonacciSequence : InfiniteSequence<BigInteger>, IFibonacciSequence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FibonacciSequence" /> class.
        /// </summary>
        /// <param name="firstTerm">
        /// The first term in the sequence.
        /// </param>
        /// <param name="secondTerm">
        /// The second term in the sequence.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The sum of <paramref name="firstTerm" /> and <paramref name="secondTerm" /> is equal to zero.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="firstTerm" /> or <paramref name="secondTerm" /> is less than zero -or- <paramref name="secondTerm" /> is
        /// less than <paramref name="firstTerm" />.
        /// </exception>
        public FibonacciSequence(BigInteger firstTerm, BigInteger secondTerm)
            : base(new BigInteger[] { firstTerm.RejectIf().IsLessThan(BigInteger.Zero, nameof(firstTerm)), secondTerm.RejectIf().IsLessThan(BigInteger.Zero, nameof(firstTerm)).OrIf().IsLessThan(firstTerm, nameof(secondTerm), nameof(firstTerm)) })
        {
            return;
        }

        /// <summary>
        /// Calculates the next term in the sequence.
        /// </summary>
        /// <param name="calculatedTerms">
        /// The ordered, calculated terms for the current <see cref="FibonacciSequence" />.
        /// </param>
        /// <returns>
        /// The next term in the sequence.
        /// </returns>
        protected override BigInteger CalculateNext(IEnumerable<BigInteger> calculatedTerms)
        {
            var inputTerms = calculatedTerms.Skip(calculatedTerms.Count() - 2).Take(2).ToArray();
            return (inputTerms[0] + inputTerms[1]);
        }

        /// <summary>
        /// Represents the classic Fibonacci sequence beginning with zero and one.
        /// </summary>
        public static readonly IFibonacciSequence Classic = new FibonacciSequence(0, 1);
    }
}