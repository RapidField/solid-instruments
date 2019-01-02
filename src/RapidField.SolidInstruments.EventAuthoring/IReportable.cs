// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an object that can be converted to a reportable application event.
    /// </summary>
    public interface IReportable
    {
        /// <summary>
        /// Compose a reportable application event for the current object.
        /// </summary>
        /// <returns>
        /// A reportable <see cref="ApplicationEvent" /> representing the current object.
        /// </returns>
        ApplicationEvent ComposeReportableEvent();
    }
}