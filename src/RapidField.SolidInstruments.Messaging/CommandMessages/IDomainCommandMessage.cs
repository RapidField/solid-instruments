// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command related to domain logic.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    public interface IDomainCommandMessage<TCommand> : ICommandMessage<TCommand>
        where TCommand : class, IDomainCommand
    {
    }
}