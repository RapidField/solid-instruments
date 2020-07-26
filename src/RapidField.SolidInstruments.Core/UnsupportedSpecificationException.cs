// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an exception that is raised when a method or type is supplied with unsupported or unrecognized specification(s).
    /// </summary>
    public sealed class UnsupportedSpecificationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedSpecificationException" /> class.
        /// </summary>
        public UnsupportedSpecificationException()
            : this(unsupportedSpecification: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedSpecificationException" /> class.
        /// </summary>
        /// <param name="unsupportedSpecification">
        /// The unsupported specification(s) that caused the exception.
        /// </param>
        public UnsupportedSpecificationException(Object unsupportedSpecification)
            : this(unsupportedSpecification, null, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedSpecificationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public UnsupportedSpecificationException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedSpecificationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public UnsupportedSpecificationException(String message, Exception innerException)
            : this(unsupportedSpecification: null, message: message, innerException: innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedSpecificationException" /> class.
        /// </summary>
        /// <param name="unsupportedSpecification">
        /// The unsupported specification(s) that caused the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public UnsupportedSpecificationException(Object unsupportedSpecification, Exception innerException)
            : this(unsupportedSpecification, null, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedSpecificationException" /> class.
        /// </summary>
        /// <param name="unsupportedSpecification">
        /// The unsupported specification(s) that caused the exception.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public UnsupportedSpecificationException(Object unsupportedSpecification, String message, Exception innerException)
            : base(message.IsNullOrEmpty() ? ConstructMessage(unsupportedSpecification) : message, innerException)
        {
            UnsupportedSpecification = unsupportedSpecification;
        }

        /// <summary>
        /// Constructs a message for the exception.
        /// </summary>
        /// <param name="unsupportedSpecification">
        /// The unsupported specification(s) that caused the exception.
        /// </param>
        /// <returns>
        /// A message for the exception.
        /// </returns>
        [DebuggerHidden]
        private static String ConstructMessage(Object unsupportedSpecification)
        {
            if (unsupportedSpecification is null)
            {
                return "The supplied specification(s) are not supported.";
            }

            return $"The supplied specification(s) are not supported: {unsupportedSpecification}.";
        }

        /// <summary>
        /// Gets the unsupported specification(s) that caused the current exception to be raised.
        /// </summary>
        public Object UnsupportedSpecification
        {
            get;
        }
    }
}