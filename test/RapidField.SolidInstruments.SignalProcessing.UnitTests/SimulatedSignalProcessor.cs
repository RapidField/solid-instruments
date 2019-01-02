// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.SignalProcessing.UnitTests
{
    /// <summary>
    /// Represents a <see cref="SignalProcessor{TOutput, TSettings, TInputChannels}" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedSignalProcessor : SignalProcessor<Int32, SimulatedSignalProcessorSettings, SimulatedChannelCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedSignalProcessor" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first input channel.
        /// </param>
        /// <param name="channelB">
        /// The second input channel.
        /// </param>
        /// <param name="settings">
        /// The operational settings for the signal processor.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="channelA" /> is <see langword="null" /> -or- <paramref name="channelB" /> is <see langword="null" /> -or-
        /// <paramref name="settings" /> is <see langword="null" />.
        /// </exception>
        public SimulatedSignalProcessor(Int32[] channelA, Int32[] channelB, SimulatedSignalProcessorSettings settings)
            : base(new SimulatedChannelCollection(new ObjectCollectionChannel<Int32>(channelA), new ObjectCollectionChannel<Int32>(channelB)), settings)
        {
            return;
        }

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
        protected sealed override Boolean TryRead(SimulatedChannelCollection inputChannels, SimulatedSignalProcessorSettings settings, Int32 index, out Int32 outputValue)
        {
            var channelAReadTask = inputChannels.ChannelA.ReadAsync(index);
            var channelBReadTask = inputChannels.ChannelB.ReadAsync(index);
            Task.WaitAll(channelAReadTask, channelBReadTask);
            outputValue = (settings.Factor * (channelAReadTask.Result.Value + channelBReadTask.Result.Value));
            return true;
        }
    }
}