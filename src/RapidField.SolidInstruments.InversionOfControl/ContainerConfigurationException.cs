// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents an exception that is raised while attempting to configure an <see cref="IDependencyContainer" />.
    /// </summary>
    public class ContainerConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDependencyScopeException" /> class.
        /// </summary>
        public ContainerConfigurationException()
            : this(message: null, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDependencyScopeException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ContainerConfigurationException(String message)
            : this(message, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDependencyScopeException" /> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ContainerConfigurationException(Exception innerException)
            : this(null, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDependencyScopeException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ContainerConfigurationException(String message, Exception innerException)
            : base(message ?? "An exception was raised while attempting to configure a dependency container. See inner exception for details.", innerException)
        {
            return;
        }
    }
}