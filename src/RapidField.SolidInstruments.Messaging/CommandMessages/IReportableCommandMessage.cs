// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command that can be described by an <see cref="IEvent" />.
    /// </summary>
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
    public interface IReportableCommandMessage<TCommand, TEvent, TEventMessage> : ICommandMessage<TCommand>, IReportableCommandMessage
        where TEvent : class, IEvent
        where TCommand : class, IReportableCommand<TEvent>
        where TEventMessage : class, IEventMessage<TEvent>
    {
        /// <summary>
        /// Composes an <see cref="IEventMessage{TEvent}" /> representing information about the current
        /// <see cref="IReportableCommandMessage{TCommand, TEvent, TEventMessage}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="IEventMessage{TEvent}" /> representing information about the current object.
        /// </returns>
        /// <exception cref="EventAuthoringException">
        /// An exception was raised while composing the event.
        /// </exception>
        public TEventMessage ComposeEventMessage();
    }

    /// <summary>
    /// Represents a message that contains a command that can be described by an <see cref="IEvent" />.
    /// </summary>
    public interface IReportableCommandMessage : ICommandMessage
    {
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
        public void ConditionallyReport(ICommandMediator mediator);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IReportableCommandMessage" /> should be reported as an
        /// <see cref="IEventMessage" /> by its processor.
        /// </summary>
        public Boolean IsReported
        {
            get;
        }
    }
}