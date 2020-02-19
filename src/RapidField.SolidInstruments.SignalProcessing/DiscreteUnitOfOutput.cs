// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents a discrete unit of output from an <see cref="IChannel" />.
    /// </summary>
    /// <remarks>
    /// <see cref="DiscreteUnitOfOutput{T}" /> is the default implementation of <see cref="IDiscreteUnitOfOutput{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the associated channel's output value.
    /// </typeparam>
    public sealed class DiscreteUnitOfOutput<T> : IDiscreteUnitOfOutput<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreteUnitOfOutput{T}" /> class.
        /// </summary>
        /// <param name="value">
        /// The output value.
        /// </param>
        /// <param name="channelReadIndex">
        /// The zero-based index for the output value within the associated channel's output stream.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="channelReadIndex" /> is less than zero.
        /// </exception>
        public DiscreteUnitOfOutput(T value, Int32 channelReadIndex)
        {
            ChannelReadIndex = channelReadIndex.RejectIf().IsLessThan(0, nameof(channelReadIndex));
            Value = value;
        }

        /// <summary>
        /// Converts the value of the current <see cref="DiscreteUnitOfOutput{T}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="DiscreteUnitOfOutput{T}" />.
        /// </returns>
        public override String ToString() => $"{{ {nameof(ChannelReadIndex)}: {ChannelReadIndex}, {nameof(Value)}: {Value} }}";

        /// <summary>
        /// Gets the zero-based index for the current <see cref="DiscreteUnitOfOutput{T}" /> within the associated channel's output
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