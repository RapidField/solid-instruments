// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Mathematics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="IList{T}" /> interface with cryptographic features.
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Performs a reverse stagger-sort on the current <see cref="IList{T}" />.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the <see cref="IList{T}" />.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IList{T}" />.
        /// </param>
        public static void ReverseStaggerSort<T>(this IList<T> target)
        {
            var elementCount = target.Count;

            if (elementCount > 1)
            {
                T[] evenIndexElements;
                T[] oddIndexElements;

                if (elementCount.IsEven())
                {
                    // Size the arrays appropriately for an even-length collection.
                    evenIndexElements = new T[elementCount / 2];
                    oddIndexElements = new T[elementCount / 2];
                }
                else
                {
                    // Size the arrays appropriately for an odd-length collection.
                    evenIndexElements = new T[((elementCount - 1) / 2) + 1];
                    oddIndexElements = new T[(elementCount - 1) / 2];
                }

                var currentEvenElementIndex = 0;
                var currentOddElementIndex = 0;

                for (var i = 0; i < elementCount; i++)
                {
                    if (i.IsEven())
                    {
                        // Capture a collection of even-indexed elements.
                        evenIndexElements[currentEvenElementIndex++] = target.ElementAt(i);
                    }
                    else
                    {
                        // Capture a collection of odd-indexed elements.
                        oddIndexElements[currentOddElementIndex++] = target.ElementAt(i);
                    }
                }

                // Reverse the odd-indexed elements.
                oddIndexElements = oddIndexElements.Reverse().ToArray();
                currentEvenElementIndex = 0;
                currentOddElementIndex = 0;

                for (var i = 0; i < elementCount; i++)
                {
                    if (i.IsEven())
                    {
                        // Reintroduce the even-indexed elements.
                        target[elementCount - 1 - i] = evenIndexElements[currentOddElementIndex++];
                    }
                    else
                    {
                        // Reintroduce the reverse-ordered odd-indexed elements.
                        target[elementCount - 1 - i] = oddIndexElements[currentEvenElementIndex++];
                    }
                }
            }
        }

        /// <summary>
        /// Randomly shuffles the elements in the current <see cref="IList{T}" />.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the <see cref="IList{T}" />.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IList{T}" />.
        /// </param>
        /// <param name="randomnessProvider">
        /// A <see cref="RandomNumberGenerator" /> that facilitates random shuffling.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        public static void Shuffle<T>(this IList<T> target, RandomNumberGenerator randomnessProvider)
        {
            randomnessProvider = randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
            var currentIndex = target.Count - 1;

            while (currentIndex >= 0)
            {
                var randomIndex = randomnessProvider.GetInt32(0, currentIndex);
                var randomElement = target[randomIndex];

                // Swap the element at the current index with the random element.
                target[randomIndex] = target[currentIndex];
                target[currentIndex] = randomElement;
                currentIndex--;
            }
        }
    }
}