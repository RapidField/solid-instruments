// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    public readonly partial struct Number
    {
        /// <summary>
        /// Represents a <see cref="Number" /> with a value approximately equal to the mathematical constant e -- or Euler's number
        /// -- the base of the natural logarithm (~2.71828).
        /// </summary>
        public static readonly Number E = new(Math.E);

        /// <summary>
        /// Represents a <see cref="Number" /> with the value one (1).
        /// </summary>
        public static readonly Number One = new(1d);

        /// <summary>
        /// Represents a <see cref="Number" /> with a value approximately equal to the mathematical constant π -- or pi -- the ratio
        /// of a circle's circumference to its diameter (~3.14159).
        /// </summary>
        public static readonly Number Pi = new(Math.PI);

        /// <summary>
        /// Represents a <see cref="Number" /> with a value approximately equal to the mathematical constant τ -- or tau -- the
        /// number of radians in one turn (~6.28319).
        /// </summary>
        public static readonly Number Tau = new(Math.Tau);

        /// <summary>
        /// Represents a <see cref="Number" /> with the value zero (0).
        /// </summary>
        public static readonly Number Zero = new(0d);
    }
}