// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.EventAuthoring.Extensions;
using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an extensible catalog of available events.
    /// </summary>
    /// <remarks>
    /// Consuming libraries may use <see cref="IEventRegister" /> as an extension method target to expose creational methods for
    /// custom events. See <see cref="IEventRegisterExtensions" /> for examples. An instance of an <see cref="IEventRegister" /> is
    /// made available by
    /// <see cref="ICommandMediatorExtensions.HandleEvent{TEvent}(ICommandMediator, Func{IEventRegister, TEvent})" /> and
    /// <see cref="ICommandMediatorExtensions.HandleEventAsync{TEvent}(ICommandMediator, Func{IEventRegister, TEvent})" />.
    /// </remarks>
    public interface IEventRegister
    {
    }
}