// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an extensible catalog of available event messages.
    /// </summary>
    /// <remarks>
    /// <see cref="EventMessageRegister" /> is the default implementation of <see cref="IEventMessageRegister" />.
    /// </remarks>
    public sealed class EventMessageRegister : IEventMessageRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessageRegister" /> class.
        /// </summary>
        [DebuggerHidden]
        private EventMessageRegister()
        {
            Events = EventRegister.Instance;
        }

        /// <summary>
        /// Gets a catalog of available events.
        /// </summary>
        public IEventRegister Events
        {
            get;
        }

        /// <summary>
        /// Represents a singleton instance of the <see cref="EventMessageRegister" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly IEventMessageRegister Instance = new EventMessageRegister();
    }
}