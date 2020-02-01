// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about a system state change event.
    /// </summary>
    public interface ISystemStateEvent : IMetadataEnrichedEvent
    {
        /// <summary>
        /// Gets or sets a name or value that uniquely identifies the associated system.
        /// </summary>
        String SystemIdentity
        {
            get;
            set;
        }
    }
}