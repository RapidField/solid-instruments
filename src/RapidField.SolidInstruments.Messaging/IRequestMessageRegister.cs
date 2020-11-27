// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Messaging.Extensions;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an extensible catalog of available request messages.
    /// </summary>
    /// <remarks>
    /// Consuming libraries may use <see cref="IRequestMessageRegister" /> as an extension method target to expose creational
    /// methods for custom messages. See <see cref="IRequestMessageRegisterExtensions" /> for examples. An instance of an
    /// <see cref="IRequestMessageRegister" /> is made available by
    /// <see cref="ICommandMediatorExtensions.TransmitRequestMessage{TRequestMessage, TResponseMessage}(ICommandMediator, Func{IRequestMessageRegister, TRequestMessage})" />
    /// and
    /// <see cref="ICommandMediatorExtensions.TransmitRequestMessageAsync{TRequestMessage, TResponseMessage}(ICommandMediator, Func{IRequestMessageRegister, TRequestMessage})" />.
    /// </remarks>
    public interface IRequestMessageRegister
    {
    }
}