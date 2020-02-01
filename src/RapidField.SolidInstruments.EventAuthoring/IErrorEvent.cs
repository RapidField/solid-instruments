// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an error event.
    /// </summary>
    public interface IErrorEvent : IEvent
    {
        /// <summary>
        /// Gets or sets a name or value that uniquely identifies the application in which the associated error occurred.
        /// </summary>
        String ApplicationIdentity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets textual diagnostic information about the associated error.
        /// </summary>
        String DiagnosticDetails
        {
            get;
            set;
        }
    }
}