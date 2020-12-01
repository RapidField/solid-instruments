// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Uri" /> class with general purpose features.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Appends the specified data as query string parameters to the current absolute <see cref="Uri" />.
        /// </summary>
        /// <remarks>
        /// This method cannot be invoked against <see cref="Uri" /> instances representing relative URIs.
        /// </remarks>
        /// <param name="target">
        /// The current instance of the <see cref="Uri" />.
        /// </param>
        /// <param name="queryStringData">
        /// The query string data to be appended to the current <see cref="Uri" />.
        /// </param>
        /// <returns>
        /// The current <see cref="Uri" /> with the specified query string data appended.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="queryStringData" /> does not contain any non-empty keys.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queryStringData" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The current <see cref="Uri" /> represents a relative URI.
        /// </exception>
        public static Uri AppendQueryStringData(this Uri target, IDictionary<String, String> queryStringData)
        {
            if (target.IsAbsoluteUri is false)
            {
                throw new InvalidOperationException($"{nameof(AppendQueryStringData)} cannot be invoked against instances representing relative URIs.");
            }

            queryStringData.RejectIf().IsNull(nameof(queryStringData));
            var suppliedQueryStringParameterCount = queryStringData.Count;

            if (suppliedQueryStringParameterCount == 0)
            {
                throw new ArgumentException(EmptyKeyCollectionExceptionMessageTemplate.ApplyFormat(nameof(AppendQueryStringData), nameof(queryStringData)), nameof(queryStringData));
            }

            var queryStringBuilder = new StringBuilder();

            for (var i = 0; i < suppliedQueryStringParameterCount; i++)
            {
                var rawKey = queryStringData.Keys.ElementAt(i);
                var rawValue = queryStringData.Values.ElementAt(i);

                if (rawKey.IsNullOrEmpty())
                {
                    continue;
                }
                else if (rawValue.IsNullOrEmpty())
                {
                    queryStringBuilder.Append($"{WebUtility.UrlEncode(rawKey)}&");
                }
                else
                {
                    queryStringBuilder.Append($"{WebUtility.UrlEncode(rawKey)}={WebUtility.UrlEncode(rawValue)}&");
                }
            }

            var queryStringBuilderLength = queryStringBuilder.Length;

            if (queryStringBuilderLength == 0)
            {
                throw new ArgumentException(EmptyKeyCollectionExceptionMessageTemplate.ApplyFormat(nameof(AppendQueryStringData), nameof(queryStringData), nameof(queryStringData)));
            }

            // Remove the trailing ampersand.
            queryStringBuilder.Remove(queryStringBuilderLength - 1, 1);

            if (target.Query.IsNullOrEmpty())
            {
                return new Uri($"{target.AbsoluteUri}?{queryStringBuilder}", UriKind.Absolute);
            }
            else
            {
                return new Uri($"{target.AbsoluteUri}&{queryStringBuilder}", UriKind.Absolute);
            }
        }

        /// <summary>
        /// Converts the current <see cref="Uri" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Uri" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Uri" />.
        /// </returns>
        public static Byte[] ToByteArray(this Uri target) => target.ToString().ToByteArray(Encoding.UTF8);

        /// <summary>
        /// Represents a message template for exceptions that are raised when no valid keys are supplied to a method.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String EmptyKeyCollectionExceptionMessageTemplate = "{0} cannot accept {1} arguments that do not contain any non-empty keys.";
    }
}