// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents an exception that is raised by an <see cref="IObjectFactory" /> or an <see cref="IObjectContainer" /> instance
    /// during object production.
    /// </summary>
    public class ObjectProductionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProductionException" /> class.
        /// </summary>
        public ObjectProductionException()
            : this(objectType: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProductionException" />
        /// </summary>
        /// <param name="objectType">
        /// The type of the object that could not be produced.
        /// </param>
        public ObjectProductionException(Type objectType)
            : this(objectType: objectType, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProductionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ObjectProductionException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProductionException" />
        /// </summary>
        /// <param name="objectType">
        /// The type of the object that could not be produced.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ObjectProductionException(Type objectType, Exception innerException)
            : this(objectType is null ? "An exception was raised during production of an instance." : $"An exception was raised during production of an instance of type {objectType}.", innerException)
        {
            ObjectType = objectType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProductionException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ObjectProductionException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the object that could not be produced.
        /// </summary>
        public Type ObjectType
        {
            get;
        }
    }
}