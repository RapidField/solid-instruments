﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring.Extensions
{
    /// <summary>
    /// Extends the <see cref="EventVerbosity" /> enumeration with event authoring features.
    /// </summary>
    public static class EventVerbosityExtensions
    {
        /// <summary>
        /// Converts the current <see cref="EventVerbosity" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="EventVerbosity" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="EventVerbosity" />.
        /// </returns>
        public static Byte[] ToByteArray(this EventVerbosity target) => BitConverter.GetBytes((Int32)target);
    }
}