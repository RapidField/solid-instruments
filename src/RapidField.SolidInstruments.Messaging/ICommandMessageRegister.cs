// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Messaging.Extensions;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an extensible catalog of available command messages.
    /// </summary>
    /// <remarks>
    /// Consuming libraries may use <see cref="ICommandMessageRegister" /> as an extension method target to expose creational
    /// methods for custom messages. See <see cref="ICommandMessageRegisterExtensions" /> for examples. An instance of an
    /// <see cref="ICommandMessageRegister" /> is made available by
    /// <see cref="ICommandMediatorExtensions.TransmitCommandMessage{TCommandMessage}(ICommandMediator, Func{ICommandMessageRegister, TCommandMessage})" />
    /// and
    /// <see cref="ICommandMediatorExtensions.TransmitCommandMessageAsync{TCommandMessage}(ICommandMediator, Func{ICommandMessageRegister, TCommandMessage})" />.
    /// </remarks>
    public interface ICommandMessageRegister
    {
        /// <summary>
        /// Gets a catalog of available commands.
        /// </summary>
        public ICommandRegister Commands
        {
            get;
        }
    }
}