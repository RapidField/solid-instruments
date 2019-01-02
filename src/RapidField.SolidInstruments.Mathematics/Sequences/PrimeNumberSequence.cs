// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace RapidField.SolidInstruments.Mathematics.Sequences
{
    /// <summary>
    /// Represents a sequence of positive integers that have no positive divisors other than one and themselves.
    /// </summary>
    public sealed class PrimeNumberSequence : InfiniteSequence<BigInteger>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrimeNumberSequence" /> class.
        /// </summary>
        public PrimeNumberSequence()
            : base(new BigInteger[] { 2, 3 })
        {
            return;
        }

        /// <summary>
        /// Calculates the next term in the sequence.
        /// </summary>
        /// <param name="calculatedTerms">
        /// The ordered, calculated terms for the current <see cref="PrimeNumberSequence" />.
        /// </param>
        /// <returns>
        /// The next term in the sequence.
        /// </returns>
        protected override BigInteger CalculateNext(IEnumerable<BigInteger> calculatedTerms)
        {
            var lastTerm = calculatedTerms.Last();

            while (IsPrimeNumber(lastTerm += 2) == false)
            {
                continue;
            }

            return lastTerm;
        }

        /// <summary>
        /// Determines whether or not the specified number is a prime number.
        /// </summary>
        /// <param name="number">
        /// Any odd number greater than one.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified number is prime, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        private static Boolean IsPrimeNumber(BigInteger number)
        {
            for (var i = new BigInteger(3); (i * i) <= number; i += 2)
            {
                if ((number % i) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Represents a static collection of prime numbers.
        /// </summary>
        public static readonly PrimeNumberSequence Instance = new PrimeNumberSequence();
    }
}