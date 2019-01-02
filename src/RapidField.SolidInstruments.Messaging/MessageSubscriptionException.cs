// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an exception that is raised when a message subscription operation fails.
    /// </summary>
    public class MessageSubscriptionException : MessagingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriptionException" /> class.
        /// </summary>
        public MessageSubscriptionException()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriptionException" />
        /// </summary>
        /// <param name="messageType">
        /// The type of the message that was being processed when the exception was raised.
        /// </param>
        public MessageSubscriptionException(Type messageType)
            : base(messageType)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriptionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public MessageSubscriptionException(String message)
            : base(message)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriptionException" />
        /// </summary>
        /// <param name="messageType">
        /// The type of the message that was being processed when the exception was raised.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessageSubscriptionException(Type messageType, Exception innerException)
            : base(messageType, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriptionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessageSubscriptionException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }
    }
}