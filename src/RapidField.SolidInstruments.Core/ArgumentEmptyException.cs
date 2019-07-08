// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an exception that is raised when an empty, non-null data reference is passed to a method that does not accept it
    /// as a valid argument.
    /// </summary>
    public sealed class ArgumentEmptyException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEmptyException" /> class.
        /// </summary>
        public ArgumentEmptyException()
            : base(ConstructMessage(null))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEmptyException" /> class.
        /// </summary>
        /// <param name="parameterName">
        /// The name of the parameter that caused the exception.
        /// </param>
        public ArgumentEmptyException(String parameterName)
            : base(ConstructMessage(parameterName), parameterName, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEmptyException" /> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ArgumentEmptyException(Exception innerException)
            : base(ConstructMessage(null), null, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEmptyException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ArgumentEmptyException(String message, Exception innerException)
            : base(message, null, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEmptyException" /> class.
        /// </summary>
        /// <param name="parameterName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ArgumentEmptyException(String parameterName, String message)
            : base(message, parameterName, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEmptyException" /> class.
        /// </summary>
        /// <param name="parameterName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ArgumentEmptyException(String parameterName, String message, Exception innerException)
            : base(message, parameterName, innerException)
        {
            return;
        }

        /// <summary>
        /// Constructs a message for the exception.
        /// </summary>
        /// <param name="parameterName">
        /// The name of the parameter which caused the current exception to be raised.
        /// </param>
        /// <returns>
        /// A generic message if <paramref name="parameterName" /> is null, otherwise a detailed message containing the provided
        /// value.
        /// </returns>
        [DebuggerHidden]
        private static String ConstructMessage(String parameterName) => parameterName is null ? "The specified argument is empty." : $"The {parameterName} argument is empty.";
    }
}