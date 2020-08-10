// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Messaging.EventMessages;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    public interface ICommandMessage<TCommand> : ICommandMessage
        where TCommand : class, ICommand
    {
        /// <summary>
        /// Gets or sets the associated command.
        /// </summary>
        public TCommand Command
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a message that contains a command.
    /// </summary>
    public interface ICommandMessage : IMessage, ICommandMessageBase
    {
    }
}