// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Encapsulates container configuration for message listeners.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IMessageListenerModule<TConfigurator> : IMessageListenerModule, IMessageHandlerModule<TConfigurator>
        where TConfigurator : class, new()
    {
    }

    /// <summary>
    /// Encapsulates container configuration for message listeners.
    /// </summary>
    public interface IMessageListenerModule : IMessageHandlerModule
    {
    }
}