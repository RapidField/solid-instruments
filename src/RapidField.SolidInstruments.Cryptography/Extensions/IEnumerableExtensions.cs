// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="IEnumerable{T}" /> interface with cryptographic features.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Selects a random element from the current <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IEnumerable{T}" />.
        /// </param>
        /// <param name="randomnessProvider">
        /// A <see cref="RandomNumberGenerator" /> that facilitates random element selection.
        /// </param>
        /// <returns>
        /// A randomly selected element within the current <see cref="IEnumerable{T}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The collection does not contain any elements.
        /// </exception>
        public static T SelectRandom<T>(this IEnumerable<T> target, RandomNumberGenerator randomnessProvider)
        {
            randomnessProvider.RejectIf().IsNull(nameof(randomnessProvider));
            var collectionLength = target.Count();

            if (collectionLength == 0)
            {
                throw new InvalidOperationException($"{nameof(SelectRandom)} cannot be invoked on an empty collection.");
            }

            return target.ElementAt(randomnessProvider.GetInt32(0, (collectionLength - 1)));
        }
    }
}