// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Mathematics
{
    /// <summary>
    /// Represents an exception that is raised while attempting to perform an interpolation calculation.
    /// </summary>
    public class InterpolationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterpolationException" /> class.
        /// </summary>
        public InterpolationException()
            : this(null, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterpolationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public InterpolationException(String message)
            : this(message, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterpolationException" /> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public InterpolationException(Exception innerException)
            : this(null, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterpolationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public InterpolationException(String message, Exception innerException)
            : base(message.IsNullOrEmpty() ? "An exception was raised while attempting to perform an interpolation calculation." : message, innerException)
        {
            return;
        }
    }
}