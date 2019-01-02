// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.SignalProcessing.UnitTests
{
    /// <summary>
    /// Represents a <see cref="ChannelCollection" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedChannelCollection : ChannelCollection<ObjectCollectionChannel<Int32>, ObjectCollectionChannel<Int32>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedChannelCollection" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public SimulatedChannelCollection(ObjectCollectionChannel<Int32> channelA, ObjectCollectionChannel<Int32> channelB)
            : base(channelA, channelB)
        {
            return;
        }
    }
}