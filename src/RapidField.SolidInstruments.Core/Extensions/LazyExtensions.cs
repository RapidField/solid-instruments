// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Lazy{T}" /> class with general purpose features.
    /// </summary>
    public static class LazyExtensions
    {
        /// <summary>
        /// Releases all resources consumed by the value of the current <see cref="Lazy{T}" />, if the value has been created.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Lazy{T}" />.
        /// </param>
        public static void Dispose<T>(this Lazy<T> target)
            where T : IDisposable
        {
            if ((target?.IsValueCreated).GetValueOrDefault(false))
            {
                target.Value.Dispose();
            }
        }

        /// <summary>
        /// Releases all resources consumed by the value of the current <see cref="Lazy{T}" />, if the value has been created.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Lazy{T}" />.
        /// </param>
        public static Task DisposeAsync<T>(this Lazy<T> target)
            where T : IAsyncDisposable
        {
            if ((target?.IsValueCreated).GetValueOrDefault(false))
            {
                return target.Value.DisposeAsync().AsTask();
            }

            return Task.CompletedTask;
        }
    }
}