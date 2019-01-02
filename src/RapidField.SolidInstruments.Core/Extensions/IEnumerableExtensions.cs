// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="IEnumerable{T}" /> interface with general purpose features.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Indicates whether or not the current <see cref="IEnumerable{T}" /> represents the same elements, in the same order, as
        /// the specified other collection.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <param name="target">
        /// The current instance of the <see cref="IEnumerable{T}" />.
        /// </param>
        /// <param name="otherCollection">
        /// Another <see cref="IEnumerable{T}" /> to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="IEnumerable{T}" /> represents the same elements, in the same order, as
        /// the specified other collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="otherCollection" /> is <see langword="null" />.
        /// </exception>
        public static Boolean IsEquivalentTo<T>(this IEnumerable<T> target, IEnumerable<T> otherCollection)
        {
            var collectionLength = target.Count();

            if (collectionLength != otherCollection.RejectIf().IsNull(nameof(otherCollection)).TargetArgument.Count())
            {
                return false;
            }

            for (var i = 0; i < collectionLength; i++)
            {
                var targetElement = target.ElementAt(i);
                var otherElement = otherCollection.ElementAt(i);

                if (targetElement == null && otherElement == null)
                {
                    continue;
                }
                else if (targetElement == null && otherElement != null)
                {
                    return false;
                }
                else if (targetElement != null && otherElement == null)
                {
                    return false;
                }
                else if (targetElement.Equals(otherElement))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Indicates whether or not the current <see cref="IEnumerable{T}" /> is <see langword="null" /> or empty.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <param name="target">
        /// The current instance of the <see cref="IEnumerable{T}" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="IEnumerable{T}" /> is <see langword="null" /> or empty, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> target) => (target is null || target.Count() == 0);
    }
}