// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    public interface IMessage : IMessage<Nix>, ICommand
    {
    }

    /// <summary>
    /// Represents a message that emits a result when processed.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the result that is produced by handling the message.
    /// </typeparam>
    public interface IMessage<out TResult> : IMessageBase, ICommand<TResult>
    {
    }
}