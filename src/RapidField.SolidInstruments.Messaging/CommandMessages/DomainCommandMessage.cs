// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command related to domain logic.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainCommandMessage{TCommand}" /> is the default implementation of
    /// <see cref="IDomainCommandMessage{TCommand}" />.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    [DataContract]
    public abstract class DomainCommandMessage<TCommand> : CommandMessage<TCommand>, IDomainCommandMessage<TCommand>
        where TCommand : DomainCommand, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommandMessage{TCommand}" /> class.
        /// </summary>
        protected DomainCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommandMessage{TCommand}" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainCommandMessage(TCommand commandObject)
            : base(commandObject)
        {
            return;
        }
    }
}