// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a thread-safe, infinite sequence of calculated values.
    /// </summary>
    /// <typeparam name="T">
    /// The element type of the sequence.
    /// </typeparam>
    public abstract class InfiniteSequence<T> : ICalculatedSequence<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfiniteSequence{T}" /> class.
        /// </summary>
        protected InfiniteSequence()
            : this(Array.Empty<T>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfiniteSequence{T}" /> class.
        /// </summary>
        /// <param name="seedTerm">
        /// The first term in the sequence.
        /// </param>
        protected InfiniteSequence(T seedTerm)
            : this(new T[] { seedTerm })
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfiniteSequence{T}" /> class.
        /// </summary>
        /// <param name="seedTerms">
        /// The first terms in the sequence.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="seedTerms" /> is <see langword="null" />.
        /// </exception>
        protected InfiniteSequence(T[] seedTerms)
        {
            CalculatedTerms.AddRange(seedTerms);
            SeedTermCount = seedTerms.Length;
        }

        /// <summary>
        /// Gets the term at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the term to get.
        /// </param>
        /// <returns>
        /// The term at the specified index.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="index" /> is less than zero.
        /// </exception>
        public T this[Int32 index]
        {
            get
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                var indexCeiling = (index + 1);

                lock (SyncRoot)
                {
                    while (CalculatedTerms.Count < indexCeiling)
                    {
                        CalculatedTerms.Add(CalculateNext(CalculatedTerms));
                    }

                    return CalculatedTerms[index];
                }
            }
        }

        /// <summary>
        /// Calculates the next term in the sequence.
        /// </summary>
        /// <returns>
        /// The next term in the sequence.
        /// </returns>
        public T CalculateNext()
        {
            lock (SyncRoot)
            {
                var nextTerm = CalculateNext(CalculatedTerms);
                CalculatedTerms.Add(nextTerm);
                return nextTerm;
            }
        }

        /// <summary>
        /// Calculates the next terms in the sequence.
        /// </summary>
        /// <param name="count">
        /// The number of terms to calculate.
        /// </param>
        /// <returns>
        /// An array containing the calculated terms.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        public T[] CalculateNext(Int32 count)
        {
            lock (SyncRoot)
            {
                var nextTerms = new T[count.RejectIf().IsLessThan(0, nameof(count))];

                for (var i = 0; i < count; i++)
                {
                    var nextTerm = CalculateNext(CalculatedTerms);
                    CalculatedTerms.Add(nextTerm);
                    nextTerms[i] = nextTerm;
                }

                return nextTerms;
            }
        }

        /// <summary>
        /// Clears the terms in the current <see cref="InfiniteSequence{T}" />, leaving in place the seed terms.
        /// </summary>
        public void Reset()
        {
            lock (SyncRoot)
            {
                CalculatedTerms = new List<T>(CalculatedTerms.Take(SeedTermCount));
            }
        }

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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startIndex" /> is less than zero or <paramref name="count" /> is less than zero.
        /// </exception>
        public T[] ToArray(Int32 startIndex, Int32 count)
        {
            var indexCeiling = (startIndex.RejectIf().IsLessThan(0, nameof(startIndex)) + count.RejectIf().IsLessThan(0, nameof(count)));

            lock (SyncRoot)
            {
                while (CalculatedTerms.Count < indexCeiling)
                {
                    CalculatedTerms.Add(CalculateNext(CalculatedTerms));
                }

                return CalculatedTerms.Skip(startIndex).Take(count).ToArray();
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="InfiniteSequence{T}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="InfiniteSequence{T}" />.
        /// </returns>
        public override String ToString() => $"Calculated term count: {CalculatedTermCount.ToString()}";

        /// <summary>
        /// Calculates the next term in the sequence.
        /// </summary>
        /// <param name="calculatedTerms">
        /// The ordered, calculated terms for the current <see cref="InfiniteSequence{T}" />.
        /// </param>
        /// <returns>
        /// The next term in the sequence.
        /// </returns>
        protected abstract T CalculateNext(IEnumerable<T> calculatedTerms);

        /// <summary>
        /// Gets the number of terms that have been calculated.
        /// </summary>
        public Int32 CalculatedTermCount => CalculatedTerms.Count;

        /// <summary>
        /// Represents the number of seed terms that were provided during initialization of the current
        /// <see cref="InfiniteSequence{T}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 SeedTermCount;

        /// <summary>
        /// Represents an object that can be used to synchronize access to the current <see cref="InfiniteSequence{T}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Object SyncRoot = new Object();

        /// <summary>
        /// Represents the ordered, calculated terms for the current <see cref="InfiniteSequence{T}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<T> CalculatedTerms = new List<T>();
    }
}