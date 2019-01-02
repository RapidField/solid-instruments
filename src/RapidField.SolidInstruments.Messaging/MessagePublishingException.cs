// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an exception that is raised when a message publishing operation fails.
    /// </summary>
    public class MessagePublishingException : MessagingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingException" /> class.
        /// </summary>
        public MessagePublishingException()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingException" />
        /// </summary>
        /// <param name="messageType">
        /// The type of the message that was being processed when the exception was raised.
        /// </param>
        public MessagePublishingException(Type messageType)
            : base(messageType)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public MessagePublishingException(String message)
            : base(message)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingException" />
        /// </summary>
        /// <param name="messageType">
        /// The type of the message that was being processed when the exception was raised.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessagePublishingException(Type messageType, Exception innerException)
            : base(messageType, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublishingException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessagePublishingException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }
    }
}