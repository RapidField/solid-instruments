// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Concurrency
{
    /// <summary>
    /// Specifies the consumption state of an <see cref="IConcurrencyControl" />.
    /// </summary>
    public enum ConcurrencyControlConsumptionState : Int32
    {
        /// <summary>
        /// The concurrency control consumption state is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// No threads are currently consuming the associated control.
        /// </summary>
        Unclaimed = 1,

        /// <summary>
        /// One or more threads are currently consuming the associated control, but additional threads may still enter.
        /// </summary>
        PartiallyClaimed = 2,

        /// <summary>
        /// One or more threads are currently consuming the associated control, and no additional threads may enter.
        /// </summary>
        FullyClaimed = 3
    }
}