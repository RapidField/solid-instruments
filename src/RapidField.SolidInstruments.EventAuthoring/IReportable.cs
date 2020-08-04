// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents an object that can be described by an <see cref="IEvent" />.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the event that describes the object.
    /// </typeparam>
    public interface IReportable<TEvent> : IReportable
        where TEvent : class, IEvent
    {
        /// <summary>
        /// Composes an <see cref="IEvent" /> representing information about the current <see cref="IReportable{TEvent}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="IEvent" /> representing information about the current object.
        /// </returns>
        /// <exception cref="EventAuthoringException">
        /// An exception was raised while composing the event.
        /// </exception>
        public new TEvent ComposeEvent();
    }

    /// <summary>
    /// Represents an object that can be described by an <see cref="IEvent" />.
    /// </summary>
    public interface IReportable
    {
        /// <summary>
        /// Composes an <see cref="IEvent" /> representing information about the current <see cref="IReportable" />.
        /// </summary>
        /// <returns>
        /// An <see cref="IEvent" /> representing information about the current object.
        /// </returns>
        /// <exception cref="EventAuthoringException">
        /// An exception was raised while composing the event.
        /// </exception>
        public IEvent ComposeEvent();
    }
}