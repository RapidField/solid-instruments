// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an event.
    /// </summary>
    public interface IEvent : ICommand, IComparable<IEvent>, IEquatable<IEvent>
    {
        /// <summary>
        /// Converts the current <see cref="IEvent" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="IEvent" />.
        /// </returns>
        public Byte[] ToByteArray();

        /// <summary>
        /// Gets or sets the category of the event.
        /// </summary>
        public EventCategory Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a textual description of the event.
        /// </summary>
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a <see cref="DateTime" /> that indicates when the event occurred.
        /// </summary>
        public DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the verbosity level of the event.
        /// </summary>
        public EventVerbosity Verbosity
        {
            get;
            set;
        }
    }
}