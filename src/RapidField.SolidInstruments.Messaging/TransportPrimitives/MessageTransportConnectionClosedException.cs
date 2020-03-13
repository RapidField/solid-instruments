// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents an exception that is raised when an attempt is made to perform a messaging operation against a closed
    /// <see cref="IMessageTransportConnection" />.
    /// </summary>
    public class MessageTransportConnectionClosedException : MessagingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportConnectionClosedException" /> class.
        /// </summary>
        public MessageTransportConnectionClosedException()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportConnectionClosedException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public MessageTransportConnectionClosedException(String message)
            : base(message)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportConnectionClosedException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessageTransportConnectionClosedException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }
    }
}