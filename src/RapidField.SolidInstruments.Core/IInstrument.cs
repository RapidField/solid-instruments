// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a utility with disposable resources and exposes a lazily-loaded concurrency control mechanism.
    /// </summary>
    public interface IInstrument : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IInstrument" /> is fully occupied, as measured by thread
        /// saturation for state-controlling operations.
        /// </summary>
        /// <remarks>
        /// Interrogate this property to determine if the instrument is immediately available to perform an operation that reserves
        /// state control. This is useful for cases in which another resource may be utilized to perform the same operation.
        /// </remarks>
        public Boolean IsBusy
        {
            get;
        }
    }
}