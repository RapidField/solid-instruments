// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an exception that is raised when a method is supplied with an unsupported <see cref="Type" />.
    /// </summary>
    public sealed class UnsupportedTypeArgumentException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        public UnsupportedTypeArgumentException()
            : this(unsupportedTypeArgument: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        /// <param name="unsupportedTypeArgument">
        /// The unsupported <see cref="Type" /> argument that caused the exception.
        /// </param>
        public UnsupportedTypeArgumentException(Type unsupportedTypeArgument)
            : this(unsupportedTypeArgument, parameterName: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public UnsupportedTypeArgumentException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        /// <param name="unsupportedTypeArgument">
        /// The unsupported <see cref="Type" /> argument that caused the exception.
        /// </param>
        /// <param name="parameterName">
        /// The name of the parameter that caused the exception.
        /// </param>
        public UnsupportedTypeArgumentException(Type unsupportedTypeArgument, String parameterName)
            : this(unsupportedTypeArgument, parameterName, message: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public UnsupportedTypeArgumentException(String message, Exception innerException)
            : this(unsupportedTypeArgument: null, parameterName: null, message: message, innerException: innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        /// <param name="unsupportedTypeArgument">
        /// The unsupported <see cref="Type" /> argument that caused the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public UnsupportedTypeArgumentException(Type unsupportedTypeArgument, Exception innerException)
            : this(unsupportedTypeArgument, null, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        /// <param name="unsupportedTypeArgument">
        /// The unsupported <see cref="Type" /> argument that caused the exception.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public UnsupportedTypeArgumentException(Type unsupportedTypeArgument, String message, Exception innerException)
            : this(unsupportedTypeArgument, null, message, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        /// <param name="unsupportedTypeArgument">
        /// The unsupported <see cref="Type" /> argument that caused the exception.
        /// </param>
        /// <param name="parameterName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public UnsupportedTypeArgumentException(Type unsupportedTypeArgument, String parameterName, String message)
            : this(unsupportedTypeArgument, parameterName, message, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTypeArgumentException" /> class.
        /// </summary>
        /// <param name="unsupportedTypeArgument">
        /// The unsupported <see cref="Type" /> argument that caused the exception.
        /// </param>
        /// <param name="parameterName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public UnsupportedTypeArgumentException(Type unsupportedTypeArgument, String parameterName, String message, Exception innerException)
            : base(message.IsNullOrEmpty() ? ConstructMessage(unsupportedTypeArgument, parameterName) : message, parameterName, innerException)
        {
            UnsupportedTypeArgument = unsupportedTypeArgument;
        }

        /// <summary>
        /// Constructs a message for the exception.
        /// </summary>
        /// <param name="unsupportedTypeArgument">
        /// The unsupported <see cref="Type" /> argument that caused the exception.
        /// </param>
        /// <param name="parameterName">
        /// The name of the parameter which caused the current exception to be raised.
        /// </param>
        /// <returns>
        /// A message for the exception.
        /// </returns>
        [DebuggerHidden]
        private static String ConstructMessage(Type unsupportedTypeArgument, String parameterName)
        {
            if (unsupportedTypeArgument is null)
            {
                return "The specified type argument is unsupported.";
            }
            else if (parameterName.IsNullOrEmpty())
            {
                return $"{unsupportedTypeArgument} is an unsupported type argument.";
            }

            return $"{unsupportedTypeArgument} is an unsupported type argument for {parameterName}";
        }

        /// <summary>
        /// Gets the unsupported <see cref="Type" /> argument that caused the current exception to be raised.
        /// </summary>
        public Type UnsupportedTypeArgument
        {
            get;
        }
    }
}