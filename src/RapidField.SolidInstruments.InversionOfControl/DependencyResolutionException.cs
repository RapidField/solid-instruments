// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents an exception that is raised by an <see cref="IDependencyScope" /> while attempting to resolve a dependency.
    /// </summary>
    public class DependencyResolutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolutionException" /> class.
        /// </summary>
        public DependencyResolutionException()
            : this(dependencyType: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolutionException" />
        /// </summary>
        /// <param name="dependencyType">
        /// The type of the dependency that was being resolved when the exception was raised.
        /// </param>
        public DependencyResolutionException(Type dependencyType)
            : this(dependencyType: dependencyType, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolutionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public DependencyResolutionException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolutionException" />
        /// </summary>
        /// <param name="dependencyType">
        /// The type of the dependency that was being resolved when the exception was raised.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public DependencyResolutionException(Type dependencyType, Exception innerException)
            : this(dependencyType is null ? "An exception was raised while resolving a dependency." : $"An exception was raised while resolving a dependency of type {dependencyType.FullName}.", innerException)
        {
            DependencyType = dependencyType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolutionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public DependencyResolutionException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the dependency that was being resolved when the exception was raised.
        /// </summary>
        public Type DependencyType
        {
            get;
        }
    }
}