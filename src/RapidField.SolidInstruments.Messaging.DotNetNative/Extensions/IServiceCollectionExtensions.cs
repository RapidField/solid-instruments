// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Command.DotNetNative.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.EventAuthoring.DotNetNative.Extensions;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Messaging.InMemory;
using RapidField.SolidInstruments.Messaging.Service;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using System;
using System.Diagnostics;

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
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddCommandMessageListener<TCommand, TMessage>(this IServiceCollection target)
            where TCommand : class, ICommand
            where TMessage : class, ICommandMessage<TCommand> => target.AddCommandMessageListener(typeof(TCommand), typeof(TMessage));

        /// <summary>
        /// Registers a transient command message listener for the specified message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="commandType">
        /// The type of the associated command.
        /// </param>
        /// <param name="messageType">
        /// The type of the command message for which a listener is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="messageType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="ICommand" /> -or- <paramref name="messageType" /> does not
        /// implement <see cref="ICommandMessage{TCommand}" />.
        /// </exception>
        public static IServiceCollection AddCommandMessageListener(this IServiceCollection target, Type commandType, Type messageType)
        {
            var commandMessageInerfaceType = CommandMessageInterfaceType.MakeGenericType(commandType.RejectIf().IsNull(nameof(commandType)).OrIf().IsNotSupportedType(CommandInterfaceType, nameof(commandType)));
            var commandMessageListenerType = CommandMessageListenerType.MakeGenericType(commandType, messageType.RejectIf().IsNull(nameof(messageType)).OrIf().IsNotSupportedType(commandMessageInerfaceType, nameof(messageType)));
            return target.AddMessageListener(messageType, commandMessageListenerType);
        }

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
            where TCommandHandler : class, ICommandHandler<TCommand> => target.AddCommandMessageListenerAndHandler(typeof(TCommand), typeof(TMessage), typeof(TCommandHandler));

        /// <summary>
        /// Registers a transient command message listener and an associated command handler for the specified message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="commandType">
        /// The type of the associated command.
        /// </param>
        /// <param name="messageType">
        /// The type of the command message for which a listener is registered.
        /// </param>
        /// <param name="commandHandlerType">
        /// The type of the command handler that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="messageType" /> is
        /// <see langword="null" />
        /// -or- <paramref name="commandHandlerType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="ICommand" /> -or- <paramref name="messageType" /> does not
        /// implement <see cref="ICommandMessage{TCommand}" /> -or- <paramref name="commandHandlerType" /> does not implement
        /// <see cref="ICommandHandler{TCommand}" />.
        /// </exception>
        public static IServiceCollection AddCommandMessageListenerAndHandler(this IServiceCollection target, Type commandType, Type messageType, Type commandHandlerType)
        {
            target.AddCommandMessageListener(commandType, messageType);
            target.AddCommandHandler(commandType, commandHandlerType);
            return target;
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
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddCommandMessageTransmitter<TCommand, TMessage>(this IServiceCollection target)
            where TCommand : class, ICommand
            where TMessage : class, ICommandMessage<TCommand> => target.AddCommandMessageTransmitter(typeof(TCommand), typeof(TMessage));

        /// <summary>
        /// Registers a transient command message transmitter for the specified message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="commandType">
        /// The type of the associated command.
        /// </param>
        /// <param name="messageType">
        /// The type of the command message for which a transmitter is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="messageType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="ICommand" /> -or- <paramref name="messageType" /> does not
        /// implement <see cref="ICommandMessage{TCommand}" />.
        /// </exception>
        public static IServiceCollection AddCommandMessageTransmitter(this IServiceCollection target, Type commandType, Type messageType)
        {
            var commandMessageInerfaceType = CommandMessageInterfaceType.MakeGenericType(commandType.RejectIf().IsNull(nameof(commandType)).OrIf().IsNotSupportedType(CommandInterfaceType, nameof(commandType)));
            var commandMessageTransmitterType = CommandMessageTransmitterType.MakeGenericType(commandType, messageType.RejectIf().IsNull(nameof(messageType)).OrIf().IsNotSupportedType(commandMessageInerfaceType, nameof(messageType)));
            return target.AddMessageTransmitter(messageType, commandMessageTransmitterType);
        }

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
            where TMessage : class, IEventMessage<TEvent> => target.AddEventMessageListener(typeof(TEvent), typeof(TMessage));

        /// <summary>
        /// Registers a transient event message listener for the specified message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="eventType">
        /// The type of the associated event.
        /// </param>
        /// <param name="messageType">
        /// The type of the event message for which a listener is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventType" /> is <see langword="null" /> -or- <paramref name="messageType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="eventType" /> does not implement <see cref="IEvent" /> -or- <paramref name="messageType" /> does not
        /// implement <see cref="IEventMessage{TEvent}" />.
        /// </exception>
        public static IServiceCollection AddEventMessageListener(this IServiceCollection target, Type eventType, Type messageType)
        {
            var eventMessageInerfaceType = EventMessageInterfaceType.MakeGenericType(eventType.RejectIf().IsNull(nameof(eventType)).OrIf().IsNotSupportedType(EventInterfaceType, nameof(eventType)));
            var eventMessageListenerType = EventMessageListenerType.MakeGenericType(eventType, messageType.RejectIf().IsNull(nameof(messageType)).OrIf().IsNotSupportedType(eventMessageInerfaceType, nameof(messageType)));
            return target.AddMessageListener(messageType, eventMessageListenerType);
        }

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
            where TEventHandler : class, IEventHandler<TEvent> => target.AddEventMessageListenerAndHandler(typeof(TEvent), typeof(TMessage), typeof(TEventHandler));

        /// <summary>
        /// Registers a transient event message listener and an associated event handler for the specified message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="eventType">
        /// The type of the associated event.
        /// </param>
        /// <param name="messageType">
        /// The type of the event message for which a listener is registered.
        /// </param>
        /// <param name="eventHandlerType">
        /// The type of the event handler that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventType" /> is <see langword="null" /> -or- <paramref name="messageType" /> is <see langword="null" />
        /// -or- <paramref name="eventHandlerType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="eventType" /> does not implement <see cref="IEvent" /> -or- <paramref name="messageType" /> does not
        /// implement <see cref="IEventMessage{TEvent}" /> -or- <paramref name="eventHandlerType" /> does not implement
        /// <see cref="IEventHandler{TEvent}" />.
        /// </exception>
        public static IServiceCollection AddEventMessageListenerAndHandler(this IServiceCollection target, Type eventType, Type messageType, Type eventHandlerType)
        {
            target.AddEventMessageListener(eventType, messageType);
            target.AddEventHandler(eventType, eventHandlerType);
            return target;
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
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddEventMessageTransmitter<TEvent, TMessage>(this IServiceCollection target)
            where TEvent : class, IEvent
            where TMessage : class, IEventMessage<TEvent> => target.AddEventMessageTransmitter(typeof(TEvent), typeof(TMessage));

        /// <summary>
        /// Registers a transient event message transmitter for the specified message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="eventType">
        /// The type of the associated event.
        /// </param>
        /// <param name="messageType">
        /// The type of the event message for which a transmitter is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventType" /> is <see langword="null" /> -or- <paramref name="messageType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="eventType" /> does not implement <see cref="IEvent" /> -or- <paramref name="messageType" /> does not
        /// implement <see cref="IEventMessage{TEvent}" />.
        /// </exception>
        public static IServiceCollection AddEventMessageTransmitter(this IServiceCollection target, Type eventType, Type messageType)
        {
            var eventMessageInerfaceType = EventMessageInterfaceType.MakeGenericType(eventType.RejectIf().IsNull(nameof(eventType)).OrIf().IsNotSupportedType(EventInterfaceType, nameof(eventType)));
            var eventMessageTransmitterType = EventMessageTransmitterType.MakeGenericType(eventType, messageType.RejectIf().IsNull(nameof(messageType)).OrIf().IsNotSupportedType(eventMessageInerfaceType, nameof(messageType)));
            return target.AddMessageTransmitter(messageType, eventMessageTransmitterType);
        }

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
            where TMessageListener : class, IMessageListener<HeartbeatMessage> => target.AddHeartbeatMessageListener(typeof(TMessageListener));

        /// <summary>
        /// Registers a transient message listener for the <see cref="HeartbeatMessage" /> type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="messageListenerType">
        /// The type of the message listener that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageListenerType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="messageListenerType" /> does not implement <see cref="IMessageListener{TMessage}" />.
        /// </exception>
        public static IServiceCollection AddHeartbeatMessageListener(this IServiceCollection target, Type messageListenerType)
        {
            var messageListenerInterfaceType = MessageListenerInterfaceType.MakeGenericType(HeartbeatMessageType);
            return target.AddMessageListener(HeartbeatMessageType, messageListenerType.RejectIf().IsNull(nameof(messageListenerType)).OrIf().IsNotSupportedType(messageListenerInterfaceType, nameof(messageListenerType)));
        }

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
            where TMessageListener : class, IMessageListener<TMessage> => target.AddMessageListener(typeof(TMessage), typeof(TMessageListener));

        /// <summary>
        /// Registers a transient message listener for the specified message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="messageType">
        /// The type of the message for which a listener is registered.
        /// </param>
        /// <param name="messageListenerType">
        /// The type of the message listener that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageType" /> is <see langword="null" /> -or- <paramref name="messageListenerType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="messageType" /> does not implement <see cref="IMessage" /> -or- <paramref name="messageListenerType" />
        /// does not implement <see cref="IMessageListener{TMessage}" />.
        /// </exception>
        public static IServiceCollection AddMessageListener(this IServiceCollection target, Type messageType, Type messageListenerType)
        {
            var messageListenerInterfaceType = MessageListenerInterfaceType.MakeGenericType(messageType.RejectIf().IsNull(nameof(messageType)).OrIf().IsNotSupportedType(MessageInterfaceType));
            target.TryAddTransient(messageListenerInterfaceType, messageListenerType.RejectIf().IsNull(nameof(messageListenerType)).OrIf().IsNotSupportedType(messageListenerInterfaceType, nameof(messageListenerType)));
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
            where TMessageTransmitter : class, IMessageTransmitter<TMessage> => target.AddMessageTransmitter(typeof(TMessage), typeof(TMessageTransmitter));

        /// <summary>
        /// Registers a transient message transmitter for the specified message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="messageType">
        /// The type of the message for which a transmitter is registered.
        /// </param>
        /// <param name="messageTransmitterType">
        /// The type of the message transmitter that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageType" /> is <see langword="null" /> -or- <paramref name="messageTransmitterType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="messageType" /> does not implement <see cref="IMessage" /> -or-
        /// <paramref name="messageTransmitterType" /> does not implement <see cref="IMessageTransmitter{TMessage}" />.
        /// </exception>
        public static IServiceCollection AddMessageTransmitter(this IServiceCollection target, Type messageType, Type messageTransmitterType)
        {
            var messageTransmitterInterfaceType = MessageTransmitterInterfaceType.MakeGenericType(messageType.RejectIf().IsNull(nameof(messageType)).OrIf().IsNotSupportedType(MessageInterfaceType));
            return target.AddCommandHandler(messageType, messageTransmitterType.RejectIf().IsNull(nameof(messageTransmitterType)).OrIf().IsNotSupportedType(messageTransmitterInterfaceType, nameof(messageTransmitterType)));
        }

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
            where TMessageListener : class, IMessageListener<TRequestMessage, TResponseMessage> => target.AddRequestMessageListener(typeof(TRequestMessage), typeof(TResponseMessage), typeof(TMessageListener));

        /// <summary>
        /// Registers a transient message listener for the specified request message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="requestMessageType">
        /// The type of the request message that is transmitted by the listener.
        /// </param>
        /// <param name="responseMessageType">
        /// The type of the response message that is transmitted in response to the request.
        /// </param>
        /// <param name="messageListenerType">
        /// The type of the message listener that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessageType" /> is <see langword="null" /> -or- <paramref name="responseMessageType" /> is
        /// <see langword="null" /> -or- <paramref name="messageListenerType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="requestMessageType" /> does not implement <see cref="IRequestMessage{TResponseMessage}" /> -or-
        /// <paramref name="responseMessageType" /> does not implement <see cref="IResponseMessage" /> -or-
        /// <paramref name="messageListenerType" /> does not implement
        /// <see cref="IMessageListener{TRequestMessage, TResponseMessage}" />.
        /// </exception>
        public static IServiceCollection AddRequestMessageListener(this IServiceCollection target, Type requestMessageType, Type responseMessageType, Type messageListenerType)
        {
            var requestMessageInterfaceType = RequestMessageInterfaceType.MakeGenericType(responseMessageType.RejectIf().IsNull(nameof(responseMessageType)).OrIf().IsNotSupportedType(ResponseMessageInterfaceType, nameof(responseMessageType)));
            var messageListenerWithResponseMessageInterfaceType = MessageListenerWithResponseMessageInterfaceType.MakeGenericType(requestMessageType.RejectIf().IsNull(nameof(requestMessageType)).OrIf().IsNotSupportedType(requestMessageInterfaceType, nameof(requestMessageType)), responseMessageType);
            target.TryAddTransient(messageListenerWithResponseMessageInterfaceType, messageListenerType.RejectIf().IsNull(nameof(messageListenerType)).OrIf().IsNotSupportedType(messageListenerWithResponseMessageInterfaceType, nameof(messageListenerType)));
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
            where TResponseMessage : class, IResponseMessage => target.AddRequestMessageTransmitter(typeof(TRequestMessage), typeof(TResponseMessage));

        /// <summary>
        /// Registers a transient message transmitter for the specified request message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="requestMessageType">
        /// The type of the request message that is transmitted by the transmitter.
        /// </param>
        /// <param name="responseMessageType">
        /// The type of the response message that is transmitted in response to the request.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessageType" /> is <see langword="null" /> -or- <paramref name="responseMessageType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="requestMessageType" /> does not implement <see cref="IRequestMessage{TResponseMessage}" /> -or-
        /// <paramref name="responseMessageType" /> does not implement <see cref="IResponseMessage" />.
        /// </exception>
        public static IServiceCollection AddRequestMessageTransmitter(this IServiceCollection target, Type requestMessageType, Type responseMessageType)
        {
            var requestMessageInterfaceType = RequestMessageInterfaceType.MakeGenericType(responseMessageType.RejectIf().IsNull(nameof(responseMessageType)).OrIf().IsNotSupportedType(ResponseMessageInterfaceType, nameof(responseMessageType)));
            var requestTransmitterType = RequestTransmitterType.MakeGenericType(requestMessageType.RejectIf().IsNull(nameof(requestMessageType)).OrIf().IsNotSupportedType(requestMessageInterfaceType, nameof(requestMessageType)), responseMessageType);
            return target.AddRequestMessageTransmitter(requestMessageType, responseMessageType, requestTransmitterType);
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
            where TMessageTransmitter : class, IMessageTransmitter<TRequestMessage, TResponseMessage> => target.AddRequestMessageTransmitter(typeof(TRequestMessage), typeof(TResponseMessage), typeof(TMessageTransmitter));

        /// <summary>
        /// Registers a transient message transmitter for the specified request message type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="requestMessageType">
        /// The type of the request message that is transmitted by the transmitter.
        /// </param>
        /// <param name="responseMessageType">
        /// The type of the response message that is transmitted in response to the request.
        /// </param>
        /// <param name="messageTransmitterType">
        /// The type of the message transmitter that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessageType" /> is <see langword="null" /> -or- <paramref name="responseMessageType" /> is
        /// <see langword="null" /> -or- <paramref name="messageTransmitterType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="requestMessageType" /> does not implement <see cref="IRequestMessage{TResponseMessage}" /> -or-
        /// <paramref name="responseMessageType" /> does not implement <see cref="IResponseMessage" /> -or-
        /// <paramref name="messageTransmitterType" /> does not implement
        /// <see cref="IMessageTransmitter{TRequestMessage, TResponseMessage}" />.
        /// </exception>
        public static IServiceCollection AddRequestMessageTransmitter(this IServiceCollection target, Type requestMessageType, Type responseMessageType, Type messageTransmitterType)
        {
            var requestMessageInterfaceType = RequestMessageInterfaceType.MakeGenericType(responseMessageType.RejectIf().IsNull(nameof(responseMessageType)).OrIf().IsNotSupportedType(ResponseMessageInterfaceType, nameof(responseMessageType)));
            var messageTransmitterWithResponseMessageInterfaceType = MessageTransmitterWithResponseMessageInterfaceType.MakeGenericType(requestMessageType.RejectIf().IsNull(nameof(requestMessageType)).OrIf().IsNotSupportedType(requestMessageInterfaceType, nameof(requestMessageType)), responseMessageType);
            return target.AddCommandHandler(requestMessageType, messageTransmitterType.RejectIf().IsNull(nameof(messageTransmitterType)).OrIf().IsNotSupportedType(messageTransmitterWithResponseMessageInterfaceType, nameof(messageTransmitterType)));
        }

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

        /// <summary>
        /// Represents the <see cref="ICommand" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandInterfaceType = typeof(ICommand);

        /// <summary>
        /// Represents the <see cref="ICommandMessage{TCommand}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandMessageInterfaceType = typeof(ICommandMessage<>);

        /// <summary>
        /// Represents the <see cref="CommandMessageListener{TCommand, TMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandMessageListenerType = typeof(CommandMessageListener<,>);

        /// <summary>
        /// Represents the <see cref="CommandMessageTransmitter{TCommand, TMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandMessageTransmitterType = typeof(CommandMessageTransmitter<,>);

        /// <summary>
        /// Represents the <see cref="IEvent" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventInterfaceType = typeof(IEvent);

        /// <summary>
        /// Represents the <see cref="IEventMessage{TEvent}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventMessageInterfaceType = typeof(IEventMessage<>);

        /// <summary>
        /// Represents the <see cref="EventMessageListener{TEvent, TMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventMessageListenerType = typeof(EventMessageListener<,>);

        /// <summary>
        /// Represents the <see cref="EventMessageTransmitter{TEvent, TMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventMessageTransmitterType = typeof(EventMessageTransmitter<,>);

        /// <summary>
        /// Represents the <see cref="HeartbeatMessage" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type HeartbeatMessageType = typeof(HeartbeatMessage);

        /// <summary>
        /// Represents the <see cref="IMessage" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageInterfaceType = typeof(IMessage);

        /// <summary>
        /// Represents the <see cref="IMessageListener{TMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageListenerInterfaceType = typeof(IMessageListener<>);

        /// <summary>
        /// Represents the <see cref="IMessageListener{TRequestMessage, TResponseMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageListenerWithResponseMessageInterfaceType = typeof(IMessageListener<,>);

        /// <summary>
        /// Represents the <see cref="IMessageTransmitter{TMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageTransmitterInterfaceType = typeof(IMessageTransmitter<>);

        /// <summary>
        /// Represents the <see cref="IMessageTransmitter{TRequestMessage, TResponseMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type MessageTransmitterWithResponseMessageInterfaceType = typeof(IMessageTransmitter<,>);

        /// <summary>
        /// Represents the <see cref="IRequestMessage{TResponseMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type RequestMessageInterfaceType = typeof(IRequestMessage<>);

        /// <summary>
        /// Represents the <see cref="RequestTransmitter{TRequestMessage, TResponseMessage}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type RequestTransmitterType = typeof(RequestTransmitter<,>);

        /// <summary>
        /// Represents the <see cref="IResponseMessage" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResponseMessageInterfaceType = typeof(IResponseMessage);
    }
}