// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Runtime.Serialization;
using SolidInstrumentsCommand = RapidField.SolidInstruments.Command.Command;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command related to domain logic and which can be described by an <see cref="IEvent" />.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainCommandMessage{TCommand, TEvent, TEventMessage}" /> is a derivative of
    /// <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" /> which is extended to support
    /// <see cref="IReportableCommandMessage{TCommand, TEvent, TEventMessage}" />.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the event that describes the command.
    /// </typeparam>
    /// <typeparam name="TEventMessage">
    /// The type of the event message that is published by the processor if <see cref="IReportableCommandMessage.IsReported" /> is
    /// <see langword="true" />.
    /// </typeparam>
    [DataContract]
    public abstract class DomainCommandMessage<TCommand, TEvent, TEventMessage> : CommandMessage<TCommand, TEvent, TEventMessage>, IDomainCommandMessage<TCommand>
        where TEvent : Event, IDomainEvent, new()
        where TCommand : SolidInstrumentsCommand, IDomainReportableCommand<TEvent>, new()
        where TEventMessage : EventMessage<TEvent>, IDomainEventMessage<TEvent>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommandMessage{TCommand, TEvent, TEventMessage}" /> class.
        /// </summary>
        protected DomainCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommandMessage{TCommand, TEvent, TEventMessage}" /> class.
        /// </summary>
        /// <param name="isReported">
        /// A value indicating whether or not the command should be reported with an event message by its processor. The default
        /// value is <see langword="true" />.
        /// </param>
        protected DomainCommandMessage(Boolean isReported)
            : base(isReported)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommandMessage{TCommand, TEvent, TEventMessage}" /> class.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommandMessage{TCommand, TEvent, TEventMessage}" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <param name="isReported">
        /// A value indicating whether or not the command should be reported with an event message by its processor. The default
        /// value is <see langword="true" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainCommandMessage(TCommand commandObject, Boolean isReported)
            : base(commandObject, isReported)
        {
            return;
        }

        /// <summary>
        /// Hydrates and enriches the specified event message with details that are pertinent to the current
        /// <see cref="DomainCommandMessage{TCommand, TEvent, TEventMessage}" />.
        /// </summary>
        /// <param name="associatedEventMessage">
        /// The event message to enrich.
        /// </param>
        protected override void EnrichEventMessage(TEventMessage associatedEventMessage) => base.EnrichEventMessage(associatedEventMessage);
    }

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