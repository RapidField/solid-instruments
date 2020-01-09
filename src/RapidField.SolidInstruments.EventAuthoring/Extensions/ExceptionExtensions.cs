// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="Exception" /> class with event authoring features.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Compose an <see cref="IEvent" /> representing information about the current <see cref="Exception" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Exception" />.
        /// </param>
        /// <returns>
        /// An <see cref="IEvent" /> representing information about the current object.
        /// </returns>
        public static IEvent ComposeEvent(this Exception target) => new ErrorEvent(target);
    }
}