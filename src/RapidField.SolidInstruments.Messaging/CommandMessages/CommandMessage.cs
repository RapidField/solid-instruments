// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Runtime.Serialization;
using SolidInstrumentsCommand = RapidField.SolidInstruments.Command.Command;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command.
    /// </summary>
    /// <remarks>
    /// <see cref="CommandMessage{TCommand}" /> is the default implementation of <see cref="ICommandMessage{TCommand}" />.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    [DataContract]
    public abstract class CommandMessage<TCommand> : Message, ICommandMessage<TCommand>
        where TCommand : SolidInstrumentsCommand, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage{TCommand}" /> class.
        /// </summary>
        protected CommandMessage()
            : this(new TCommand())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage{TCommand}" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        protected CommandMessage(TCommand commandObject)
            : base(commandObject.CorrelationIdentifier, Guid.NewGuid())
        {
            Command = commandObject.RejectIf().IsNull(nameof(commandObject));
        }

        /// <summary>
        /// Gets or sets the associated command.
        /// </summary>
        [DataMember]
        public TCommand Command
        {
            get;
            set;
        }
    }
}