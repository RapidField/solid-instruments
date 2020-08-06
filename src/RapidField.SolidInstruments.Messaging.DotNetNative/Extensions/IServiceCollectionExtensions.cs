// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Command.DotNetNative.Extensions;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.EventAuthoring.DotNetNative.Extensions;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Messaging.InMemory;
using RapidField.SolidInstruments.Messaging.Service;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;

namespace RapidField.SolidInstruments.Messaging.DotNetNative.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features to support messaging.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a transient command message listener for the specified message type.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the associated command.
        /// </typeparam>
        /// <typeparam name="TMessage">
        /// The type of the command message for which a listener is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        public static IServiceCollection AddCommandMessageListener<TCommand, TMessage>(this IServiceCollection target)
            where TCommand : class, ICommand
            where TMessage : class, ICommandMessage<TCommand> => target.AddMessageListener<TMessage, CommandMessageListener<TCommand, TMessage>>();

        /// <summary>
        /// Registers a transient command message listener and an associated command handler for the specified message type.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the associated command.
        /// </typeparam>
        /// <typeparam name="TMessage">
        /// The type of the command message for which a listener is registered.
        /// </typeparam>
        /// <typeparam name="TCommandHandler">
        /// The type of the command handler that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddCommandMessageListenerAndHandler<TCommand, TMessage, TCommandHandler>(this IServiceCollection target)
            where TCommand : class, ICommand
            where TMessage : class, ICommandMessage<TCommand>
            where TCommandHandler : class, ICommandHandler<TCommand> => target.AddCommandMessageListener<TCommand, TMessage>().AddCommandHandler<TCommand, TCommandHandler>();

        /// <summary>
        /// Registers a transient command message transmitter for the specified message type.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the associated command.
        /// </typeparam>
        /// <typeparam name="TMessage">
        /// The type of the command message for which a transmitter is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddCommandMessageTransmitter<TCommand, TMessage>(this IServiceCollection target)
            where TCommand : class, ICommand
            where TMessage : class, ICommandMessage<TCommand> => target.AddMessageTransmitter<TMessage, CommandMessageTransmitter<TCommand, TMessage>>();

        /// <summary>
        /// Registers a transient event message listener for the specified message type.
        /// </summary>
        /// <typeparam name="TEvent">
        /// The type of the associated event.
        /// </typeparam>
        /// <typeparam name="TMessage">
        /// The type of the event message for which a listener is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddEventMessageListener<TEvent, TMessage>(this IServiceCollection target)
            where TEvent : class, IEvent
            where TMessage : class, IEventMessage<TEvent> => target.AddMessageListener<TMessage, EventMessageListener<TEvent, TMessage>>();

        /// <summary>
        /// Registers a transient event message listener and an associated event handler for the specified message type.
        /// </summary>
        /// <typeparam name="TEvent">
        /// The type of the associated event.
        /// </typeparam>
        /// <typeparam name="TMessage">
        /// The type of the event message for which a listener is registered.
        /// </typeparam>
        /// <typeparam name="TEventHandler">
        /// The type of the event handler that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddEventMessageListenerAndHandler<TEvent, TMessage, TEventHandler>(this IServiceCollection target)
            where TEvent : class, IEvent
            where TMessage : class, IEventMessage<TEvent>
            where TEventHandler : class, IEventHandler<TEvent> => target.AddEventMessageListener<TEvent, TMessage>().AddEventHandler<TEvent, TEventHandler>();

        /// <summary>
        /// Registers a transient event message transmitter for the specified message type.
        /// </summary>
        /// <typeparam name="TEvent">
        /// The type of the associated event.
        /// </typeparam>
        /// <typeparam name="TMessage">
        /// The type of the event message for which a transmitter is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddEventMessageTransmitter<TEvent, TMessage>(this IServiceCollection target)
            where TEvent : class, IEvent
            where TMessage : class, IEventMessage<TEvent> => target.AddMessageTransmitter<TMessage, EventMessageTransmitter<TEvent, TMessage>>();

        /// <summary>
        /// Registers a transient message listener for the <see cref="HeartbeatMessage" /> type.
        /// </summary>
        /// <typeparam name="TMessageListener">
        /// The type of the message listener that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddHeartbeatMessageListener<TMessageListener>(this IServiceCollection target)
            where TMessageListener : class, IMessageListener<HeartbeatMessage> => target.AddMessageListener<HeartbeatMessage, TMessageListener>();

        /// <summary>
        /// Registers a transient message listener for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message for which a listener is registered.
        /// </typeparam>
        /// <typeparam name="TMessageListener">
        /// The type of the message listener that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddMessageListener<TMessage, TMessageListener>(this IServiceCollection target)
            where TMessage : class, IMessage
            where TMessageListener : class, IMessageListener<TMessage>
        {
            target.TryAddTransient<IMessageListener<TMessage>, TMessageListener>();
            return target;
        }

        /// <summary>
        /// Registers a transient message transmitter for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message for which a transmitter is registered.
        /// </typeparam>
        /// <typeparam name="TMessageTransmitter">
        /// The type of the message transmitter that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddMessageTransmitter<TMessage, TMessageTransmitter>(this IServiceCollection target)
            where TMessage : class, IMessage
            where TMessageTransmitter : class, IMessageTransmitter<TMessage> => target.AddCommandHandler<TMessage, TMessageTransmitter>();

        /// <summary>
        /// Registers a transient message listener for the <see cref="PingRequestMessage" /> type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddPingRequestMessageListener(this IServiceCollection target) => target.AddRequestMessageListener<PingRequestMessage, PingResponseMessage, PingRequestMessageListener>();

        /// <summary>
        /// Registers a transient message transmitter for the <see cref="PingRequestMessage" /> type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddPingRequestMessageTransmitter(this IServiceCollection target) => target.AddRequestMessageTransmitter<PingRequestMessage, PingResponseMessage>();

        /// <summary>
        /// Registers a transient message listener for the specified request message type.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message that is transmitted by the listener.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is transmitted in response to the request.
        /// </typeparam>
        /// <typeparam name="TMessageListener">
        /// The type of the message listener that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddRequestMessageListener<TRequestMessage, TResponseMessage, TMessageListener>(this IServiceCollection target)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
            where TMessageListener : class, IMessageListener<TRequestMessage, TResponseMessage>
        {
            target.TryAddTransient<IMessageListener<TRequestMessage, TResponseMessage>, TMessageListener>();
            return target;
        }

        /// <summary>
        /// Registers a transient message transmitter for the specified request message type.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message that is transmitted by the transmitter.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is transmitted in response to the request.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddRequestMessageTransmitter<TRequestMessage, TResponseMessage>(this IServiceCollection target)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage => target.AddRequestMessageTransmitter<TRequestMessage, TResponseMessage, RequestTransmitter<TRequestMessage, TResponseMessage>>();

        /// <summary>
        /// Registers a transient message transmitter for the specified request message type.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message that is transmitted by the transmitter.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is transmitted in response to the request.
        /// </typeparam>
        /// <typeparam name="TMessageTransmitter">
        /// The type of the message transmitter that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddRequestMessageTransmitter<TRequestMessage, TResponseMessage, TMessageTransmitter>(this IServiceCollection target)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
            where TMessageTransmitter : class, IMessageTransmitter<TRequestMessage, TResponseMessage> => target.AddCommandHandler<TRequestMessage, TMessageTransmitter>();

        /// <summary>
        /// Registers a collection of types that establish support for in-memory service bus functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddSupportingTypesForInMemoryMessaging(this IServiceCollection target)
        {
            target.TryAddSingleton(MessageTransport.Instance);
            target.TryAddSingleton(MessageTransport.Instance.CreateConnection());
            target.TryAddScoped<InMemoryMessageAdapter>();
            target.TryAddScoped<IMessageAdapter<PrimitiveMessage>>((serviceProvider) => serviceProvider.GetService<InMemoryMessageAdapter>());
            target.TryAddScoped<InMemoryClientFactory>();
            target.TryAddScoped<IMessagingClientFactory<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage>>((serviceProvider) => serviceProvider.GetService<InMemoryClientFactory>());
            target.TryAddScoped<InMemoryTransmittingFacade>();
            target.TryAddScoped<IMessageTransmittingFacade>((serviceProvider) => serviceProvider.GetService<InMemoryTransmittingFacade>());
            target.TryAddSingleton<InMemoryListeningFacade>();
            target.TryAddSingleton<IMessageListeningFacade>((serviceProvider) => serviceProvider.GetService<InMemoryListeningFacade>());
            target.TryAddSingleton<InMemoryRequestingFacade>();
            target.TryAddSingleton<IMessageRequestingFacade>((serviceProvider) => serviceProvider.GetService<InMemoryRequestingFacade>());
            return target;
        }
    }
}