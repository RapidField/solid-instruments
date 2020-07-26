// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="UserActionEventOutcome" /> enumeration with event authoring features.
    /// </summary>
    public static class UserActionEventOutcomeExtensions
    {
        /// <summary>
        /// Converts the current <see cref="UserActionEventOutcome" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="UserActionEventOutcome" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="UserActionEventOutcome" />.
        /// </returns>
        public static Byte[] ToByteArray(this UserActionEventOutcome target) => BitConverter.GetBytes((Int32)target);
    }
}