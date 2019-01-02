// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Specifies the status of an <see cref="IChannel" />.
    /// </summary>
    public enum ChannelStatus : Int32
    {
        /// <summary>
        /// The status of the channel is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The channel is available and operable.
        /// </summary>
        Live = 1,

        /// <summary>
        /// The channel is available but deliberately silent.
        /// </summary>
        Silent = 2,

        /// <summary>
        /// The channel is unavailable.
        /// </summary>
        Unavailable = 3
    }
}