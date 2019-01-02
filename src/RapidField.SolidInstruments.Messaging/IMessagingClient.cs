// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates messaging operations for a message bus.
    /// </summary>
    public interface IMessagingClient : IDisposable
    {
        /// <summary>
        /// Gets the format that is used to serialize and deserialize messages.
        /// </summary>
        SerializationFormat MessageSerializationFormat
        {
            get;
        }
    }
}