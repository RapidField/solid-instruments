// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an event that is labeled with categorical and/or contextual information.
    /// </summary>
    public interface ILabeledEvent : IEvent
    {
        /// <summary>
        /// Gets a collection of textual labels that provide categorical and/or contextual information about the current
        /// <see cref="ILabeledEvent" />.
        /// </summary>
        ICollection<String> Labels
        {
            get;
        }
    }
}