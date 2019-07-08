// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.ObjectComposition;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Encapsulates creation of new <see cref="IDurableMessageQueue" /> instances.
    /// </summary>
    public interface IDurableMessageQueueFactory : IObjectFactory<IDurableMessageQueue>
    {
    }
}