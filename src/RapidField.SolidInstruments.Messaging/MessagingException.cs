// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an exception that is raised when a messaging operation fails.
    /// </summary>
    public class MessagingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingException" /> class.
        /// </summary>
        public MessagingException()
            : this(messageType: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingException" />
        /// </summary>
        /// <param name="messageType">
        /// The type of the message that was being processed when the exception was raised.
        /// </param>
        public MessagingException(Type messageType)
            : this(messageType: messageType, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public MessagingException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingException" />
        /// </summary>
        /// <param name="messageType">
        /// The type of the message that was being processed when the exception was raised.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessagingException(Type messageType, Exception innerException)
            : this(messageType is null ? "An exception was raised while processing a message." : $"An exception was raised while processing a message instance of type {messageType}.", innerException)
        {
            MessageType = messageType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public MessagingException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the message that was being processed when the exception was raised.
        /// </summary>
        public Type MessageType
        {
            get;
        }
    }
}