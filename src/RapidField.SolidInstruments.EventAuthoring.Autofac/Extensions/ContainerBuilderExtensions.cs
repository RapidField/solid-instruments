// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using RapidField.SolidInstruments.Command.Autofac.Extensions;

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
            where TEventHandler : class, IEventHandler<TEvent> => target.RegisterCommandHandler<TEvent, TEventHandler>();
    }
}