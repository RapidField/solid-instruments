// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Converts one or more input signals to a single output signal.
    /// </summary>
    /// <typeparam name="TOutput">
    /// The type of the signal processor's output value.
    /// </typeparam>
    /// <typeparam name="TSettings">
    /// The type of the operational settings for the signal processor.
    /// </typeparam>
    public interface ISignalProcessor<TOutput, TSettings> : IChannel<TOutput>
        where TSettings : SignalProcessorSettings, new()
    {
        /// <summary>
        /// Gets the input channels for the current <see cref="ISignalProcessor{TOutput, TSettings}" />.
        /// </summary>
        IChannelCollection InputChannels
        {
            get;
        }

        /// <summary>
        /// Gets the operational settings for the current <see cref="ISignalProcessor{TOutput, TSettings}" />.
        /// </summary>
        TSettings Settings
        {
            get;
        }
    }
}