// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    public readonly partial struct Number : IComparable<Number>, IEquatable<Number>
    {
        /// <summary>
        /// Represents the numeric type represented by the bytes constituting <see cref="Value" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly NumericDataFormat Format;

        /// <summary>
        /// Represents the bytes constituting the value of the current <see cref="Number" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Memory<Byte> Value;
    }
}