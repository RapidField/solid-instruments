// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents a streaming data signal.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the channel's output value.
    /// </typeparam>
    public interface IChannel<T> : IChannel
    {
        /// <summary>
        /// Gets the unit of output from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="index">
        /// A zero-based index within the output stream at which to read.
        /// </param>
        /// <returns>
        /// The discrete unit of output from the channel's output stream at the specified index.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <see cref="InvalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.RaiseException" /> and the specified index
        /// is out of range.
        /// </exception>
        /// <exception cref="ChannelReadException">
        /// An exception was raised while performing the read operation, or the channel was unavailable.
        /// </exception>
        T this[Int32 index]
        {
            get;
        }

        /// <summary>
        /// Asynchronously reads a discrete unit of output from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="index">
        /// A zero-based index within the output stream at which to read.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing a discrete unit of output from the channel's output stream
        /// at the specified index.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="InvalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.RaiseException" /> and
        /// <paramref name="index" /> is less than zero -or- <paramref name="index" /> exceeds the end boundary of the channel's
        /// output stream.
        /// </exception>
        /// <exception cref="ChannelReadException">
        /// An exception was raised while performing the read operation, or the channel was unavailable.
        /// </exception>
        Task<IDiscreteUnitOfOutput<T>> ReadAsync(Int32 index);

        /// <summary>
        /// Asynchronously reads a sample from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="index">
        /// A zero-based index within the output stream at which to read.
        /// </param>
        /// <param name="lookBehindLength">
        /// The number of discrete units of output preceding the specified index to include with the sample. The default value is
        /// zero.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing a sample from the channel's output stream at the specified
        /// index.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="InvalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.RaiseException" /> and
        /// <paramref name="index" /> is less than zero -or- <paramref name="lookBehindLength" /> is less than zero -or- the
        /// resulting sample range precedes the start boundary of the channel's output stream -or- the resulting sample range
        /// exceeds the end boundary of the channel's output stream.
        /// </exception>
        /// <exception cref="ChannelReadException">
        /// An exception was raised while performing the read operation, or the channel was unavailable.
        /// </exception>
        Task<ISignalSample<T>> ReadAsync(Int32 index, Int32 lookBehindLength);

        /// <summary>
        /// Asynchronously reads a sample from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="index">
        /// A zero-based index within the output stream at which to read.
        /// </param>
        /// <param name="lookBehindLength">
        /// The number of discrete units of output preceding the specified index to include with the sample. The default value is
        /// zero.
        /// </param>
        /// <param name="lookAheadLength">
        /// The number of discrete units of output following the specified index to include with the sample. The default value is
        /// zero.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing a sample from the channel's output stream at the specified
        /// index.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="InvalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.RaiseException" /> and
        /// <paramref name="index" /> is less than zero -or- <paramref name="lookBehindLength" /> is less than zero -or-
        /// <paramref name="lookAheadLength" /> is less than zero -or- the resulting sample range precedes the start boundary of the
        /// channel's output stream -or- the resulting sample range exceeds the end boundary of the channel's output stream.
        /// </exception>
        /// <exception cref="ChannelReadException">
        /// An exception was raised while performing the read operation, or the channel was unavailable.
        /// </exception>
        Task<ISignalSample<T>> ReadAsync(Int32 index, Int32 lookBehindLength, Int32 lookAheadLength);

        /// <summary>
        /// Returns a discrete unit that represents a silent or empty signal.
        /// </summary>
        /// <returns>
        /// A discrete unit that represents a silent or empty signal.
        /// </returns>
        T ReadSilence();

        /// <summary>
        /// Returns a discrete unit that represents a silent or empty signal.
        /// </summary>
        /// <param name="index">
        /// A zero-based index to include with the result.
        /// </param>
        /// <returns>
        /// A discrete unit that represents a silent or empty signal.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="InvalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.RaiseException" /> and
        /// <paramref name="index" /> is less than zero.
        /// </exception>
        IDiscreteUnitOfOutput<T> ReadSilence(Int32 index);

        /// <summary>
        /// Gets the behavior of the channel when an out-of-range read request is made.
        /// </summary>
        InvalidReadBehavior InvalidReadBehavior
        {
            get;
        }
    }

    /// <summary>
    /// Represents a streaming data signal.
    /// </summary>
    public interface IChannel : IInstrument
    {
        /// <summary>
        /// Changes the channel's status to <see cref="ChannelStatus.Live" /> if it is currently silent, otherwise changes the
        /// status to <see cref="ChannelStatus.Silent" /> if it is currently live, otherwise raises an
        /// <see cref="InvalidOperationException" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The channel's status is equal to <see cref="ChannelStatus.Unavailable" />.
        /// </exception>
        void Toggle();

        /// <summary>
        /// Gets a unique identifier for the current <see cref="IChannel" />.
        /// </summary>
        Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the name of the current <see cref="IChannel" />.
        /// </summary>
        String Name
        {
            get;
        }

        /// <summary>
        /// Gets the number of discrete units in the output stream for the current <see cref="IChannel" />, or -1 if the length is
        /// not fixed.
        /// </summary>
        Int32 OutputLength
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the output stream for the current <see cref="IChannel" /> has a fixed length.
        /// </summary>
        Boolean OutputLengthIsFixed
        {
            get;
        }

        /// <summary>
        /// Gets the type of a discrete unit of output value for the current <see cref="IChannel" />.
        /// </summary>
        Type OutputType
        {
            get;
        }

        /// <summary>
        /// Gets the status of the current <see cref="IChannel" />.
        /// </summary>
        ChannelStatus Status
        {
            get;
        }
    }
}