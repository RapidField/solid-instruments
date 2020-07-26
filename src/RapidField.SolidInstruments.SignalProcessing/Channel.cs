// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents a streaming data signal.
    /// </summary>
    /// <remarks>
    /// <see cref="Channel{T}" /> is the default implementation of <see cref="IChannel{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the channel's output value.
    /// </typeparam>
    public abstract class Channel<T> : Instrument, IChannel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Channel{T}" /> class.
        /// </summary>
        protected Channel()
            : this(DefaultInvalidReadBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel{T}" /> class.
        /// </summary>
        /// <param name="invalidReadBehavior">
        /// The behavior of the channel when an out-of-range read request is made. The default value is
        /// <see cref="InvalidReadBehavior.ReadSilence" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="invalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.Unspecified" />.
        /// </exception>
        protected Channel(InvalidReadBehavior invalidReadBehavior)
            : this(invalidReadBehavior, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel{T}" /> class.
        /// </summary>
        /// <param name="name">
        /// The name of the channel, or <see langword="null" /> to use the name of the channel type. The default value is
        /// <see langword="null" />.
        /// </param>
        protected Channel(String name)
            : this(DefaultInvalidReadBehavior, name)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel{T}" /> class.
        /// </summary>
        /// <param name="invalidReadBehavior">
        /// The behavior of the channel when an out-of-range read request is made. The default value is
        /// <see cref="InvalidReadBehavior.ReadSilence" />.
        /// </param>
        /// <param name="name">
        /// The name of the channel, or <see langword="null" /> to use the name of the channel type. The default value is
        /// <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="invalidReadBehavior" /> is equal to <see cref="InvalidReadBehavior.Unspecified" />.
        /// </exception>
        protected Channel(InvalidReadBehavior invalidReadBehavior, String name)
            : base()
        {
            Identifier = Guid.NewGuid();
            InvalidReadBehavior = invalidReadBehavior.RejectIf().IsEqualToValue(InvalidReadBehavior.Unspecified, nameof(invalidReadBehavior));
            Name = name.IsNullOrWhiteSpace() ? GetType().Name : ToString();
            OutputType = typeof(T);
            Status = ChannelStatus.Live;
        }

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
        public T this[Int32 index]
        {
            get
            {
                switch (Status)
                {
                    case ChannelStatus.Live:

                        break;

                    case ChannelStatus.Silent:

                        return ReadSilence();

                    case ChannelStatus.Unavailable:

                        throw new ChannelReadException(this);

                    default:

                        throw new UnsupportedSpecificationException($"The specified channel status, {Status}, is not supported.");
                }

                if (OutputLengthIsFixed && (index < 0 || index >= OutputLength))
                {
                    return InvalidReadBehavior switch
                    {
                        InvalidReadBehavior.RaiseException => throw new IndexOutOfRangeException(),
                        InvalidReadBehavior.ReadSilence => ReadSilence(),
                        _ => throw new UnsupportedSpecificationException($"The specified invalid read behavior, {InvalidReadBehavior}, is not supported.")
                    };
                }

                using (var controlToken = StateControl.Enter())
                {
                    try
                    {
                        if (TryRead(index, out var outputValue))
                        {
                            return outputValue;
                        }
                    }
                    catch (ChannelReadException)
                    {
                        Status = ChannelStatus.Unavailable;
                        throw;
                    }
                    catch (Exception exception)
                    {
                        Status = ChannelStatus.Unavailable;
                        throw new ChannelReadException(this, exception);
                    }

                    Status = ChannelStatus.Unavailable;
                    throw new ChannelReadException(this);
                }
            }
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
        public Task<IDiscreteUnitOfOutput<T>> ReadAsync(Int32 index)
        {
            switch (Status)
            {
                case ChannelStatus.Live:

                    break;

                case ChannelStatus.Silent:

                    return Task.FromResult(ReadSilence(index));

                case ChannelStatus.Unavailable:

                    throw new ChannelReadException(this);

                default:

                    throw new UnsupportedSpecificationException($"The specified channel status, {Status}, is not supported.");
            }

            if (OutputLengthIsFixed && (index < 0 || index >= OutputLength))
            {
                return InvalidReadBehavior switch
                {
                    InvalidReadBehavior.RaiseException => throw new ArgumentOutOfRangeException(nameof(index)),
                    InvalidReadBehavior.ReadSilence => Task.FromResult(ReadSilence(index)),
                    _ => throw new UnsupportedSpecificationException($"The specified invalid read behavior, {InvalidReadBehavior}, is not supported.")
                };
            }

            using (var controlToken = StateControl.Enter())
            {
                try
                {
                    if (TryRead(index, out var outputValue))
                    {
                        return Task.FromResult<IDiscreteUnitOfOutput<T>>(new DiscreteUnitOfOutput<T>(outputValue, index));
                    }
                }
                catch (ChannelReadException)
                {
                    Status = ChannelStatus.Unavailable;
                    throw;
                }
                catch (Exception exception)
                {
                    Status = ChannelStatus.Unavailable;
                    throw new ChannelReadException(this, exception);
                }

                Status = ChannelStatus.Unavailable;
                throw new ChannelReadException(this);
            }
        }

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
        public Task<ISignalSample<T>> ReadAsync(Int32 index, Int32 lookBehindLength) => ReadAsync(index, lookBehindLength, 0);

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
        public Task<ISignalSample<T>> ReadAsync(Int32 index, Int32 lookBehindLength, Int32 lookAheadLength)
        {
            switch (Status)
            {
                case ChannelStatus.Live:

                    break;

                case ChannelStatus.Silent:

                    return Task.FromResult<ISignalSample<T>>(new SignalSample<T>(ReadSilence(index)));

                case ChannelStatus.Unavailable:

                    throw new ChannelReadException(this);

                default:

                    throw new UnsupportedSpecificationException($"The specified channel status, {Status}, is not supported.");
            }

            if (OutputLengthIsFixed)
            {
                var argumentOutOfRangeExceptionParameterName = (String)null;

                if (index < 0 || index >= OutputLength)
                {
                    argumentOutOfRangeExceptionParameterName = nameof(index);
                }
                else if (lookBehindLength < 0 || lookBehindLength > index)
                {
                    argumentOutOfRangeExceptionParameterName = nameof(lookBehindLength);
                }
                else if (lookAheadLength < 0 || lookAheadLength >= (OutputLength - index))
                {
                    argumentOutOfRangeExceptionParameterName = nameof(lookAheadLength);
                }

                if (argumentOutOfRangeExceptionParameterName is null == false)
                {
                    return InvalidReadBehavior switch
                    {
                        InvalidReadBehavior.RaiseException => throw new ArgumentOutOfRangeException(argumentOutOfRangeExceptionParameterName),
                        InvalidReadBehavior.ReadSilence => Task.FromResult<ISignalSample<T>>(new SignalSample<T>(ReadSilence(index))),
                        _ => throw new UnsupportedSpecificationException($"The specified invalid read behavior, {InvalidReadBehavior}, is not supported.")
                    };
                }
            }

            var startIndex = (index - lookBehindLength);
            var count = (lookBehindLength + lookAheadLength + 1);
            var endIndex = (startIndex + count);
            var range = default(T[]);

            using (var controlToken = StateControl.Enter())
            {
                try
                {
                    var outputRange = new T[count];

                    if (TryRead(startIndex, count, outputRange))
                    {
                        range = outputRange;
                    }
                    else
                    {
                        Status = ChannelStatus.Unavailable;
                        throw new ChannelReadException(this);
                    }
                }
                catch (ChannelReadException)
                {
                    Status = ChannelStatus.Unavailable;
                    throw;
                }
                catch (Exception exception)
                {
                    Status = ChannelStatus.Unavailable;
                    throw new ChannelReadException(this, exception);
                }
            }

            var unitOfOutput = new DiscreteUnitOfOutput<T>(range[lookBehindLength], lookBehindLength);
            var lookBehindList = new List<IDiscreteUnitOfOutput<T>>(lookBehindLength);
            var lookAheadList = new List<IDiscreteUnitOfOutput<T>>(lookAheadLength);

            for (var i = 0; i < lookBehindLength; i++)
            {
                lookBehindList.Add(new DiscreteUnitOfOutput<T>(range[i], (startIndex + i)));
            }

            for (var i = 0; i < lookAheadLength; i++)
            {
                var adjustment = (lookBehindLength + i + 1);
                lookAheadList.Add(new DiscreteUnitOfOutput<T>(range[adjustment], (startIndex + adjustment)));
            }

            return Task.FromResult<ISignalSample<T>>(new SignalSample<T>(unitOfOutput, new OutputRange<T>(lookBehindList), new OutputRange<T>(lookAheadList)));
        }

        /// <summary>
        /// Returns a discrete unit that represents a silent or empty signal.
        /// </summary>
        /// <returns>
        /// A discrete unit that represents a silent or empty signal.
        /// </returns>
        public virtual T ReadSilence() => default;

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
        public IDiscreteUnitOfOutput<T> ReadSilence(Int32 index) => new DiscreteUnitOfOutput<T>(ReadSilence(), InvalidReadBehavior == InvalidReadBehavior.RaiseException ? index : (index < 0 ? 0 : index));

        /// <summary>
        /// Changes the channel's status to <see cref="ChannelStatus.Live" /> if it is currently silent, otherwise changes the
        /// status to <see cref="ChannelStatus.Silent" /> if it is currently live, otherwise raises an
        /// <see cref="InvalidOperationException" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The channel's status is equal to <see cref="ChannelStatus.Unavailable" />.
        /// </exception>
        public void Toggle()
        {
            using (var controlToken = StateControl.Enter())
            {
                Status = Status switch
                {
                    ChannelStatus.Live => ChannelStatus.Silent,
                    ChannelStatus.Silent => ChannelStatus.Live,
                    ChannelStatus.Unavailable => throw new InvalidOperationException("The channel is unavailable."),
                    _ => throw new UnsupportedSpecificationException($"The specified channel status, {Status}, is not supported.")
                };
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="Channel{T}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Channel{T}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Identifier)}\": \"{Identifier.ToSerializedString()}\", {nameof(Name)}\": \"{Name}\" }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Channel{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Attempts to read a discrete unit of output from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="index">
        /// A zero-based index within the output stream at which to read.
        /// </param>
        /// <param name="outputValue">
        /// The resulting discrete unit of output.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the read operation was performed, otherwise <see langword="false" />.
        /// </returns>
        protected abstract Boolean TryRead(Int32 index, out T outputValue);

        /// <summary>
        /// Attempts to read a range of discrete units of output from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="startIndex">
        /// A zero-based index within the output stream at which to being reading.
        /// </param>
        /// <param name="count">
        /// The number of output values to read.
        /// </param>
        /// <param name="outputRange">
        /// An array into which the output range should be filled or copied.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the read operation was performed, otherwise <see langword="false" />.
        /// </returns>
        protected abstract Boolean TryRead(Int32 startIndex, Int32 count, T[] outputRange);

        /// <summary>
        /// Gets a unique identifier for the current <see cref="Channel{T}" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the behavior of the channel when an out-of-range read request is made.
        /// </summary>
        public InvalidReadBehavior InvalidReadBehavior
        {
            get;
        }

        /// <summary>
        /// Gets the name of the current <see cref="Channel{T}" />.
        /// </summary>
        public String Name
        {
            get;
        }

        /// <summary>
        /// Gets the number of discrete units in the output stream for the current <see cref="Channel{T}" />, or -1 if the length is
        /// not fixed.
        /// </summary>
        public virtual Int32 OutputLength => UnfixedOutputLength;

        /// <summary>
        /// Gets a value indicating whether or not the output stream for the current <see cref="Channel{T}" /> has a fixed length.
        /// </summary>
        public Boolean OutputLengthIsFixed => (OutputLength != UnfixedOutputLength);

        /// <summary>
        /// Gets the type of a discrete unit of output value for the current <see cref="Channel{T}" />.
        /// </summary>
        public Type OutputType
        {
            get;
        }

        /// <summary>
        /// Gets the status of the current <see cref="IChannel" />.
        /// </summary>
        public ChannelStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents the default behavior of a channel when an out-of-range read request is made.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const InvalidReadBehavior DefaultInvalidReadBehavior = InvalidReadBehavior.ReadSilence;

        /// <summary>
        /// Represents the value that is used to indicate that <see cref="OutputLength" /> is unbounded or unspecified.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 UnfixedOutputLength = -1;
    }
}