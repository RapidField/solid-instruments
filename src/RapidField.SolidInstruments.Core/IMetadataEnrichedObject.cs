// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that is enriched with metadata information.
    /// </summary>
    public interface IMetadataEnrichedObject
    {
        /// <summary>
        /// Gets a dictionary of metadata for the current <see cref="IMetadataEnrichedObject" />.
        /// </summary>
        public IDictionary<String, String> Metadata
        {
            get;
        }
    }
}