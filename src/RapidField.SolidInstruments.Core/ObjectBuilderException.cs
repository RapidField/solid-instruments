// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an exception that is raised when an error occurs during configuration or finalization of an
    /// <see cref="IObjectBuilder{TResult}" /> instance.
    /// </summary>
    public class ObjectBuilderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" /> class.
        /// </summary>
        public ObjectBuilderException()
            : this(builderType: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" />
        /// </summary>
        /// <param name="builderType">
        /// The type of the <see cref="IObjectBuilder{TResult}" /> that raised the exception.
        /// </param>
        public ObjectBuilderException(Type builderType)
            : this(builderType: builderType, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ObjectBuilderException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationException" />
        /// </summary>
        /// <param name="builderType">
        /// The type of the <see cref="IObjectBuilder{TResult}" /> that raised the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ObjectBuilderException(Type builderType, Exception innerException)
            : this(builderType is null ? "An exception was raised during configuration finalization of an instance." : $"An exception was raised during configuration or finalization of an instance of type {builderType}.", innerException)
        {
            BuilderType = builderType;
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
        public ObjectBuilderException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the <see cref="IObjectBuilder{TResult}" /> that raised the exception.
        /// </summary>
        public Type BuilderType
        {
            get;
        }
    }
}