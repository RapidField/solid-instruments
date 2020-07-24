// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command.DotNetNative.Extensions;

namespace RapidField.SolidInstruments.EventAuthoring.DotNetNative.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features to support event
    /// authoring.
    /// </summary>
    public static class IServiceCollectionExtensions
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
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(this IServiceCollection target)
            where TEvent : class, IEvent
            where TEventHandler : class, IEventHandler<TEvent> => target.AddCommandHandler<TEvent, TEventHandler>();
    }
}