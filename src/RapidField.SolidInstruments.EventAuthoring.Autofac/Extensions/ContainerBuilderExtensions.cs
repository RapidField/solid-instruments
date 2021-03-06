﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using RapidField.SolidInstruments.Command.Autofac.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.EventAuthoring.Autofac.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with inversion of control features to support event authoring.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers a transient event handler for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">
        /// The type of the event for which a handler is registered.
        /// </typeparam>
        /// <typeparam name="TEventHandler">
        /// The type of the event handler that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterEventHandler<TEvent, TEventHandler>(this ContainerBuilder target)
            where TEvent : class, IEvent
            where TEventHandler : class, IEventHandler<TEvent> => target.RegisterEventHandler(typeof(TEvent), typeof(TEventHandler));

        /// <summary>
        /// Registers a transient event handler for the specified event type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="eventType">
        /// The type of the event for which a handler is registered.
        /// </param>
        /// <param name="eventHandlerType">
        /// The type of the event handler that is registered.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventType" /> is <see langword="null" /> -or- <paramref name="eventHandlerType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="eventType" /> does not implement <see cref="IEvent" /> -or- <paramref name="eventHandlerType" /> does
        /// not implement <see cref="IEventHandler{TEvent}" />.
        /// </exception>
        public static void RegisterEventHandler(this ContainerBuilder target, Type eventType, Type eventHandlerType)
        {
            var eventHandlerInterfaceType = EventHandlerInterfaceType.MakeGenericType(eventType.RejectIf().IsNull(nameof(eventType)).OrIf().IsNotSupportedType(EventInterfaceType, nameof(eventType)));
            target.RegisterCommandHandler(eventType, eventHandlerType.RejectIf().IsNull(nameof(eventHandlerType)).OrIf().IsNotSupportedType(eventHandlerInterfaceType, nameof(eventHandlerType)));
        }

        /// <summary>
        /// Represents the <see cref="IEventHandler{TEvent}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventHandlerInterfaceType = typeof(IEventHandler<>);

        /// <summary>
        /// Represents the <see cref="IEvent" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventInterfaceType = typeof(IEvent);
    }
}