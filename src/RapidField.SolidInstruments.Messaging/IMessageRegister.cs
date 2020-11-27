// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Messaging.Extensions;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an extensible catalog of available messages.
    /// </summary>
    /// <remarks>
    /// Consuming libraries may use <see cref="IMessageRegister" /> as an extension method target to expose creational methods for
    /// custom messages. See <see cref="IMessageRegisterExtensions" /> for examples. An instance of an
    /// <see cref="IMessageRegister" /> is made available by
    /// <see cref="ICommandMediatorExtensions.TransmitMessage{TMessage, TResult}(ICommandMediator, Func{IMessageRegister, TMessage})" />
    /// and
    /// <see cref="ICommandMediatorExtensions.TransmitMessageAsync{TMessage, TResult}(ICommandMediator, Func{IMessageRegister, TMessage})" />.
    /// </remarks>
    public interface IMessageRegister
    {
        /// <summary>
        /// Gets a catalog of available command messages.
        /// </summary>
        public ICommandMessageRegister CommandMessages
        {
            get;
        }

        /// <summary>
        /// Gets a catalog of available event messages.
        /// </summary>
        public IEventMessageRegister EventMessages
        {
            get;
        }

        /// <summary>
        /// Gets a catalog of available request messages.
        /// </summary>
        public IRequestMessageRegister RequestMessages
        {
            get;
        }
    }
}