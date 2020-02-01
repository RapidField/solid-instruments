// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents a discrete unit of output from an <see cref="IChannel" />.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the associated channel's output value.
    /// </typeparam>
    public interface IDiscreteUnitOfOutput<T>
    {
        /// <summary>
        /// Gets the zero-based index for the current <see cref="IDiscreteUnitOfOutput{T}" /> within the associated channel's output
        /// stream.
        /// </summary>
        public Int32 ChannelReadIndex
        {
            get;
        }

        /// <summary>
        /// Gets the output value.
        /// </summary>
        public T Value
        {
            get;
        }
    }
}