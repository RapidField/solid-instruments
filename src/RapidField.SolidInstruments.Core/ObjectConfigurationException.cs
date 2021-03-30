// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an exception that is raised when an error occurs during configuration of a
    /// <see cref="ConfigurableInstrument{TConfiguration}" /> instance.
    /// </summary>
    public class ObjectConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" /> class.
        /// </summary>
        public ObjectConfigurationException()
            : this(instrumentType: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" />
        /// </summary>
        /// <param name="instrumentType">
        /// The type of the <see cref="ConfigurableInstrument{TConfiguration}" /> that raised the exception.
        /// </param>
        public ObjectConfigurationException(Type instrumentType)
            : this(instrumentType: instrumentType, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ObjectConfigurationException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" />
        /// </summary>
        /// <param name="instrumentType">
        /// The type of the <see cref="ConfigurableInstrument{TConfiguration}" /> that raised the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ObjectConfigurationException(Type instrumentType, Exception innerException)
            : this(instrumentType is null ? "An exception was raised during configuration of an instance." : $"An exception was raised during configuration of an instance of type {instrumentType}.", innerException)
        {
            InstrumentType = instrumentType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ObjectConfigurationException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the <see cref="ConfigurableInstrument{TConfiguration}" /> that raised the exception.
        /// </summary>
        public Type InstrumentType
        {
            get;
        }
    }
}