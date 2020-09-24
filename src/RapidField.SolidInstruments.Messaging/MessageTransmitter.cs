// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Transmits messages.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageTransmitter{TMessage}" /> is the default implementation of <see cref="IMessageTransmitter{TMessage}" />.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the message that is transmitted by the transmitter.
    /// </typeparam>
    public abstract class MessageTransmitter<TMessage> : MessageHandler<TMessage>, IMessageTransmitter<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransmitter{TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="facade">
        /// An appliance that facilitates implementation-specific message transmission operations.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="facade" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        protected MessageTransmitter(ICommandMediator mediator, IMessageTransmittingFacade facade, MessagingEntityType entityType)
            : base(mediator, MessageHandlerRole.Transmitter, entityType)
        {
            Facade = facade.RejectIf().IsNull(nameof(facade)).TargetArgument;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageTransmitter{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands. Do not process <paramref name="command" /> using
        /// <paramref name="mediator" />, as doing so will generally result in infinite-looping.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Process(TMessage command, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            switch (EntityType)
            {
                case MessagingEntityType.Queue:

                    controlToken.AttachTask(Facade.TransmitToQueueAsync(command));
                    break;

                case MessagingEntityType.Topic:

                    controlToken.AttachTask(Facade.TransmitToTopicAsync(command));
                    break;

                default:

                    throw new UnsupportedSpecificationException($"The specified messaging entity type, {EntityType}, is not supported.");
            }
        }

        /// <summary>
        /// Gets the type of the message that the current <see cref="MessageTransmitter{TMessage}" /> transmits.
        /// </summary>
        public Type MessageType => typeof(TMessage);

        /// <summary>
        /// Represents an appliance that facilitates implementation-specific message transmission operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IMessageTransmittingFacade Facade;
    }

    /// <summary>
    /// Transmits request messages.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageTransmitter{TRequestMessage, TResponseMessage}" /> is the default implementation of
    /// <see cref="IMessageTransmitter{TRequestMessage, TResponseMessage}" />.
    /// </remarks>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is transmitted by the transmitter.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is transmitted in response to the request.
    /// </typeparam>
    public abstract class MessageTransmitter<TRequestMessage, TResponseMessage> : MessageHandler<TRequestMessage, TResponseMessage>, IMessageTransmitter<TRequestMessage, TResponseMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransmitter{TRequestMessage, TResponseMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="facade">
        /// An appliance that facilitates implementation-specific request message transmission operations.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="facade" /> is <see langword="null" />.
        /// </exception>
        protected MessageTransmitter(ICommandMediator mediator, IMessageRequestingFacade facade)
            : base(mediator, MessageHandlerRole.Transmitter, Message.RequestEntityType)
        {
            Facade = facade.RejectIf().IsNull(nameof(facade)).TargetArgument;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageTransmitter{TRequestMessage, TResponseMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands. Do not process <paramref name="command" /> using
        /// <paramref name="mediator" />, as doing so will generally result in infinite-looping.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected override TResponseMessage Process(TRequestMessage command, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            using (var requestTask = Facade.RequestAsync<TRequestMessage, TResponseMessage>(command))
            {
                requestTask.Wait();
                return requestTask.Result;
            }
        }

        /// <summary>
        /// Gets the type of the message that the current <see cref="MessageTransmitter{TRequestMessage, TResponseMessage}" />
        /// transmits.
        /// </summary>
        public Type MessageType => typeof(TRequestMessage);

        /// <summary>
        /// Represents an appliance that facilitates implementation-specific message transmission operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IMessageRequestingFacade Facade;
    }
}