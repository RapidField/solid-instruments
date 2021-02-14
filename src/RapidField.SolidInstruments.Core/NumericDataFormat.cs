// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Specifies the numeric type represented by the bytes constituting a <see cref="Number" />.
    /// </summary>
    internal enum NumericDataFormat : Byte
    {
        /// <summary>
        /// The numeric data format is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.Byte" />.
        /// </summary>
        Byte = 1,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.SByte" />.
        /// </summary>
        SByte = 2,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.UInt16" />.
        /// </summary>
        UInt16 = 3,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent an <see cref="System.Int16" />.
        /// </summary>
        Int16 = 4,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.UInt32" />.
        /// </summary>
        UInt32 = 5,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent an <see cref="System.Int32" />.
        /// </summary>
        Int32 = 6,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.UInt64" />.
        /// </summary>
        UInt64 = 7,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent an <see cref="System.Int64" />.
        /// </summary>
        Int64 = 8,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.Single" />.
        /// </summary>
        Single = 9,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.Double" />.
        /// </summary>
        Double = 10,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.Decimal" />.
        /// </summary>
        Decimal = 11,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="System.Numerics.BigInteger" />.
        /// </summary>
        BigInteger = 12,

        /// <summary>
        /// The bytes contained within a <see cref="Number" /> represent a <see cref="Rationals.Rational" />.
        /// </summary>
        BigRational = 13
    }
}