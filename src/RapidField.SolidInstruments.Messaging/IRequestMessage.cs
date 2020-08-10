// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a message that requests a response of a specified type.
    /// </summary>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is associated with the request.
    /// </typeparam>
    public interface IRequestMessage<out TResponseMessage> : IMessage<TResponseMessage>, ICommand<TResponseMessage>, IRequestMessageBase
        where TResponseMessage : class, IResponseMessage
    {
    }
}