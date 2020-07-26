// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;

namespace RapidField.SolidInstruments.Mathematics.Sequences
{
    /// <summary>
    /// Represents a unidirectional sequence of numbers for which each term is found by multiplying the previous term by a constant
    /// factor.
    /// </summary>
    public interface IGeometricSequence : IInfiniteSequence<Double>
    {
        /// <summary>
        /// Gets the constant factor between the terms in the sequence.
        /// </summary>
        public Double CommonRatio
        {
            get;
        }
    }
}