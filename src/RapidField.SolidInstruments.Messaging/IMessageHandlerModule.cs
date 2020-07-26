// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Encapsulates container configuration for message handlers.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IMessageHandlerModule<TConfigurator> : IMessageHandlerModule, ICommandHandlerModule<TConfigurator>
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for message handlers.
    /// </summary>
    public interface IMessageHandlerModule : ICommandHandlerModule
    {
    }
}