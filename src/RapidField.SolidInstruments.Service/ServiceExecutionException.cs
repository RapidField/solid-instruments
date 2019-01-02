// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Service
{
    /// <summary>
    /// Represents an exception that is raised during service execution.
    /// </summary>
    public class ServiceExectuionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExectuionException" /> class.
        /// </summary>
        public ServiceExectuionException()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExectuionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ServiceExectuionException(String message)
            : base(message)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExectuionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ServiceExectuionException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }
    }
}