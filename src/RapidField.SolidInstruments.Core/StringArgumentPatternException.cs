// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an exception that is raised when a method is supplied with an <see cref="String" /> with an invalid pattern.
    /// </summary>
    public sealed class StringArgumentPatternException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringArgumentPatternException" /> class.
        /// </summary>
        public StringArgumentPatternException()
            : this(null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringArgumentPatternException" /> class.
        /// </summary>
        /// <param name="parameterName">
        /// The name of the parameter that caused the exception.
        /// </param>
        public StringArgumentPatternException(String parameterName)
            : this(null, parameterName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringArgumentPatternException" /> class.
        /// </summary>
        /// <param name="failedMatchingPattern">
        /// The regular expression pattern that the <see cref="String" /> failed to match, causing the current exception to be
        /// raised.
        /// </param>
        /// <param name="parameterName">
        /// The name of the parameter that caused the exception.
        /// </param>
        public StringArgumentPatternException(String failedMatchingPattern, String parameterName)
            : this(failedMatchingPattern, parameterName, null, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringArgumentPatternException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public StringArgumentPatternException(String message, Exception innerException)
            : this(null, message, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringArgumentPatternException" /> class.
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
        public StringArgumentPatternException(String parameterName, String message, Exception innerException)
            : this(null, parameterName, message, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringArgumentPatternException" /> class.
        /// </summary>
        /// <param name="failedMatchingPattern">
        /// The regular expression pattern that the <see cref="String" /> failed to match, causing the current exception to be
        /// raised.
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
        public StringArgumentPatternException(String failedMatchingPattern, String parameterName, String message, Exception innerException)
            : base(message.IsNullOrEmpty() ? ConstructMessage(parameterName, failedMatchingPattern) : message, parameterName, innerException)
        {
            FailedMatchingPattern = failedMatchingPattern;
        }

        /// <summary>
        /// Constructs a message for the exception.
        /// </summary>
        /// <param name="parameterName">
        /// The name of the parameter which caused the current exception to be raised.
        /// </param>
        /// <param name="failedMatchingPattern">
        /// The regular expression pattern that the <see cref="String" /> failed to match, causing the current exception to be
        /// raised.
        /// </param>
        /// <returns>
        /// A message for the exception.
        /// </returns>
        [DebuggerHidden]
        private static String ConstructMessage(String parameterName, String failedMatchingPattern)
        {
            if (parameterName.IsNullOrEmpty())
            {
                return "A string argument was specified which does not match the required pattern.";
            }
            else if (failedMatchingPattern.IsNullOrEmpty())
            {
                return $"The string argument for {parameterName} does not match the required pattern.";
            }

            return $"The string argument for {parameterName} does not match the required pattern: {failedMatchingPattern}";
        }

        /// <summary>
        /// Gets the regular expression pattern that the <see cref="String" /> failed to match, causing the current exception to be
        /// raised.
        /// </summary>
        public String FailedMatchingPattern
        {
            get;
        }
    }
}