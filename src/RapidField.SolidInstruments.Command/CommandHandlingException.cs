// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents an exception that is raised by an <see cref="ICommandHandler{TCommand}" /> instance while processing a
    /// <see cref="ICommand" />.
    /// </summary>
    public class CommandHandlingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlingException" /> class.
        /// </summary>
        public CommandHandlingException()
            : this(commandType: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlingException" />
        /// </summary>
        /// <param name="commandType">
        /// The type of the command that was being processed when the exception was raised.
        /// </param>
        public CommandHandlingException(Type commandType)
            : this(commandType: commandType, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlingException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public CommandHandlingException(String message)
            : this(message: message, innerException: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlingException" />
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public CommandHandlingException(Exception innerException)
            : this(commandType: null, innerException: innerException)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlingException" />
        /// </summary>
        /// <param name="commandType">
        /// The type of the command that was being processed when the exception was raised.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public CommandHandlingException(Type commandType, Exception innerException)
            : this(commandType is null ? "An exception was raised while processing a command." : $"An exception was raised while processing a command instance of type {commandType.FullName}.", innerException)
        {
            CommandType = commandType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlingException" /> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public CommandHandlingException(String message, Exception innerException)
            : base(message, innerException)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the command that was being processed when the exception was raised.
        /// </summary>
        public Type CommandType
        {
            get;
        }
    }
}