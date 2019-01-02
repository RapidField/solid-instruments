// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Specifies the behavior of an <see cref="IChannel{T}" /> when an out-of-range read request is made.
    /// </summary>
    public enum InvalidReadBehavior : Int32
    {
        /// <summary>
        /// The behavior of the channel is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The channel returns a silent signal when an out-of-range read request is made.
        /// </summary>
        ReadSilence = 1,

        /// <summary>
        /// The channel raises an exception when an out-of-range read request is made.
        /// </summary>
        RaiseException = 2
    }
}