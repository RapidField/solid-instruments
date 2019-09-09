// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Supports message exchange for a collection of queues and topics.
    /// </summary>
    public interface IDurableMessageTransport : IDisposable
    {
        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        Task CreateQueueAsync(String path);
    }
}