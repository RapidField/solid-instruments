// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about a security-relevant event.
    /// </summary>
    public interface ISecurityEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the severity of the current <see cref="ISecurityEvent" />.
        /// </summary>
        SecurityEventSeverity Severity
        {
            get;
            set;
        }
    }
}