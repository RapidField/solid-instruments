// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;

namespace RapidField.SolidInstruments.Mathematics.Sequences
{
    /// <summary>
    /// Represents a unidirectional sequence of numbers that are equidistant at each step.
    /// </summary>
    public interface IArithmeticSequence : IInfiniteSequence<Double>
    {
        /// <summary>
        /// Gets the constant difference between the terms in the sequence.
        /// </summary>
        Double CommonDifference
        {
            get;
        }
    }
}