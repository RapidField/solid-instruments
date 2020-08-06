// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Command.Autofac.Extensions;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.EventAuthoring.Autofac.Extensions;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Messaging.InMemory;
using RapidField.SolidInstruments.Messaging.Service;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;

namespace RapidField.SolidInstruments.Messaging.Autofac.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with inversion of control features to support messaging.
    /// </summary>
    public static class ContainerBuilderExtensions
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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterCommandMessageListener<TCommand, TMessage>(this ContainerBuilder target)
            where TCommand : class, ICommand
            where TMessage : class, ICommandMessage<TCommand> => target.RegisterMessageListener<TMessage, CommandMessageListener<TCommand, TMessage>>();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterCommandMessageListenerAndHandler<TCommand, TMessage, TCommandHandler>(this ContainerBuilder target)
            where TCommand : class, ICommand
            where TMessage : class, ICommandMessage<TCommand>
            where TCommandHandler : class, ICommandHandler<TCommand>
        {
            target.RegisterCommandMessageListener<TCommand, TMessage>();
            target.RegisterCommandHandler<TCommand, TCommandHandler>();
        }

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterCommandMessageTransmitter<TCommand, TMessage>(this ContainerBuilder target)
            where TCommand : class, ICommand
            where TMessage : class, ICommandMessage<TCommand> => target.RegisterMessageTransmitter<TMessage, CommandMessageTransmitter<TCommand, TMessage>>();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterEventMessageListener<TEvent, TMessage>(this ContainerBuilder target)
            where TEvent : class, IEvent
            where TMessage : class, IEventMessage<TEvent> => target.RegisterMessageListener<TMessage, EventMessageListener<TEvent, TMessage>>();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterEventMessageListenerAndHandler<TEvent, TMessage, TEventHandler>(this ContainerBuilder target)
            where TEvent : class, IEvent
            where TMessage : class, IEventMessage<TEvent>
            where TEventHandler : class, IEventHandler<TEvent>
        {
            target.RegisterEventMessageListener<TEvent, TMessage>();
            target.RegisterEventHandler<TEvent, TEventHandler>();
        }

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterEventMessageTransmitter<TEvent, TMessage>(this ContainerBuilder target)
            where TEvent : class, IEvent
            where TMessage : class, IEventMessage<TEvent> => target.RegisterMessageTransmitter<TMessage, EventMessageTransmitter<TEvent, TMessage>>();

        /// <summary>
        /// Registers a transient message listener for the <see cref="HeartbeatMessage" /> type.
        /// </summary>
        /// <typeparam name="TMessageListener">
        /// The type of the message listener that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterHeartbeatMessageListener<TMessageListener>(this ContainerBuilder target)
            where TMessageListener : class, IMessageListener<HeartbeatMessage> => target.RegisterMessageListener<HeartbeatMessage, TMessageListener>();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterMessageListener<TMessage, TMessageListener>(this ContainerBuilder target)
            where TMessage : class, IMessage
            where TMessageListener : class, IMessageListener<TMessage> => target.RegisterType<TMessageListener>().IfNotRegistered(typeof(TMessageListener)).As<IMessageListener<TMessage>>().InstancePerDependency();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterMessageTransmitter<TMessage, TMessageTransmitter>(this ContainerBuilder target)
            where TMessage : class, IMessage
            where TMessageTransmitter : class, IMessageTransmitter<TMessage> => target.RegisterCommandHandler<TMessage, TMessageTransmitter>();

        /// <summary>
        /// Registers a transient message listener for the <see cref="PingRequestMessage" /> type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterPingRequestMessageListener(this ContainerBuilder target) => target.RegisterRequestMessageListener<PingRequestMessage, PingResponseMessage, PingRequestMessageListener>();

        /// <summary>
        /// Registers a transient message transmitter for the <see cref="PingRequestMessage" /> type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterPingRequestMessageTransmitter(this ContainerBuilder target) => target.RegisterRequestMessageTransmitter<PingRequestMessage, PingResponseMessage>();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterRequestMessageListener<TRequestMessage, TResponseMessage, TMessageListener>(this ContainerBuilder target)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
            where TMessageListener : class, IMessageListener<TRequestMessage, TResponseMessage> => target.RegisterType<TMessageListener>().IfNotRegistered(typeof(TMessageListener)).As<IMessageListener<TRequestMessage, TResponseMessage>>().InstancePerDependency();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterRequestMessageTransmitter<TRequestMessage, TResponseMessage>(this ContainerBuilder target)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage => target.RegisterRequestMessageTransmitter<TRequestMessage, TResponseMessage, RequestTransmitter<TRequestMessage, TResponseMessage>>();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterRequestMessageTransmitter<TRequestMessage, TResponseMessage, TMessageTransmitter>(this ContainerBuilder target)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
            where TMessageTransmitter : class, IMessageTransmitter<TRequestMessage, TResponseMessage> => target.RegisterCommandHandler<TRequestMessage, TMessageTransmitter>();

        /// <summary>
        /// Registers a collection of types that establish support for in-memory service bus functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterSupportingTypesForInMemoryMessaging(this ContainerBuilder target)
        {
            target.RegisterInstance(MessageTransport.Instance).IfNotRegistered(typeof(IMessageTransport)).SingleInstance();
            target.RegisterInstance(MessageTransport.Instance.CreateConnection()).IfNotRegistered(typeof(IMessageTransportConnection)).SingleInstance();
            target.RegisterType<InMemoryMessageAdapter>().IfNotRegistered(typeof(InMemoryMessageAdapter)).As<IMessageAdapter<PrimitiveMessage>>().AsSelf().InstancePerLifetimeScope();
            target.RegisterType<InMemoryClientFactory>().IfNotRegistered(typeof(InMemoryClientFactory)).As<IMessagingClientFactory<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage>>().AsSelf().InstancePerLifetimeScope();
            target.RegisterType<InMemoryTransmittingFacade>().IfNotRegistered(typeof(InMemoryTransmittingFacade)).As<IMessageTransmittingFacade>().AsSelf().InstancePerLifetimeScope();
            target.RegisterType<InMemoryListeningFacade>().IfNotRegistered(typeof(InMemoryListeningFacade)).As<IMessageListeningFacade>().AsSelf().SingleInstance();
            target.RegisterType<InMemoryRequestingFacade>().IfNotRegistered(typeof(InMemoryRequestingFacade)).As<IMessageRequestingFacade>().AsSelf().SingleInstance();
        }
    }
}