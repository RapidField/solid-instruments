// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client connection to an <see cref="IMessageTransport" />.
    /// </summary>
    internal interface IMessageTransportConnection : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Closes the current <see cref="IMessageTransportConnection" /> as an idempotent operation.
        /// </summary>
        public void Close();

        /// <summary>
        /// Gets a value that uniquely identifies the current <see cref="IMessageTransportConnection" />.
        /// </summary>
        Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the state of the current <see cref="IMessageTransportConnection" />.
        /// </summary>
        MessageTransportConnectionState State
        {
            get;
        }

        /// <summary>
        /// Gets the associated <see cref="IMessageTransport" />.
        /// </summary>
        IMessageTransport Transport
        {
            get;
        }
    }
}