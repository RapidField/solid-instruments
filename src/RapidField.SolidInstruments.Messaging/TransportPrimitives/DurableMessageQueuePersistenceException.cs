// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents an exception that is raised when an <see cref="IDurableMessageQueuePersistenceProxy" /> is unable to perform a
    /// state persistence operation.
    /// </summary>
    public sealed class DurableMessageQueuePersistenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueuePersistenceException" /> class.
        /// </summary>
        public DurableMessageQueuePersistenceException()
            : base(DefaultMessage)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueuePersistenceException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public DurableMessageQueuePersistenceException(String message)
            : base(message)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueuePersistenceException" /> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public DurableMessageQueuePersistenceException(Exception innerException)
            : base(DefaultMessage, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueuePersistenceException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public DurableMessageQueuePersistenceException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Represents the default message for the exception.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultMessage = "A queue state persistence operation failed.";
    }
}