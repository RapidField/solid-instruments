// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Diagnostics;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an extensible catalog of available events.
    /// </summary>
    /// <remarks>
    /// <see cref="EventRegister" /> is the default implementation of <see cref="IEventRegister" />.
    /// </remarks>
    public sealed class EventRegister : IEventRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventRegister" /> class.
        /// </summary>
        [DebuggerHidden]
        private EventRegister()
        {
            return;
        }

        /// <summary>
        /// Represents a singleton instance of the <see cref="EventRegister" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly IEventRegister Instance = new EventRegister();
    }
}