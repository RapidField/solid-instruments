// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="IEnumerable{T}" /> interface with general purpose features.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Returns a collection containing the combined elements of the current collection of collections.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the nested collections.
        /// </typeparam>
        /// <param name="target">
        /// The current collection of collections.
        /// </param>
        /// <returns>
        /// A collection containing the combined elements of the current collection of collections.
        /// </returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> target)
        {
            var flattenedCollection = new List<T>();

            foreach (var collection in target)
            {
                if (collection?.Any() ?? false)
                {
                    flattenedCollection.AddRange(collection);
                }
            }

            return flattenedCollection;
        }

        /// <summary>
        /// Indicates whether or not the current <see cref="IEnumerable{T}" /> is empty.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <param name="target">
        /// The current instance of the <see cref="IEnumerable{T}" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="IEnumerable{T}" /> is empty, otherwise <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsEmpty<T>(this IEnumerable<T> target) => target.Any() is false;

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

                if (targetElement is null && otherElement is null)
                {
                    continue;
                }
                else if (targetElement is null && otherElement is not null)
                {
                    return false;
                }
                else if (targetElement is not null && otherElement is null)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> target) => target is null || target.IsEmpty();
    }
}