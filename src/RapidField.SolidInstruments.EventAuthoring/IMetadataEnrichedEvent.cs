// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an event that is enriched with metadata information.
    /// </summary>
    public interface IMetadataEnrichedEvent : IEvent
    {
        /// <summary>
        /// Gets a dictionary of metadata for the current <see cref="IMetadataEnrichedEvent" />.
        /// </summary>
        IDictionary<String, String> Metadata
        {
            get;
        }
    }
}