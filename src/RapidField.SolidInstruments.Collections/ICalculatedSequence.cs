﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a sequence of calculated terms.
    /// </summary>
    /// <typeparam name="T">
    /// The element type of the sequence.
    /// </typeparam>
    public interface ICalculatedSequence<T> : ICalculatedSequence
    {
        /// <summary>
        /// Calculates the next term in the sequence.
        /// </summary>
        /// <returns>
        /// The next term in the sequence.
        /// </returns>
        public T CalculateNext();

        /// <summary>
        /// Calculates the next terms in the sequence.
        /// </summary>
        /// <param name="count">
        /// The number of terms to calculate.
        /// </param>
        /// <returns>
        /// An array containing the calculated terms.
        /// </returns>
        public T[] CalculateNext(Int32 count);

        /// <summary>
        /// Calculates the specified range of terms and returns them as an array.
        /// </summary>
        /// <param name="startIndex">
        /// The zero-based index of the first returned term.
        /// </param>
        /// <param name="count">
        /// The number of terms to return.
        /// </param>
        /// <returns>
        /// An array containing the calculated terms in the specified range.
        /// </returns>
        public T[] ToArray(Int32 startIndex, Int32 count);
    }

    /// <summary>
    /// Represents a sequence of calculated terms.
    /// </summary>
    public interface ICalculatedSequence
    {
        /// <summary>
        /// Gets the number of terms that have been calculated.
        /// </summary>
        public Int32 CalculatedTermCount
        {
            get;
        }
    }
}