// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an event.
    /// </summary>
    public interface IEvent : IComparable<IEvent>, IEquatable<IEvent>
    {
        /// <summary>
        /// Converts the current <see cref="IEvent" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="IEvent" />.
        /// </returns>
        Byte[] ToByteArray();

        /// <summary>
        /// Gets or sets the category of the event.
        /// </summary>
        EventCategory Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a textual description of the event.
        /// </summary>
        String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a <see cref="DateTime" /> that indicates when the event occurred.
        /// </summary>
        DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the verbosity level of the event.
        /// </summary>
        EventVerbosity Verbosity
        {
            get;
            set;
        }
    }
}