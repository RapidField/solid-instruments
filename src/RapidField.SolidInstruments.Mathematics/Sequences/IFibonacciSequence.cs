// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System.Numerics;

namespace RapidField.SolidInstruments.Mathematics.Sequences
{
    /// <summary>
    /// Represents a unidirectional, positive sequence of Fibonacci numbers.
    /// </summary>
    public interface IFibonacciSequence : IInfiniteSequence<BigInteger>
    {
    }
}