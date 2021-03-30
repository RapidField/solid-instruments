// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Numerics;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    public readonly partial struct Number
    {
        /// <summary>
        /// Facilitates implicit <see cref="Byte" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(Byte target) => new(target);

        /// <summary>
        /// Facilitates implicit <see cref="SByte" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(SByte target) => new(target);

        /// <summary>
        /// Facilitates implicit <see cref="UInt16" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(UInt16 target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="Int16" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(Int16 target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="UInt32" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(UInt32 target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="Int32" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(Int32 target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="UInt64" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(UInt64 target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="Int64" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(Int64 target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="Single" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(Single target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="Double" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(Double target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="Decimal" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(Decimal target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="BigInteger" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(BigInteger target) => new Number(target).Compress();

        /// <summary>
        /// Facilitates implicit <see cref="BigRational" /> to <see cref="Number" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Number(BigRational target) => new Number(target).Compress();
    }
}