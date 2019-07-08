// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an exception that is raised when a message requesting operation fails.
    /// </summary>
    public class MessageRequestingException : MessagingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestingException" /> class.
        /// </summary>
        public MessageRequestingException()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestingException" />
        /// </summary>
        /// <param name="requestMessageType">
        /// The type of the request message that was being processed when the exception was raised.
        /// </param>
        public MessageRequestingException(Type requestMessageType)
            : base(requestMessageType)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestingException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public MessageRequestingException(String message)
            : base(message)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestingException" />
        /// </summary>
        /// <param name="requestMessageType">
        /// The type of the request message that was being processed when the exception was raised.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessageRequestingException(Type requestMessageType, Exception innerException)
            : base(requestMessageType, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestingException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessageRequestingException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }
    }
}