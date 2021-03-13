// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    /// <summary>
    /// Represents an equivalent pair of a <see cref="Number" /> and its underlying primitive numeric structure which is made
    /// available for testing.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the primitive numeric structure underlying the test pair.
    /// </typeparam>
    internal abstract class NumericTestPair<TValue> : NumericTestPair
        where TValue : struct, IComparable, IComparable<TValue>, IEquatable<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericTestPair{TValue}" /> class.
        /// </summary>
        /// <param name="numericValue">
        /// The primitive numeric data value defining the numeric test pair.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TValue" /> is an unsupported numeric type.
        /// </exception>
        protected NumericTestPair(TValue numericValue)
            : base(numericValue, DetermineNumericDataFormat(ValueType))
        {
            return;
        }

        /// <summary>
        /// Converts the value of the current <see cref="NumericTestPair{TValue}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="NumericTestPair{TValue}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Format)}\": {Format}, \"{nameof(Number)}\": {Number}, \"{nameof(Value)}\": {Value} }}";

        /// <summary>
        /// Raises an exception if equality and comparison operations are relatively inconsistent for the current and specified
        /// <see cref="NumericTestPair" /> objects.
        /// </summary>
        /// <param name="otherTestPair">
        /// The other test pair against which to evaluate relative state consistency.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// The test pair states are internally or relatively inconsistent.
        /// </exception>
        public sealed override void VerifyRelativeStateConsistency(NumericTestPair otherTestPair)
        {
            VerifyInternalStateConsistency();
            otherTestPair.VerifyInternalStateConsistency();
            var thisValue = ValueAsBigRational;
            var otherValue = otherTestPair.ValueAsBigRational;
            var thisNumber = Number;
            var otherNumber = otherTestPair.Number;
            Boolean thisValueIsEqualToOtherValue;
            Int32 thisValueToOtherValueComparisonResult;
            Boolean thisNumberIsEqualToOtherNumber;
            Int32 thisNumberToOtherNumberComparisonResult;

            try
            {
                thisValueIsEqualToOtherValue = thisValue == otherValue;
                thisValueToOtherValueComparisonResult = thisValue.CompareTo(otherValue);
                thisNumberIsEqualToOtherNumber = thisNumber == otherNumber;
                thisNumberToOtherNumberComparisonResult = thisNumber.CompareTo(otherNumber);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"The test pair states are relatively inconsistent. See inner exception. {{ \"TestPairs\": [ {this}, {otherTestPair} ] }}", exception);
            }

            if (thisValueIsEqualToOtherValue != thisNumberIsEqualToOtherNumber)
            {
                throw new InvalidOperationException($"The test pair states are relatively inconsistent per equality operations. {{ \"TestPairs\": [ {this}, {otherTestPair} ] }}");
            }
            else if (thisValueToOtherValueComparisonResult != thisNumberToOtherNumberComparisonResult)
            {
                throw new InvalidOperationException($"The test pair states are relatively inconsistent per comparison operations. {{ \"TestPairs\": [ {this}, {otherTestPair} ] }}");
            }
        }

        /// <summary>
        /// Raises an exception if the primitive numeric value for the current <see cref="NumericTestPair{TValue}" /> is not equal
        /// to and comparatively equivalent to its own <see cref="Core.Number" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The test pair state is internally inconsistent.
        /// </exception>
        protected internal sealed override void VerifyInternalStateConsistency()
        {
            Boolean internalStateIsConsistent;

            try
            {
                internalStateIsConsistent = VerifyStateConsistency(Value, Number);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"The test pair state is internally inconsistent. See inner exception. {this}", exception);
            }

            if (internalStateIsConsistent)
            {
                return;
            }

            throw new InvalidOperationException($"The test pair state is internally inconsistent. {this}");
        }

        /// <summary>
        /// Converts the specified numeric value to its <see cref="BigRational" /> equivalent.
        /// </summary>
        /// <param name="value">
        /// The numeric value to convert.
        /// </param>
        /// <returns>
        /// The <see cref="BigRational" /> equivalent of the specified numeric value.
        /// </returns>
        protected abstract BigRational ConvertValueToBigRational(TValue value);

        /// <summary>
        /// Returns a value indicating whether or not the primitive numeric value for the current
        /// <see cref="NumericTestPair{TValue}" /> is equal to and comparatively equivalent to its own <see cref="Number" />.
        /// </summary>
        /// <param name="value">
        /// The primitive numeric value.
        /// </param>
        /// <param name="number">
        /// The corresponding <see cref="Number" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the primitive numeric value for the current <see cref="NumericTestPair{TValue}" /> is equal
        /// to and comparatively equivalent to its own <see cref="Number" />, otherwise <see langword="false" />.
        /// </returns>
        protected abstract Boolean VerifyStateConsistency(TValue value, Number number);

        /// <summary>
        /// Returns the numeric data format for the specified value type.
        /// </summary>
        /// <param name="valueType">
        /// The type of a provided numeric value.
        /// </param>
        /// <returns>
        /// The numeric data format for <paramref name="valueType" />.
        /// </returns>
        [DebuggerHidden]
        private static NumericDataFormat DetermineNumericDataFormat(Type valueType)
        {
            if (valueType is null)
            {
                return NumericDataFormat.Unspecified;
            }
            else if (valueType == typeof(Byte))
            {
                return NumericDataFormat.Byte;
            }
            else if (valueType == typeof(SByte))
            {
                return NumericDataFormat.SByte;
            }
            else if (valueType == typeof(UInt16))
            {
                return NumericDataFormat.UInt16;
            }
            else if (valueType == typeof(Int16))
            {
                return NumericDataFormat.Int16;
            }
            else if (valueType == typeof(UInt32))
            {
                return NumericDataFormat.UInt32;
            }
            else if (valueType == typeof(Int32))
            {
                return NumericDataFormat.Int32;
            }
            else if (valueType == typeof(UInt64))
            {
                return NumericDataFormat.UInt64;
            }
            else if (valueType == typeof(Int64))
            {
                return NumericDataFormat.Int64;
            }
            else if (valueType == typeof(Single))
            {
                return NumericDataFormat.Single;
            }
            else if (valueType == typeof(Double))
            {
                return NumericDataFormat.Double;
            }
            else if (valueType == typeof(Decimal))
            {
                return NumericDataFormat.Decimal;
            }
            else if (valueType == typeof(BigInteger))
            {
                return NumericDataFormat.BigInteger;
            }
            else if (valueType == typeof(BigRational))
            {
                return NumericDataFormat.BigRational;
            }
            else
            {
                return NumericDataFormat.Unspecified;
            }
        }

        /// <summary>
        /// Gets the numeric value for the current <see cref="NumericTestPair" /> as a <see cref="BigRational" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal sealed override BigRational ValueAsBigRational => ConvertValueToBigRational(Value);

        /// <summary>
        /// Gets the primitive numeric data value defining the current <see cref="NumericTestPair{TValue}" />.
        /// </summary>
        protected TValue Value => (TValue)NumericValue;

        /// <summary>
        /// Gets the type of the primitive numeric data value defining the current <see cref="NumericTestPair{TValue}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ValueType = typeof(TValue);
    }

    /// <summary>
    /// Represents an equivalent pair of a <see cref="Core.Number" /> and its underlying primitive numeric structure which is made
    /// available for testing.
    /// </summary>
    internal abstract class NumericTestPair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericTestPair" /> class.
        /// </summary>
        /// <param name="numericValue">
        /// The primitive numeric data value defining the numeric test pair.
        /// </param>
        /// <param name="format">
        /// The primitive numeric data type of <paramref name="numericValue" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="numericValue" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="format" /> is equal to <see cref="NumericDataFormat.Unspecified" />.
        /// </exception>
        protected NumericTestPair(Object numericValue, NumericDataFormat format)
        {
            Format = format.RejectIf().IsEqualToValue(NumericDataFormat.Unspecified, nameof(format));
            LazyNumber = new Lazy<Number>(InitializeNumber, LazyThreadSafetyMode.ExecutionAndPublication);
            NumericValue = numericValue.RejectIf().IsNull(nameof(numericValue)).TargetArgument;
        }

        /// <summary>
        /// Raises an exception if arithmetic operations are relatively inconsistent or incorrect for the current and specified
        /// <see cref="NumericTestPair" /> objects.
        /// </summary>
        /// <param name="otherTestPair">
        /// The other test pair against which to evaluate arithmetic operational correctness.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// The test pairs produce incorrect or inconsistent arithmetic results.
        /// </exception>
        public void VerifyArithmeticOperationalCorrectness(NumericTestPair otherTestPair)
        {
            var thisValue = ValueAsBigRational;
            var otherValue = otherTestPair.ValueAsBigRational;
            var thisNumber = Number;
            var otherNumber = otherTestPair.Number;

            try
            {
                var additionValueResult = thisValue + otherValue;
                var additionNumberResult = thisNumber + otherNumber;
                var subtractionValueResult = thisValue - otherValue;
                var subtractionNumberResult = thisNumber - otherNumber;
                var multiplicationValueResult = thisValue * otherValue;
                var multiplicationNumberResult = thisNumber * otherNumber;
                var divisionValueResult = otherValue == BigRational.Zero ? otherValue : thisValue / otherValue;
                var divisionNumberResult = otherNumber == Number.Zero ? otherNumber : thisNumber / otherNumber;
                var additionResultsAreConsistent = additionNumberResult.Equals(additionValueResult);
                var subtractionResultsAreConsistent = subtractionNumberResult.Equals(subtractionValueResult);
                var multiplicationResultsAreConsistent = multiplicationNumberResult.Equals(multiplicationValueResult);
                var divisionResultsAreConsistent = divisionNumberResult.Equals(divisionValueResult);

                if (additionResultsAreConsistent && subtractionResultsAreConsistent && multiplicationResultsAreConsistent && divisionResultsAreConsistent)
                {
                    return;
                }

                throw new InvalidOperationException($"The test pairs produced incorrect or inconsistent arithmetic results. {{ \"TestPairs\": [ {this}, {otherTestPair} ] }}");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"The test pairs produced incorrect or inconsistent arithmetic results. See inner exception. {{ \"TestPairs\": [ {this}, {otherTestPair} ] }}", exception);
            }
        }

        /// <summary>
        /// Raises an exception if equality and comparison operations are relatively inconsistent for the current and specified
        /// <see cref="NumericTestPair" /> objects.
        /// </summary>
        /// <param name="otherTestPair">
        /// The other test pair against which to evaluate relative state consistency.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// The test pair states are internally or relatively inconsistent.
        /// </exception>
        public abstract void VerifyRelativeStateConsistency(NumericTestPair otherTestPair);

        /// <summary>
        /// Raises an exception if the primitive numeric value for the current <see cref="NumericTestPair" /> is not equal to and
        /// comparatively equivalent to its own <see cref="Core.Number" />.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The test pair state is internally inconsistent.
        /// </exception>
        protected internal abstract void VerifyInternalStateConsistency();

        /// <summary>
        /// Initializes the <see cref="Number" /> value for the current <see cref="NumericTestPair" />.
        /// </summary>
        /// <returns>
        /// The <see cref="Number" /> value for the current <see cref="NumericTestPair" />.
        /// </returns>
        [DebuggerHidden]
        private Number InitializeNumber() => Format switch
        {
            NumericDataFormat.Byte => (Byte)NumericValue,
            NumericDataFormat.SByte => (SByte)NumericValue,
            NumericDataFormat.UInt16 => (UInt16)NumericValue,
            NumericDataFormat.Int16 => (Int16)NumericValue,
            NumericDataFormat.UInt32 => (UInt32)NumericValue,
            NumericDataFormat.Int32 => (Int32)NumericValue,
            NumericDataFormat.UInt64 => (UInt64)NumericValue,
            NumericDataFormat.Int64 => (Int64)NumericValue,
            NumericDataFormat.Single => (Single)NumericValue,
            NumericDataFormat.Double => (Double)NumericValue,
            NumericDataFormat.Decimal => (Decimal)NumericValue,
            NumericDataFormat.BigInteger => (BigInteger)NumericValue,
            NumericDataFormat.BigRational => (BigRational)NumericValue,
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Gets an exception message that indicates that <see cref="Format" /> is an unsupported numeric data format.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal String UnsupportedFormatExceptionMessage => $"The specified numeric data format, {Format}, is not supported.";

        /// <summary>
        /// Gets the numeric value for the current <see cref="NumericTestPair" /> as a <see cref="BigRational" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal abstract BigRational ValueAsBigRational
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Core.Number" /> value for the current <see cref="NumericTestPair" />.
        /// </summary>
        protected internal Number Number => LazyNumber.Value;

        /// <summary>
        /// Represents the primitive numeric data type of <see cref="NumericValue" />.
        /// </summary>
        protected readonly NumericDataFormat Format;

        /// <summary>
        /// Represents the primitive numeric data value defining the current <see cref="NumericTestPair" />.
        /// </summary>
        protected readonly Object NumericValue;

        /// <summary>
        /// Represents the lazily-initialized <see cref="Core.Number" /> value for the current <see cref="NumericTestPair" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Number> LazyNumber;
    }
}