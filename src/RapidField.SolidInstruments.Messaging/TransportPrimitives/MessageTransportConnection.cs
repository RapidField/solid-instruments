// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client connection to an <see cref="IMessageTransport" />.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageTransportConnection" /> is the default implementation of <see cref="IMessageTransportConnection" />.
    /// </remarks>
    internal sealed class MessageTransportConnection : Instrument, IMessageTransportConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportConnection" /> class.
        /// </summary>
        /// <param name="transport">
        /// The associated transport.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="transport" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTransportConnection(IMessageTransport transport)
            : base()
        {
            Identifier = Guid.NewGuid();
            State = MessageTransportConnectionState.Open;
            TransportReference = transport.RejectIf().IsNull(nameof(transport)).TargetArgument;
        }

        /// <summary>
        /// Closes the current <see cref="MessageTransportConnection" /> as an idempotent operation.
        /// </summary>
        public void Close()
        {
            if (State == MessageTransportConnectionState.Open)
            {
                State = MessageTransportConnectionState.Closed;

                if (TransportReference.Connections.Any(connection => connection.Identifier == Identifier))
                {
                    TransportReference.CloseConnection(this);
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageTransportConnection" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Close();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets a value that uniquely identifies the current <see cref="MessageTransportConnection" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the state of the current <see cref="MessageTransportConnection" />.
        /// </summary>
        public MessageTransportConnectionState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the associated <see cref="IMessageTransport" />.
        /// </summary>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        public IMessageTransport Transport => State == MessageTransportConnectionState.Open ? TransportReference : throw new MessageTransportConnectionClosedException($"Connection {Identifier.ToSerializedString()} is closed.");

        /// <summary>
        /// Represents the associated <see cref="IMessageTransport" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IMessageTransport TransportReference;
    }
}