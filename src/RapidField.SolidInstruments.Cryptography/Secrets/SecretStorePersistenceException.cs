// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents an exception that is raised by an <see cref="ISecretStorePersistenceVehicle{TInMemorySecretStore}" /> instance
    /// while attempting to persist an in-memory <see cref="ISecretStore" />.
    /// </summary>
    public class SecretStorePersistenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretStorePersistenceException" /> class.
        /// </summary>
        public SecretStorePersistenceException()
            : this(message: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretStorePersistenceException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public SecretStorePersistenceException(String message)
            : this(message, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretStorePersistenceException" /> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public SecretStorePersistenceException(Exception innerException)
            : this(null, innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretAccessException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public SecretStorePersistenceException(String message, Exception innerException)
            : base(message.IsNullOrEmpty() ? "An exception was raised while attempting to persist the in-memory secret store." : message, innerException)
        {
            return;
        }
    }
}