﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Domain;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command related to an object that models a domain construct and which can be described
    /// by an <see cref="IEvent" />.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelCommandMessage{TModel, TCommand, TEvent, TEventMessage}" /> is a derivative of
    /// <see cref="DomainCommandMessage{TCommand, TEvent, TEventMessage}" /> which is extended to support
    /// <see cref="IReportableCommandMessage{TCommand, TEvent, TEventMessage}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
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
    public abstract class DomainModelCommandMessage<TModel, TCommand, TEvent, TEventMessage> : DomainCommandMessage<TCommand, TEvent, TEventMessage>, IDomainModelCommandMessage<TModel, TCommand>
        where TModel : class, IDomainModel
        where TEvent : DomainModelEvent<TModel>, new()
        where TCommand : DomainModelCommand<TModel>, IDomainModelReportableCommand<TModel, TEvent>, new()
        where TEventMessage : DomainModelEventMessage<TModel, TEvent>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommandMessage{TModel, TCommand, TEvent, TEventMessage}" />
        /// class.
        /// </summary>
        protected DomainModelCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommandMessage{TModel, TCommand, TEvent, TEventMessage}" />
        /// class.
        /// </summary>
        /// <param name="isReported">
        /// A value indicating whether or not the command should be reported with an event message by its processor. The default
        /// value is <see langword="true" />.
        /// </param>
        protected DomainModelCommandMessage(Boolean isReported)
            : base(isReported)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommandMessage{TModel, TCommand, TEvent, TEventMessage}" />
        /// class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelCommandMessage(TCommand commandObject)
            : base(commandObject)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommandMessage{TModel, TCommand, TEvent, TEventMessage}" />
        /// class.
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
        protected DomainModelCommandMessage(TCommand commandObject, Boolean isReported)
            : base(commandObject, isReported)
        {
            return;
        }

        /// <summary>
        /// Hydrates and enriches the specified event message with details that are pertinent to the current
        /// <see cref="DomainModelCommandMessage{TModel, TCommand, TEvent, TEventMessage}" />.
        /// </summary>
        /// <param name="associatedEventMessage">
        /// The event message to enrich.
        /// </param>
        protected override void EnrichEventMessage(TEventMessage associatedEventMessage)
        {
            try
            {
                associatedEventMessage.Event.Model = Model;
            }
            finally
            {
                base.EnrichEventMessage(associatedEventMessage);
            }
        }

        /// <summary>
        /// Gets the desired state of the associated domain model.
        /// </summary>
        [IgnoreDataMember]
        public TModel Model => Command.Model;
    }

    /// <summary>
    /// Represents a message that contains a command related to an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelCommandMessage{TModel, TCommand}" /> is the default implementation of
    /// <see cref="IDomainModelCommandMessage{TModel, TCommand}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    [DataContract]
    public abstract class DomainModelCommandMessage<TModel, TCommand> : DomainCommandMessage<TCommand>, IDomainModelCommandMessage<TModel, TCommand>
        where TModel : class, IDomainModel
        where TCommand : DomainModelCommand<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommandMessage{TModel, TCommand}" /> class.
        /// </summary>
        protected DomainModelCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommandMessage{TModel, TCommand}" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelCommandMessage(TCommand commandObject)
            : base(commandObject)
        {
            return;
        }

        /// <summary>
        /// Gets the desired state of the associated domain model.
        /// </summary>
        [IgnoreDataMember]
        public TModel Model => Command.Model;
    }
}