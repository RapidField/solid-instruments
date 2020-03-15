// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents the state of an <see cref="IMessageTransportConnection" />.
    /// </summary>
    public enum MessageTransportConnectionState : Int32
    {
        /// <summary>
        /// The connection's state is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The connection is open and available for use.
        /// </summary>
        Open = 1,

        /// <summary>
        /// The connection is closed and unavailable for use.
        /// </summary>
        Closed = 2
    }
}