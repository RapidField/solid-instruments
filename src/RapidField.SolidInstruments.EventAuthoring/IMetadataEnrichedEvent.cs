// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an event that is enriched with metadata information.
    /// </summary>
    public interface IMetadataEnrichedEvent : IEvent, IMetadataEnrichedObject
    {
    }
}