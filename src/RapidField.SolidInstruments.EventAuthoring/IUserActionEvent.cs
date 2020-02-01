// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about a user action event.
    /// </summary>
    public interface IUserActionEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the outcome of the current <see cref="IUserActionEvent" />.
        /// </summary>
        UserActionEventOutcome Outcome
        {
            get;
            set;
        }
    }
}