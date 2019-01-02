// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Represents an exception that is raised when an operation performed by an <see cref="IConcurrencyControl" /> is invalid for
    /// the control's current state.
    /// </summary>
    public class ConcurrencyControlOperationException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyControlOperationException" /> class.
        /// </summary>
        public ConcurrencyControlOperationException()
            : this(null, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyControlOperationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ConcurrencyControlOperationException(String message)
            : this(message, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyControlOperationException" /> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ConcurrencyControlOperationException(Exception innerException)
            : this(null, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyControlOperationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ConcurrencyControlOperationException(String message, Exception innerException)
            : base((message ?? ConstructMessage(innerException is null == false)), innerException)
        {
            return;
        }

        /// <summary>
        /// Constructs a message for the exception.
        /// </summary>
        /// <param name="innerExceptionIsSpecified">
        /// The value indicating whether or not an inner exception was specified.
        /// </param>
        /// <returns>
        /// A generic message.
        /// </returns>
        [DebuggerHidden]
        private static String ConstructMessage(Boolean innerExceptionIsSpecified) => innerExceptionIsSpecified ? String.Concat(DefaultMessage, " See the inner exception for details.") : DefaultMessage;

        /// <summary>
        /// Represents the default message for the exception.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultMessage = "A concurrency control operation failed.";
    }
}