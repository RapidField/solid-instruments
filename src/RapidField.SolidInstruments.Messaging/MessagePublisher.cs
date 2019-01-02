// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Publishes messages.
    /// </summary>
    /// <remarks>
    /// <see cref="MessagePublisher{TMessage}" /> is the default implementation of <see cref="IMessagePublisher{TMessage}" />.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the message that is published by the publisher.
    /// </typeparam>
    public abstract class MessagePublisher<TMessage> : MessageHandler<TMessage>, IMessagePublisher<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublisher{TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="client">
        /// A client that facilitates message publishing operations.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="client" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        protected MessagePublisher(ICommandMediator mediator, IMessagePublishingClient client, MessagingEntityType entityType)
            : base(mediator, MessageHandlerRole.Publisher, entityType)
        {
            Client = client.RejectIf().IsNull(nameof(client)).TargetArgument;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessagePublisher{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
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
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected override void Process(TMessage command, ICommandMediator mediator, ConcurrencyControlToken controlToken) => controlToken.AttachTask(Client.PublishAsync(command, EntityType));

        /// <summary>
        /// Gets the type of the message that the current <see cref="MessagePublisher{TMessage}" /> publishes.
        /// </summary>
        public Type MessageType => typeof(TMessage);

        /// <summary>
        /// Represents a client that facilitates message publishing operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IMessagePublishingClient Client;
    }

    /// <summary>
    /// Publishes messages.
    /// </summary>
    /// <remarks>
    /// <see cref="MessagePublisher{TRequestMessage, TResponseMessage}" /> is the default implementation of
    /// <see cref="IMessagePublisher{TRequestMessage, TResponseMessage}" />.
    /// </remarks>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is published by the publisher.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is published in response to the request.
    /// </typeparam>
    public abstract class MessagePublisher<TRequestMessage, TResponseMessage> : MessageHandler<TRequestMessage, TResponseMessage>, IMessagePublisher<TRequestMessage, TResponseMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublisher{TRequestMessage, TResponseMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="client">
        /// A client that facilitates message publishing operations.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="client" /> is <see langword="null" />.
        /// </exception>
        protected MessagePublisher(ICommandMediator mediator, IMessagePublishingClient client)
            : base(mediator, MessageHandlerRole.Publisher, MessagePublishingClient.RequestMessageEntityType)
        {
            Client = client.RejectIf().IsNull(nameof(client)).TargetArgument;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessagePublisher{TRequestMessage, TResponseMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
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
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected override TResponseMessage Process(TRequestMessage command, ICommandMediator mediator, ConcurrencyControlToken controlToken)
        {
            using (var publishTask = Client.RequestAsync<TRequestMessage, TResponseMessage>(command))
            {
                publishTask.Wait();
                return publishTask.Result;
            }
        }

        /// <summary>
        /// Gets the type of the message that the current <see cref="MessagePublisher{TRequestMessage, TResponseMessage}" />
        /// publishes.
        /// </summary>
        public Type MessageType => typeof(TRequestMessage);

        /// <summary>
        /// Represents a client that facilitates message publishing operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IMessagePublishingClient Client;
    }
}