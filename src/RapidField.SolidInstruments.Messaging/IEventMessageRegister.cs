// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.Messaging.Extensions;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an extensible catalog of available event messages.
    /// </summary>
    /// <remarks>
    /// Consuming libraries may use <see cref="IEventMessageRegister" /> as an extension method target to expose creational methods
    /// for custom messages. See <see cref="IEventMessageRegisterExtensions" /> for examples. An instance of an
    /// <see cref="IEventMessageRegister" /> is made available by
    /// <see cref="ICommandMediatorExtensions.TransmitEventMessage{TEventMessage}(ICommandMediator, Func{IEventMessageRegister, TEventMessage})" />
    /// and
    /// <see cref="ICommandMediatorExtensions.TransmitEventMessageAsync{TEventMessage}(ICommandMediator, Func{IEventMessageRegister, TEventMessage})" />.
    /// </remarks>
    public interface IEventMessageRegister
    {
        /// <summary>
        /// Gets a catalog of available events.
        /// </summary>
        public IEventRegister Events
        {
            get;
        }
    }
}