// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System.Numerics;

namespace RapidField.SolidInstruments.Mathematics.Sequences
{
    /// <summary>
    /// Represents a sequence of positive integers that have no positive divisors other than one and themselves.
    /// </summary>
    public interface IPrimeNumberSequence : IInfiniteSequence<BigInteger>
    {
    }
}