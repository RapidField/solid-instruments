// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Web
{
    /// <summary>
    /// Represents an exception that is raised during web application execution.
    /// </summary>
    public class WebExectuionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebExectuionException" /> class.
        /// </summary>
        public WebExectuionException()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebExectuionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public WebExectuionException(String message)
            : base(message)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebExectuionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public WebExectuionException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }
    }
}