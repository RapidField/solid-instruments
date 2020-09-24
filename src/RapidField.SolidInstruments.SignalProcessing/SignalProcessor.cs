// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Converts one or more input signals to a single output signal.
    /// </summary>
    /// <remarks>
    /// <see cref="SignalProcessor{TOutput, TSettings, TInputChannels}" /> is the default implementation of
    /// <see cref="ISignalProcessor{TOutput, TSettings}" />.
    /// </remarks>
    /// <typeparam name="TOutput">
    /// The type of the signal processor's output value.
    /// </typeparam>
    /// <typeparam name="TSettings">
    /// The type of the operational settings for the signal processor.
    /// </typeparam>
    /// <typeparam name="TInputChannels">
    /// The type of the channel input collection for the signal processor.
    /// </typeparam>
    public abstract class SignalProcessor<TOutput, TSettings, TInputChannels> : Channel<TOutput>, ISignalProcessor<TOutput, TSettings>
        where TSettings : SignalProcessorSettings, new()
        where TInputChannels : class, IChannelCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignalProcessor{TOutput, TSettings, TInputChannels}" /> class.
        /// </summary>
        /// <param name="inputChannels">
        /// The input channels for the signal processor.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inputChannels" /> is <see langword="null" />.
        /// </exception>
        protected SignalProcessor(TInputChannels inputChannels)
            : this(inputChannels, new TSettings())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalProcessor{TOutput, TSettings, TInputChannels}" /> class.
        /// </summary>
        /// <param name="inputChannels">
        /// The input channels for the signal processor.
        /// </param>
        /// <param name="settings">
        /// The operational settings for the signal processor.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inputChannels" /> is <see langword="null" /> -or- <paramref name="settings" /> is
        /// <see langword="null" />.
        /// </exception>
        protected SignalProcessor(TInputChannels inputChannels, TSettings settings)
            : this(inputChannels, settings, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalProcessor{TOutput, TSettings, TInputChannels}" /> class.
        /// </summary>
        /// <param name="inputChannels">
        /// The input channels for the signal processor.
        /// </param>
        /// <param name="settings">
        /// The operational settings for the signal processor.
        /// </param>
        /// <param name="name">
        /// The name of the signal processor, or <see langword="null" /> to use the name of the signal processor type. The default
        /// value is <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inputChannels" /> is <see langword="null" /> -or- <paramref name="settings" /> is
        /// <see langword="null" />.
        /// </exception>
        protected SignalProcessor(TInputChannels inputChannels, TSettings settings, String name)
            : base(name)
        {
            InputChannelsReference = inputChannels.RejectIf().IsNull(nameof(inputChannels));
            Settings = settings.RejectIf().IsNull(nameof(settings));
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Channel{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Attempts to read a discrete unit of output from the channel's output stream at the specified index.
        /// </summary>
        /// <param name="inputChannels">
        /// The processor's input channels.
        /// </param>
        /// <param name="settings">
        /// The processor's operational settings.
        /// </param>
        /// <param name="index">
        /// A zero-based index within the output stream at which to read.
        /// </param>
        /// <param name="outputValue">
        /// The resulting discrete unit of output.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the read operation was performed, otherwise <see langword="false" />.
        /// </returns>
        protected abstract Boolean TryRead(TInputChannels inputChannels, TSettings settings, Int32 index, out TOutput outputValue);

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
        protected sealed override Boolean TryRead(Int32 index, out TOutput outputValue) => TryRead(InputChannelsReference, Settings, index, out outputValue);

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
        protected sealed override Boolean TryRead(Int32 startIndex, Int32 count, TOutput[] outputRange)
        {
            for (var i = 0; i < count; i++)
            {
                var index = startIndex + i;

                if (TryRead(index, out var outputValue))
                {
                    outputRange[i] = outputValue;
                    continue;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the input channels for the current <see cref="SignalProcessor{TOutput, TSettings, TInputChannels}" />.
        /// </summary>
        public IChannelCollection InputChannels => InputChannelsReference;

        /// <summary>
        /// Gets the operational settings for the current <see cref="SignalProcessor{TOutput, TSettings, TInputChannels}" />.
        /// </summary>
        public TSettings Settings
        {
            get;
        }

        /// <summary>
        /// Represents the input channels for the current <see cref="SignalProcessor{TOutput, TSettings, TInputChannels}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TInputChannels InputChannelsReference;
    }
}