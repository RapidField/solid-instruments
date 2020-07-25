// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    public interface ICommandBase
    {
        /// <summary>
        /// Gets or sets a unique identifier that is assigned to related commands.
        /// </summary>
        public Guid CorrelationIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the type of the result that is emitted when processing the command.
        /// </summary>
        public Type ResultType
        {
            get;
        }
    }
}