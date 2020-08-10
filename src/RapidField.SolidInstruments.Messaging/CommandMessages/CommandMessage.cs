// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using SolidInstrumentsCommand = RapidField.SolidInstruments.Command.Command;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command that can be described by an <see cref="IEvent" />.
    /// </summary>
    /// <remarks>
    /// <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" /> is the default implementation of
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
    public abstract class CommandMessage<TCommand, TEvent, TEventMessage> : CommandMessage<TCommand>, IReportableCommandMessage<TCommand, TEvent, TEventMessage>
        where TEvent : Event, new()
        where TCommand : SolidInstrumentsCommand, IReportableCommand<TEvent>, new()
        where TEventMessage : EventMessage<TEvent>, IEventMessage<TEvent>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" /> class.
        /// </summary>
        protected CommandMessage()
            : this(DefaultIsReportedValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" /> class.
        /// </summary>
        /// <param name="isReported">
        /// A value indicating whether or not the command should be reported with an event message by its processor. The default
        /// value is <see langword="true" />.
        /// </param>
        protected CommandMessage(Boolean isReported)
            : base()
        {
            IsReported = isReported;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        protected CommandMessage(TCommand commandObject)
            : this(commandObject, DefaultIsReportedValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" /> class.
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
        protected CommandMessage(TCommand commandObject, Boolean isReported)
            : base(commandObject)
        {
            IsReported = isReported;
        }

        /// <summary>
        /// Composes an <see cref="IEventMessage{TEvent}" /> representing information about the current
        /// <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="IEventMessage{TEvent}" /> representing information about the current object.
        /// </returns>
        /// <exception cref="EventAuthoringException">
        /// An exception was raised while composing the event.
        /// </exception>
        public TEventMessage ComposeEventMessage()
        {
            try
            {
                var associatedEventMessage = new TEventMessage();
                var associatedEvent = Command.ComposeEvent();
                associatedEventMessage.Event = associatedEvent;
                associatedEventMessage.CorrelationIdentifier = CorrelationIdentifier;
                EnrichEventMessage(associatedEventMessage);
                return associatedEventMessage;
            }
            catch (EventAuthoringException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new EventAuthoringException(typeof(TEvent), exception);
            }
        }

        /// <summary>
        /// Composes a new <see cref="IEventMessage" /> and publishes it using the specified mediator if <see cref="IsReported" />
        /// is <see langword="true" />.
        /// </summary>
        /// <param name="mediator">
        /// A mediator that is used to publish the event message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while attempting to publish the event message.
        /// </exception>
        /// <exception cref="EventAuthoringException">
        /// An exception was raised while composing the event.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="mediator" /> is disposed.
        /// </exception>
        public void ConditionallyReport(ICommandMediator mediator)
        {
            _ = mediator.RejectIf().IsNull(nameof(mediator));

            if (IsReported)
            {
                var associatedEventMessage = ComposeEventMessage();
                mediator.Process(associatedEventMessage);
            }
        }

        /// <summary>
        /// Hydrates and enriches the specified event message with details that are pertinent to the current
        /// <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" />.
        /// </summary>
        /// <remarks>
        /// When overridden by a derived class, this method is invoked by <see cref="ComposeEventMessage" /> to enrich event message
        /// objects with additional details.
        /// </remarks>
        /// <param name="associatedEventMessage">
        /// The event message to enrich.
        /// </param>
        protected virtual void EnrichEventMessage(TEventMessage associatedEventMessage)
        {
            return;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" /> should
        /// be reported as an <see cref="IEventMessage{TEvent}" /> by its processor.
        /// </summary>
        [DataMember]
        public Boolean IsReported
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default value indicating whether or not the current
        /// <see cref="CommandMessage{TCommand, TEvent, TEventMessage}" /> should be reported as an
        /// <see cref="IEventMessage{TEvent}" /> by its processor.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultIsReportedValue = true;
    }

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