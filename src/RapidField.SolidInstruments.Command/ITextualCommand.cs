// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command that is described by textual information.
    /// </summary>
    public interface ITextualCommand : ILabeledCommand, IMetadataEnrichedCommand
    {
        /// <summary>
        /// Gets or sets the textual command value.
        /// </summary>
        public String Value
        {
            get;
            set;
        }
    }
}