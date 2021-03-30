// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Numerics;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    public readonly partial struct Number : IComparable<Number>, IEquatable<Number>
    {
        /// <summary>
        /// Returns the absolute value of the current <see cref="Number" />.
        /// </summary>
        /// <returns>
        /// The absolute value of the current <see cref="Number" />.
        /// </returns>
        public readonly Number AbsoluteValue() => Format switch
        {
            NumericDataFormat.Byte => this,
            NumericDataFormat.SByte => Math.Abs(ToInt16()),
            NumericDataFormat.UInt16 => this,
            NumericDataFormat.Int16 => Math.Abs(ToInt32()),
            NumericDataFormat.UInt32 => this,
            NumericDataFormat.Int32 => Math.Abs(ToInt64()),
            NumericDataFormat.UInt64 => this,
            NumericDataFormat.Int64 => BigInteger.Abs(ToBigInteger()),
            NumericDataFormat.Single => Math.Abs(ToSingle()),
            NumericDataFormat.Double => Math.Abs(ToDouble()),
            NumericDataFormat.Decimal => Math.Abs(ToDecimal()),
            NumericDataFormat.BigInteger => BigInteger.Abs(ToBigInteger()),
            NumericDataFormat.BigRational => BigRational.Abs(ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Calculates the base ten (10) logarithm of the current <see cref="Number" />.
        /// </summary>
        /// <returns>
        /// The natural base ten (10) logarithm of the current <see cref="Number" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The result cannot be represented by the <see cref="Number" /> type.
        /// </exception>
        public readonly Number CalculateBaseTenLogarithm()
        {
            try
            {
                return IsInteger ? BigInteger.Log10(ToBigInteger()) : BigRational.Log10(ToBigRational());
            }
            catch (Exception exception)
            {
                throw new OverflowException("The resulting base ten logarithm cannot be represented by the Number type.", exception);
            }
        }

        /// <summary>
        /// Calculates the cube root of the current <see cref="Number" />.
        /// </summary>
        /// <returns>
        /// The cube root of the current <see cref="Number" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The result cannot be represented by the <see cref="Number" /> type.
        /// </exception>
        public readonly Number CalculateCubeRoot() => CalculateNthRoot(3);

        /// <summary>
        /// Calculates the logarithm of the current <see cref="Number" />.
        /// </summary>
        /// <param name="baseValue">
        /// The base of the logarithm.
        /// </param>
        /// <returns>
        /// The logarithm of the current <see cref="Number" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The result cannot be represented by the <see cref="Number" /> type.
        /// </exception>
        public readonly Number CalculateLogarithm(Double baseValue)
        {
            try
            {
                return IsInteger ? BigInteger.Log(ToBigInteger(), baseValue) : BigRational.Log(ToBigRational(), baseValue);
            }
            catch (Exception exception)
            {
                throw new OverflowException($"The resulting base {baseValue} logarithm cannot be represented by the Number type.", exception);
            }
        }

        /// <summary>
        /// Calculates the natural (base e) logarithm of the current <see cref="Number" />.
        /// </summary>
        /// <returns>
        /// The natural (base e) logarithm of the current <see cref="Number" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The result cannot be represented by the <see cref="Number" /> type.
        /// </exception>
        public readonly Number CalculateNaturalLogarithm()
        {
            try
            {
                return IsInteger ? BigInteger.Log(ToBigInteger()) : BigRational.Log(ToBigRational());
            }
            catch (Exception exception)
            {
                throw new OverflowException("The resulting natural logarithm cannot be represented by the Number type.", exception);
            }
        }

        /// <summary>
        /// Calculates the nth root of the current <see cref="Number" />.
        /// </summary>
        /// <param name="root">
        /// A positive number representing the root.
        /// </param>
        /// <returns>
        /// The nth root of the current <see cref="Number" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="root" /> is less than one (1).
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result cannot be represented by the <see cref="Number" /> type.
        /// </exception>
        public readonly Number CalculateNthRoot(Int32 root)
        {
            try
            {
                return Math.Pow(Compress().RoundedTo(SignificantFiguresLimitForDouble).ToDouble(), 1d / root.RejectIf().IsLessThan(1, nameof(root)).TargetArgument.ToDouble());
            }
            catch (Exception exception)
            {
                throw new OverflowException("The resulting nth root cannot be represented by the Number type.", exception);
            }
        }

        /// <summary>
        /// Calculates the square root of the current <see cref="Number" />.
        /// </summary>
        /// <returns>
        /// The square root of the current <see cref="Number" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The result cannot be represented by the <see cref="Number" /> type.
        /// </exception>
        public readonly Number CalculateSquareRoot() => CalculateNthRoot(2);

        /// <summary>
        /// Returns the number of significant figures in the value of the current <see cref="Number" />.
        /// </summary>
        /// <returns>
        /// The number of significant figures in the value of the current <see cref="Number" />.
        /// </returns>
        public readonly Int32 CountSignificantFigures() => Format switch
        {
            NumericDataFormat.Byte => ToByte().CountSignificantFigures(),
            NumericDataFormat.SByte => ToSByte().CountSignificantFigures(),
            NumericDataFormat.UInt16 => ToUInt16().CountSignificantFigures(),
            NumericDataFormat.Int16 => ToInt16().CountSignificantFigures(),
            NumericDataFormat.UInt32 => ToUInt32().CountSignificantFigures(),
            NumericDataFormat.Int32 => ToInt32().CountSignificantFigures(),
            NumericDataFormat.UInt64 => ToUInt64().CountSignificantFigures(),
            NumericDataFormat.Int64 => ToInt64().CountSignificantFigures(),
            NumericDataFormat.Single => ToSingle().CountSignificantFigures(),
            NumericDataFormat.Double => ToDouble().CountSignificantFigures(),
            NumericDataFormat.Decimal => ToDecimal().CountSignificantFigures(),
            NumericDataFormat.BigInteger => ToBigInteger().CountSignificantFigures(),
            NumericDataFormat.BigRational => ToBigRational().CountSignificantFigures(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Calculates the result of raising the current <see cref="Number" /> to the specified integer exponent.
        /// </summary>
        /// <param name="exponent">
        /// An exponent value to which the current <see cref="Number" /> is raised.
        /// </param>
        /// <returns>
        /// The result of raising the current <see cref="Number" /> to the specified integer exponent.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="exponent" /> is negative.
        /// </exception>
        public readonly Number RaisedToPower(Int32 exponent) => IsInteger ? BigInteger.Pow(ToBigInteger(), exponent.RejectIf().IsLessThan(0, nameof(exponent)).TargetArgument) : BigRational.Pow(ToBigRational(), exponent.RejectIf().IsLessThan(0, nameof(exponent)).TargetArgument);

        /// <summary>
        /// Rounds the current <see cref="Number" /> to the specified number of fractional digits.
        /// </summary>
        /// <remarks>
        /// This method rounds away from zero by default.
        /// </remarks>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Number" /> to.
        /// </param>
        /// <returns>
        /// The current <see cref="Number" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 15.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result cannot be represented by the <see cref="Number" /> type.
        /// </exception>
        public readonly Number RoundedTo(Int32 digits) => RoundedTo(digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds the current <see cref="Number" /> to the specified number of fractional digits.
        /// </summary>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Number" /> to.
        /// </param>
        /// <param name="midpointRoundingMode">
        /// A specification for how to round if the current value is midway between two numbers.
        /// </param>
        /// <returns>
        /// The current <see cref="Number" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="midpointRoundingMode" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 28.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result cannot be represented by the <see cref="Number" /> type.
        /// </exception>
        public readonly Number RoundedTo(Int32 digits, MidpointRounding midpointRoundingMode) => Format switch
        {
            NumericDataFormat.Byte => this,
            NumericDataFormat.SByte => this,
            NumericDataFormat.UInt16 => this,
            NumericDataFormat.Int16 => this,
            NumericDataFormat.UInt32 => this,
            NumericDataFormat.Int32 => this,
            NumericDataFormat.UInt64 => this,
            NumericDataFormat.Int64 => this,
            NumericDataFormat.Single => IsInteger ? this : new Number(ToSingle().RoundedTo(digits, midpointRoundingMode)).Compress(),
            NumericDataFormat.Double => IsInteger ? this : new Number(ToDouble().RoundedTo(digits, midpointRoundingMode)).Compress(),
            NumericDataFormat.Decimal => IsInteger ? this : new Number(ToDecimal().RoundedTo(digits, midpointRoundingMode)).Compress(),
            NumericDataFormat.BigInteger => this,
            NumericDataFormat.BigRational => IsInteger ? this : new Number(ToBigRational().RoundedTo(digits, midpointRoundingMode)).Compress(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };
    }
}