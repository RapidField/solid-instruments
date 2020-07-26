// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents the result of a read operation performed against an <see cref="IChannel{T}" />.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the sampled channel's output value.
    /// </typeparam>
    public interface ISignalSample<T>
    {
        /// <summary>
        /// Gets a range of discrete units of output following <see cref="UnitOfOutput" /> in the channel's output stream, or an
        /// empty range if look-ahead was not requested.
        /// </summary>
        public IOutputRange<T> LookAheadRange
        {
            get;
        }

        /// <summary>
        /// Gets a range of discrete units of output preceding <see cref="UnitOfOutput" /> in the channel's output stream, or an
        /// empty range if look-behind was not requested.
        /// </summary>
        public IOutputRange<T> LookBehindRange
        {
            get;
        }

        /// <summary>
        /// Gets the discrete unit of output that was read from the channel's output stream.
        /// </summary>
        public IDiscreteUnitOfOutput<T> UnitOfOutput
        {
            get;
        }
    }
}