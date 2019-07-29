// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="RandomNumberGenerator" /> class with cryptographic features.
    /// </summary>
    public static class RandomNumberGeneratorExtensions
    {
        /// <summary>
        /// Generates the specified number of random bytes and adds them to the referenced collection.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="collection">
        /// A collection to which the random bytes should be added.
        /// </param>
        /// <param name="count">
        /// The number of random bytes to generate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        public static void AddBytes(this RandomNumberGenerator target, ICollection<Byte> collection, Int32 count)
        {
            collection.RejectIf().IsNull(nameof(collection));

            if (count.RejectIf().IsLessThan(0) == 0)
            {
                return;
            }

            var randomBytes = new Byte[count];
            target.GetBytes(randomBytes);
            AddBytes(collection, randomBytes);
        }

        /// <summary>
        /// Generates the specified number of random bytes and enqueues them to the referenced queue.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="queue">
        /// A queue to which the random bytes should be enqueued.
        /// </param>
        /// <param name="count">
        /// The number of random bytes to generate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queue" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        public static void EnqueueBytes(this RandomNumberGenerator target, Queue<Byte> queue, Int32 count)
        {
            if (count.RejectIf().IsLessThan(0) == 0)
            {
                return;
            }

            var randomBytes = new Byte[count];
            target.GetBytes(randomBytes);
            EnqueueBytes(queue.RejectIf().IsNull(nameof(queue)), randomBytes);
        }

        /// <summary>
        /// Generates the specified number of random bytes and enqueues them to the referenced queue.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="queue">
        /// A queue to which the random bytes should be enqueued.
        /// </param>
        /// <param name="count">
        /// The number of random bytes to generate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queue" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        public static void EnqueueBytes(this RandomNumberGenerator target, ConcurrentQueue<Byte> queue, Int32 count)
        {
            if (count.RejectIf().IsLessThan(0) == 0)
            {
                return;
            }

            var randomBytes = new Byte[count];
            target.GetBytes(randomBytes);
            EnqueueBytes(queue.RejectIf().IsNull(nameof(queue)), randomBytes);
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Boolean" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Boolean" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillBooleanArray(this RandomNumberGenerator target, Boolean[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateBoolean(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="DateTime" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="DateTime" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillDateTimeArray(this RandomNumberGenerator target, DateTime[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateDateTime(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="DateTime" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="DateTime" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillDateTimeArray(this RandomNumberGenerator target, DateTime[] array, DateTime floor, DateTime ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateDateTime(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Decimal" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Decimal" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillDecimalArray(this RandomNumberGenerator target, Decimal[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateDecimal(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Decimal" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Decimal" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillDecimalArray(this RandomNumberGenerator target, Decimal[] array, Decimal floor, Decimal ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateDecimal(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Double" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Double" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillDoubleArray(this RandomNumberGenerator target, Double[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateDouble(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Double" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Double" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillDoubleArray(this RandomNumberGenerator target, Double[] array, Double floor, Double ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateDouble(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Int16" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Int16" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillInt16Array(this RandomNumberGenerator target, Int16[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateInt16(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Int16" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Int16" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillInt16Array(this RandomNumberGenerator target, Int16[] array, Int16 floor, Int16 ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateInt16(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Int32" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Int32" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillInt32Array(this RandomNumberGenerator target, Int32[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateInt32(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Int32" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Int32" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillInt32Array(this RandomNumberGenerator target, Int32[] array, Int32 floor, Int32 ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateInt32(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Int64" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Int64" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillInt64Array(this RandomNumberGenerator target, Int64[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateInt64(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Int64" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Int64" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillInt64Array(this RandomNumberGenerator target, Int64[] array, Int64 floor, Int64 ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateInt64(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Single" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Single" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillSingleArray(this RandomNumberGenerator target, Single[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateSingle(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="Single" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="Single" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillSingleArray(this RandomNumberGenerator target, Single[] array, Single floor, Single ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateSingle(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="String" /> values of specified length.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="String" /> values.
        /// </param>
        /// <param name="characterLength">
        /// The character length of the <see cref="String" /> values.
        /// </param>
        /// <param name="permitNonLatin">
        /// A value indicating whether or not to include non-Latin Unicode characters.
        /// </param>
        /// <param name="permitLowercaseAlphabetic">
        /// A value indicating whether or not to include Unicode lowercase alphabetic characters.
        /// </param>
        /// <param name="permitUppercaseAlphabetic">
        /// A value indicating whether or not to include Unicode uppercase alphabetic characters.
        /// </param>
        /// <param name="permitNumeric">
        /// A value indicating whether or not to include Unicode numeric characters (0 - 9).
        /// </param>
        /// <param name="permitSymbolic">
        /// A value indicating whether or not to include Unicode symbolic characters.
        /// </param>
        /// <param name="permitWhiteSpace">
        /// A value indicating whether or not to include Unicode white space characters.
        /// </param>
        /// <param name="permitControl">
        /// A value indicating whether or not to include Unicode control characters.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The specified arguments do not permit any character types.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="characterLength" /> is less than one.
        /// </exception>
        public static void FillStringArray(this RandomNumberGenerator target, String[] array, Int32 characterLength, Boolean permitNonLatin, Boolean permitLowercaseAlphabetic, Boolean permitUppercaseAlphabetic, Boolean permitNumeric, Boolean permitSymbolic, Boolean permitWhiteSpace, Boolean permitControl)
        {
            array.RejectIf().IsNull(nameof(array));
            characterLength.RejectIf().IsLessThan(1, nameof(characterLength));

            if ((permitLowercaseAlphabetic || permitUppercaseAlphabetic || permitNumeric || permitSymbolic || permitWhiteSpace || permitControl) == false)
            {
                throw new ArgumentException(NoCharacterTypesPermittedExceptionMessageTemplate);
            }

            for (var i = 0; i < array.Length; i++)
            {
                GenerateString(target, characterLength, characterLength, permitNonLatin, permitLowercaseAlphabetic, permitUppercaseAlphabetic, permitNumeric, permitSymbolic, permitWhiteSpace, permitControl, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="String" /> values with random lengths within the specified range of
        /// lengths.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="String" /> values.
        /// </param>
        /// <param name="characterLengthFloor">
        /// An inclusive lower boundary for the possible character length of the <see cref="String" />.
        /// </param>
        /// <param name="characterLengthCeiling">
        /// An inclusive upper boundary for the possible character length of the <see cref="String" />.
        /// </param>
        /// <param name="permitNonLatin">
        /// A value indicating whether or not to include non-Latin Unicode characters.
        /// </param>
        /// <param name="permitLowercaseAlphabetic">
        /// A value indicating whether or not to include Unicode lowercase alphabetic characters.
        /// </param>
        /// <param name="permitUppercaseAlphabetic">
        /// A value indicating whether or not to include Unicode uppercase alphabetic characters.
        /// </param>
        /// <param name="permitNumeric">
        /// A value indicating whether or not to include Unicode numeric characters (0 - 9).
        /// </param>
        /// <param name="permitSymbolic">
        /// A value indicating whether or not to include Unicode symbolic characters.
        /// </param>
        /// <param name="permitWhiteSpace">
        /// A value indicating whether or not to include Unicode white space characters.
        /// </param>
        /// <param name="permitControl">
        /// A value indicating whether or not to include Unicode control characters.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The specified arguments do not permit any character types.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="characterLengthFloor" /> is less than zero or the <paramref name="characterLengthFloor" /> argument is
        /// greater than the <paramref name="characterLengthCeiling" />.
        /// </exception>
        public static void FillStringArray(this RandomNumberGenerator target, String[] array, Int32 characterLengthFloor, Int32 characterLengthCeiling, Boolean permitNonLatin, Boolean permitLowercaseAlphabetic, Boolean permitUppercaseAlphabetic, Boolean permitNumeric, Boolean permitSymbolic, Boolean permitWhiteSpace, Boolean permitControl)
        {
            array.RejectIf().IsNull(nameof(array));
            characterLengthFloor.RejectIf().IsLessThan(0, nameof(characterLengthFloor)).OrIf().IsGreaterThan(characterLengthCeiling, nameof(characterLengthFloor), nameof(characterLengthCeiling));

            if ((permitLowercaseAlphabetic || permitUppercaseAlphabetic || permitNumeric || permitSymbolic || permitWhiteSpace || permitControl) == false)
            {
                throw new ArgumentException(NoCharacterTypesPermittedExceptionMessageTemplate);
            }

            for (var i = 0; i < array.Length; i++)
            {
                GenerateString(target, characterLengthFloor, characterLengthCeiling, permitNonLatin, permitLowercaseAlphabetic, permitUppercaseAlphabetic, permitNumeric, permitSymbolic, permitWhiteSpace, permitControl, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="TimeSpan" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="TimeSpan" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillTimeSpanArray(this RandomNumberGenerator target, TimeSpan[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateTimeSpan(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="TimeSpan" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="TimeSpan" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillTimeSpanArray(this RandomNumberGenerator target, TimeSpan[] array, TimeSpan floor, TimeSpan ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateTimeSpan(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="UInt16" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="UInt16" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillUInt16Array(this RandomNumberGenerator target, UInt16[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateUInt16(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="UInt16" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="UInt16" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillUInt16Array(this RandomNumberGenerator target, UInt16[] array, UInt16 floor, UInt16 ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateUInt16(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="UInt32" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="UInt32" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillUInt32Array(this RandomNumberGenerator target, UInt32[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateUInt32(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="UInt32" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="UInt32" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillUInt32Array(this RandomNumberGenerator target, UInt32[] array, UInt32 floor, UInt32 ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateUInt32(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="UInt64" /> values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="UInt64" /> values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        public static void FillUInt64Array(this RandomNumberGenerator target, UInt64[] array)
        {
            array.RejectIf().IsNull(nameof(array));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateUInt64(target, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Fills the referenced array with random <see cref="UInt64" /> values within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="array">
        /// An array to fill with random <see cref="UInt64" /> values.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static void FillUInt64Array(this RandomNumberGenerator target, UInt64[] array, UInt64 floor, UInt64 ceiling)
        {
            array.RejectIf().IsNull(nameof(array));
            floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling));

            for (var i = 0; i < array.Length; i++)
            {
                GenerateUInt64(target, floor, ceiling, out var randomValue);
                array[i] = randomValue;
            }
        }

        /// <summary>
        /// Generates a random <see cref="Boolean" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="Boolean" />.
        /// </returns>
        public static Boolean GetBoolean(this RandomNumberGenerator target)
        {
            GenerateBoolean(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Char" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="Char" />.
        /// </returns>
        public static Char GetChar(this RandomNumberGenerator target)
        {
            GenerateChar(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Char" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="permitNonLatin">
        /// A value indicating whether or not the value can be a non-Latin Unicode character.
        /// </param>
        /// <param name="permitLowercaseAlphabetic">
        /// A value indicating whether or not the value can be a Unicode lowercase alphabetic character.
        /// </param>
        /// <param name="permitUppercaseAlphabetic">
        /// A value indicating whether or not the value can be an Unicode uppercase alphabetic character.
        /// </param>
        /// <param name="permitNumeric">
        /// A value indicating whether or not the value can be a Unicode numeric character (0 - 9).
        /// </param>
        /// <param name="permitSymbolic">
        /// A value indicating whether or not the value can be a Unicode symbolic character.
        /// </param>
        /// <param name="permitWhiteSpace">
        /// A value indicating whether or not the value can be a Unicode white space character.
        /// </param>
        /// <param name="permitControl">
        /// A value indicating whether or not the value can be a Unicode control character.
        /// </param>
        /// <returns>
        /// A random <see cref="Char" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The specified arguments do not permit any character types.
        /// </exception>
        public static Char GetChar(this RandomNumberGenerator target, Boolean permitNonLatin, Boolean permitLowercaseAlphabetic, Boolean permitUppercaseAlphabetic, Boolean permitNumeric, Boolean permitSymbolic, Boolean permitWhiteSpace, Boolean permitControl)
        {
            if ((permitLowercaseAlphabetic || permitUppercaseAlphabetic || permitNumeric || permitSymbolic || permitWhiteSpace || permitControl) == false)
            {
                throw new ArgumentException(NoCharacterTypesPermittedExceptionMessageTemplate);
            }

            GenerateChar(target, permitNonLatin, permitLowercaseAlphabetic, permitUppercaseAlphabetic, permitNumeric, permitSymbolic, permitWhiteSpace, permitControl, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="DateTime" />.
        /// </returns>
        public static DateTime GetDateTime(this RandomNumberGenerator target)
        {
            GenerateDateTime(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="DateTime" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="DateTime" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static DateTime GetDateTime(this RandomNumberGenerator target, DateTime floor, DateTime ceiling)
        {
            GenerateDateTime(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Decimal" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="Decimal" />.
        /// </returns>
        public static Decimal GetDecimal(this RandomNumberGenerator target)
        {
            GenerateDecimal(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Decimal" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="Decimal" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static Decimal GetDecimal(this RandomNumberGenerator target, Decimal floor, Decimal ceiling)
        {
            GenerateDecimal(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Double" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="Double" />.
        /// </returns>
        public static Double GetDouble(this RandomNumberGenerator target)
        {
            GenerateDouble(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Double" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="Double" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static Double GetDouble(this RandomNumberGenerator target, Double floor, Double ceiling)
        {
            GenerateDouble(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Int16" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="Int16" />.
        /// </returns>
        public static Int16 GetInt16(this RandomNumberGenerator target)
        {
            GenerateInt16(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Int16" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="Int16" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static Int16 GetInt16(this RandomNumberGenerator target, Int16 floor, Int16 ceiling)
        {
            GenerateInt16(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Int32" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="Int32" /> within the specified range.
        /// </returns>
        public static Int32 GetInt32(this RandomNumberGenerator target)
        {
            GenerateInt32(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Int32" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="Int32" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static Int32 GetInt32(this RandomNumberGenerator target, Int32 floor, Int32 ceiling)
        {
            GenerateInt32(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Int64" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="Int64" />.
        /// </returns>
        public static Int64 GetInt64(this RandomNumberGenerator target)
        {
            GenerateInt64(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Int64" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="Int64" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static Int64 GetInt64(this RandomNumberGenerator target, Int64 floor, Int64 ceiling)
        {
            GenerateInt64(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Single" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="Single" />.
        /// </returns>
        public static Single GetSingle(this RandomNumberGenerator target)
        {
            GenerateSingle(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="Single" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="Single" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static Single GetSingle(this RandomNumberGenerator target, Single floor, Single ceiling)
        {
            GenerateSingle(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="String" /> of specified length.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="characterLength">
        /// The character length of the <see cref="String" />.
        /// </param>
        /// <param name="permitNonLatin">
        /// A value indicating whether or not to include non-Latin Unicode characters.
        /// </param>
        /// <param name="permitLowercaseAlphabetic">
        /// A value indicating whether or not to include Unicode lowercase alphabetic characters.
        /// </param>
        /// <param name="permitUppercaseAlphabetic">
        /// A value indicating whether or not to include Unicode uppercase alphabetic characters.
        /// </param>
        /// <param name="permitNumeric">
        /// A value indicating whether or not to include Unicode numeric characters (0 - 9).
        /// </param>
        /// <param name="permitSymbolic">
        /// A value indicating whether or not to include Unicode symbolic characters.
        /// </param>
        /// <param name="permitWhiteSpace">
        /// A value indicating whether or not to include Unicode white space characters.
        /// </param>
        /// <param name="permitControl">
        /// A value indicating whether or not to include Unicode control characters.
        /// </param>
        /// <returns>
        /// A random <see cref="String" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The specified arguments do not permit any character types.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="characterLength" /> is less than one.
        /// </exception>
        public static String GetString(this RandomNumberGenerator target, Int32 characterLength, Boolean permitNonLatin, Boolean permitLowercaseAlphabetic, Boolean permitUppercaseAlphabetic, Boolean permitNumeric, Boolean permitSymbolic, Boolean permitWhiteSpace, Boolean permitControl)
        {
            if ((permitLowercaseAlphabetic || permitUppercaseAlphabetic || permitNumeric || permitSymbolic || permitWhiteSpace || permitControl) == false)
            {
                throw new ArgumentException(NoCharacterTypesPermittedExceptionMessageTemplate);
            }

            GenerateString(target, characterLength.RejectIf().IsLessThan(1, nameof(characterLength)), characterLength, permitNonLatin, permitLowercaseAlphabetic, permitUppercaseAlphabetic, permitNumeric, permitSymbolic, permitWhiteSpace, permitControl, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="String" /> with a random length within the specified range of lengths.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="characterLengthFloor">
        /// An inclusive lower boundary for the possible character length of the <see cref="String" />.
        /// </param>
        /// <param name="characterLengthCeiling">
        /// An inclusive upper boundary for the possible character length of the <see cref="String" />.
        /// </param>
        /// <param name="permitNonLatin">
        /// A value indicating whether or not to include non-Latin Unicode characters.
        /// </param>
        /// <param name="permitLowercaseAlphabetic">
        /// A value indicating whether or not to include Unicode lowercase alphabetic characters.
        /// </param>
        /// <param name="permitUppercaseAlphabetic">
        /// A value indicating whether or not to include Unicode uppercase alphabetic characters.
        /// </param>
        /// <param name="permitNumeric">
        /// A value indicating whether or not to include Unicode numeric characters (0 - 9).
        /// </param>
        /// <param name="permitSymbolic">
        /// A value indicating whether or not to include Unicode symbolic characters.
        /// </param>
        /// <param name="permitWhiteSpace">
        /// A value indicating whether or not to include Unicode white space characters.
        /// </param>
        /// <param name="permitControl">
        /// A value indicating whether or not to include Unicode control characters.
        /// </param>
        /// <returns>
        /// A random <see cref="String" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The specified arguments do not permit any character types.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="characterLengthFloor" /> is less than zero or the <paramref name="characterLengthFloor" /> argument is
        /// greater than the <paramref name="characterLengthCeiling" />.
        /// </exception>
        public static String GetString(this RandomNumberGenerator target, Int32 characterLengthFloor, Int32 characterLengthCeiling, Boolean permitNonLatin, Boolean permitLowercaseAlphabetic, Boolean permitUppercaseAlphabetic, Boolean permitNumeric, Boolean permitSymbolic, Boolean permitWhiteSpace, Boolean permitControl)
        {
            characterLengthFloor.RejectIf().IsLessThan(0, nameof(characterLengthFloor)).OrIf().IsGreaterThan(characterLengthCeiling, nameof(characterLengthFloor), nameof(characterLengthCeiling));

            if ((permitLowercaseAlphabetic || permitUppercaseAlphabetic || permitNumeric || permitSymbolic || permitWhiteSpace || permitControl) == false)
            {
                throw new ArgumentException(NoCharacterTypesPermittedExceptionMessageTemplate);
            }

            GenerateString(target, characterLengthFloor, characterLengthCeiling, permitNonLatin, permitLowercaseAlphabetic, permitUppercaseAlphabetic, permitNumeric, permitSymbolic, permitWhiteSpace, permitControl, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="TimeSpan" />.
        /// </returns>
        public static TimeSpan GetTimeSpan(this RandomNumberGenerator target)
        {
            GenerateTimeSpan(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="TimeSpan" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="TimeSpan" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static TimeSpan GetTimeSpan(this RandomNumberGenerator target, TimeSpan floor, TimeSpan ceiling)
        {
            GenerateTimeSpan(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="UInt16" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="UInt16" />.
        /// </returns>
        public static UInt16 GetUInt16(this RandomNumberGenerator target)
        {
            GenerateUInt16(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="UInt16" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="UInt16" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static UInt16 GetUInt16(this RandomNumberGenerator target, UInt16 floor, UInt16 ceiling)
        {
            GenerateUInt16(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="UInt32" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="UInt32" /> within the specified range.
        /// </returns>
        public static UInt32 GetUInt32(this RandomNumberGenerator target)
        {
            GenerateUInt32(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="UInt32" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="UInt32" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static UInt32 GetUInt32(this RandomNumberGenerator target, UInt32 floor, UInt32 ceiling)
        {
            GenerateUInt32(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="UInt64" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <returns>
        /// A random <see cref="UInt64" />.
        /// </returns>
        public static UInt64 GetUInt64(this RandomNumberGenerator target)
        {
            GenerateUInt64(target, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates a random <see cref="UInt64" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <returns>
        /// A random <see cref="UInt64" /> within the specified range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="floor" /> is greater than <paramref name="ceiling" />.
        /// </exception>
        public static UInt64 GetUInt64(this RandomNumberGenerator target, UInt64 floor, UInt64 ceiling)
        {
            GenerateUInt64(target, floor.RejectIf().IsGreaterThan(ceiling, nameof(floor), nameof(ceiling)), ceiling, out var randomValue);
            return randomValue;
        }

        /// <summary>
        /// Generates the specified number of random bytes and pushes them onto the referenced stack.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="stack">
        /// A stack onto which the random bytes should be pushed.
        /// </param>
        /// <param name="count">
        /// The number of random bytes to generate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stack" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        public static void PushBytes(this RandomNumberGenerator target, Stack<Byte> stack, Int32 count)
        {
            if (count.RejectIf().IsLessThan(0) == 0)
            {
                return;
            }

            var randomBytes = new Byte[count];
            target.GetBytes(randomBytes);
            PushBytes(stack.RejectIf().IsNull(nameof(stack)), randomBytes);
        }

        /// <summary>
        /// Generates the specified number of random bytes and pushes them onto the referenced stack.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="RandomNumberGenerator" />.
        /// </param>
        /// <param name="stack">
        /// A stack onto which the random bytes should be pushed.
        /// </param>
        /// <param name="count">
        /// The number of random bytes to generate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stack" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        public static void PushBytes(this RandomNumberGenerator target, ConcurrentStack<Byte> stack, Int32 count)
        {
            if (count.RejectIf().IsLessThan(0) == 0)
            {
                return;
            }

            var randomBytes = new Byte[count];
            target.GetBytes(randomBytes);
            PushBytes(stack.RejectIf().IsNull(nameof(stack)), randomBytes);
        }

        /// <summary>
        /// Adds the specified random byte collection to the specified collection.
        /// </summary>
        /// <param name="collection">
        /// A collection to which the random bytes should be added.
        /// </param>
        /// <param name="randomBytes">
        /// The random byte collection to add.
        /// </param>
        [DebuggerHidden]
        private static void AddBytes(ICollection<Byte> collection, IEnumerable<Byte> randomBytes)
        {
            foreach (var randomByte in randomBytes)
            {
                collection.Add(randomByte);
            }
        }

        /// <summary>
        /// Enqueues the specified random byte collection using the specified queue.
        /// </summary>
        /// <param name="queue">
        /// A queue to which the random bytes should be enqueued.
        /// </param>
        /// <param name="randomBytes">
        /// The random byte collection to enqueue.
        /// </param>
        [DebuggerHidden]
        private static void EnqueueBytes(Queue<Byte> queue, IEnumerable<Byte> randomBytes)
        {
            foreach (var randomByte in randomBytes)
            {
                queue.Enqueue(randomByte);
            }
        }

        /// <summary>
        /// Enqueues the specified random byte collection using the specified queue.
        /// </summary>
        /// <param name="queue">
        /// A queue to which the random bytes should be enqueued.
        /// </param>
        /// <param name="randomBytes">
        /// The random byte collection to enqueue.
        /// </param>
        [DebuggerHidden]
        private static void EnqueueBytes(ConcurrentQueue<Byte> queue, IEnumerable<Byte> randomBytes)
        {
            foreach (var randomByte in randomBytes)
            {
                queue.Enqueue(randomByte);
            }
        }

        /// <summary>
        /// Generates a random <see cref="Boolean" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Boolean" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateBoolean(RandomNumberGenerator target, out Boolean randomValue)
        {
            var buffer = new Byte[BooleanByteLength];
            target.GetBytes(buffer);
            buffer[0] >>= 7;
            randomValue = BitConverter.ToBoolean(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="Char" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Char" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateChar(RandomNumberGenerator target, out Char randomValue)
        {
            var buffer = new Byte[UnicodeCharByteLength];
            target.GetBytes(buffer);
            randomValue = Encoding.Unicode.GetChars(buffer, 0, UnicodeCharByteLength).First();
        }

        /// <summary>
        /// Generates a random <see cref="Char" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="permitNonLatin">
        /// A value indicating whether or not the value can be a non-Latin Unicode character.
        /// </param>
        /// <param name="permitLowercaseAlphabetic">
        /// A value indicating whether or not the value can be a Unicode lowercase alphabetic character.
        /// </param>
        /// <param name="permitUppercaseAlphabetic">
        /// A value indicating whether or not the value can be an Unicode uppercase alphabetic character.
        /// </param>
        /// <param name="permitNumeric">
        /// A value indicating whether or not the value can be a Unicode numeric character (0 - 9).
        /// </param>
        /// <param name="permitSymbolic">
        /// A value indicating whether or not the value can be a Unicode symbolic character.
        /// </param>
        /// <param name="permitWhiteSpace">
        /// A value indicating whether or not the value can be a Unicode white space character.
        /// </param>
        /// <param name="permitControl">
        /// A value indicating whether or not the value can be a Unicode control character.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Char" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateChar(RandomNumberGenerator target, Boolean permitNonLatin, Boolean permitLowercaseAlphabetic, Boolean permitUppercaseAlphabetic, Boolean permitNumeric, Boolean permitSymbolic, Boolean permitWhiteSpace, Boolean permitControl, out Char randomValue)
        {
            var characterEncoding = permitNonLatin ? Encoding.Unicode : Encoding.ASCII;
            var characterByteLength = permitNonLatin ? UnicodeCharByteLength : AsciiCharByteLength;
            var buffer = new Byte[characterByteLength * CharacterBufferLengthMultiplier];
            var singlePassIterationCount = (buffer.Length - characterByteLength);
            target.GetBytes(buffer);

            while (PermuteCharacterGeneration(buffer, singlePassIterationCount, characterByteLength, characterEncoding, permitNonLatin, permitLowercaseAlphabetic, permitUppercaseAlphabetic, permitNumeric, permitSymbolic, permitWhiteSpace, permitControl, out randomValue) == false)
            {
                target.GetBytes(buffer);
            }
        }

        /// <summary>
        /// Generates a random <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="DateTime" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateDateTime(RandomNumberGenerator target, out DateTime randomValue)
        {
            GenerateInt64(target, DateTimeTickFloor, DateTimeTickCeiling, out var ticks);
            randomValue = new DateTime(ticks, RandomDateTimeKind);
        }

        /// <summary>
        /// Generates a random <see cref="DateTime" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="DateTime" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateDateTime(RandomNumberGenerator target, DateTime floor, DateTime ceiling, out DateTime randomValue)
        {
            GenerateInt64(target, floor.Ticks, ceiling.Ticks, out var ticks);
            randomValue = new DateTime(ticks, RandomDateTimeKind);
        }

        /// <summary>
        /// Generates a random <see cref="Decimal" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Decimal" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateDecimal(RandomNumberGenerator target, out Decimal randomValue)
        {
            GenerateInt32(target, out var lowBits);
            GenerateInt32(target, out var middleBits);
            GenerateInt32(target, out var highBits);
            GenerateBoolean(target, out var isNegative);

            var scale = new Byte[1] { 0xff };

            while (scale[0] > 0x1c)
            {
                target.GetBytes(scale);
            }

            randomValue = new Decimal(lowBits, middleBits, highBits, isNegative, scale[0]);
        }

        /// <summary>
        /// Generates a random <see cref="Decimal" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Decimal" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateDecimal(RandomNumberGenerator target, Decimal floor, Decimal ceiling, out Decimal randomValue)
        {
            GenerateRangePosition(target, (ceiling - floor), out var rangePosition);
            randomValue = (floor + rangePosition);
        }

        /// <summary>
        /// Generates a random <see cref="Double" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Double" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateDouble(RandomNumberGenerator target, out Double randomValue)
        {
            var buffer = new Byte[DoubleByteLength];
            target.GetBytes(buffer);
            randomValue = BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="Double" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Double" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateDouble(RandomNumberGenerator target, Double floor, Double ceiling, out Double randomValue)
        {
            var range = Convert.ToDecimal(ceiling - floor);
            GenerateRangePosition(target, range, out var rangePosition);
            randomValue = (floor + Convert.ToDouble(rangePosition));
        }

        /// <summary>
        /// Generates a random <see cref="Int16" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Int16" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateInt16(RandomNumberGenerator target, out Int16 randomValue)
        {
            var buffer = new Byte[Int16ByteLength];
            target.GetBytes(buffer);
            randomValue = BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="Int16" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Int16" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateInt16(RandomNumberGenerator target, Int16 floor, Int16 ceiling, out Int16 randomValue)
        {
            var range = Convert.ToDecimal(ceiling - floor);
            GenerateRangePosition(target, range, out var rangePosition);
            randomValue = Convert.ToInt16(floor + rangePosition.RoundedTo(0));
        }

        /// <summary>
        /// Generates a random <see cref="Int32" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Int32" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateInt32(RandomNumberGenerator target, out Int32 randomValue)
        {
            var buffer = new Byte[Int32ByteLength];
            target.GetBytes(buffer);
            randomValue = BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="Int32" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Int32" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateInt32(RandomNumberGenerator target, Int32 floor, Int32 ceiling, out Int32 randomValue)
        {
            var range = Convert.ToDecimal(ceiling - floor);
            GenerateRangePosition(target, range, out var rangePosition);
            randomValue = Convert.ToInt32(floor + rangePosition.RoundedTo(0));
        }

        /// <summary>
        /// Generates a random <see cref="Int64" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Int64" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateInt64(RandomNumberGenerator target, out Int64 randomValue)
        {
            var buffer = new Byte[Int64ByteLength];
            target.GetBytes(buffer);
            randomValue = BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="Int64" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Int64" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateInt64(RandomNumberGenerator target, Int64 floor, Int64 ceiling, out Int64 randomValue)
        {
            var range = BigInteger.Subtract(new BigInteger(ceiling), new BigInteger(floor));
            GenerateRangePosition(target, (Decimal)range, out var rangePosition);
            randomValue = Convert.ToInt64(floor + rangePosition.RoundedTo(0));
        }

        /// <summary>
        /// Generates a random numeric position within the specified range.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="range">
        /// A numeric range within which to generate a position.
        /// </param>
        /// <param name="rangePosition">
        /// The generated random position within the specified range.
        /// </param>
        [DebuggerHidden]
        private static void GenerateRangePosition(RandomNumberGenerator target, Decimal range, out Decimal rangePosition)
        {
            GenerateInt32(target, out var lowBits);
            GenerateInt32(target, out var middleBits);

            var highBits = UInt32.MaxValue;

            while (highBits > 542101086)
            {
                GenerateUInt32(target, out highBits);
            }

            var rationalMultiplier = new Decimal(lowBits, middleBits, Convert.ToInt32(highBits), false, 28);
            rangePosition = (range * rationalMultiplier);
        }

        /// <summary>
        /// Generates a random <see cref="Single" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Single" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateSingle(RandomNumberGenerator target, out Single randomValue)
        {
            var buffer = new Byte[SingleByteLength];
            target.GetBytes(buffer);
            randomValue = BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="Single" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Single" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateSingle(RandomNumberGenerator target, Single floor, Single ceiling, out Single randomValue)
        {
            var range = Convert.ToDecimal(ceiling - floor);
            GenerateRangePosition(target, range, out var rangePosition);
            randomValue = (floor + Convert.ToSingle(rangePosition));
        }

        /// <summary>
        /// Generates a random <see cref="String" /> with a random length within the specified range of lengths.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="characterLengthFloor">
        /// An inclusive lower boundary for the possible character length of the <see cref="String" />.
        /// </param>
        /// <param name="characterLengthCeiling">
        /// An inclusive upper boundary for the possible character length of the <see cref="String" />.
        /// </param>
        /// <param name="permitNonLatin">
        /// A value indicating whether or not to include non-Latin Unicode characters.
        /// </param>
        /// <param name="permitLowercaseAlphabetic">
        /// A value indicating whether or not to include Unicode lowercase alphabetic characters.
        /// </param>
        /// <param name="permitUppercaseAlphabetic">
        /// A value indicating whether or not to include Unicode uppercase alphabetic characters.
        /// </param>
        /// <param name="permitNumeric">
        /// A value indicating whether or not to include Unicode numeric characters (0 - 9).
        /// </param>
        /// <param name="permitSymbolic">
        /// A value indicating whether or not to include Unicode symbolic characters.
        /// </param>
        /// <param name="permitWhiteSpace">
        /// A value indicating whether or not to include Unicode white space characters.
        /// </param>
        /// <param name="permitControl">
        /// A value indicating whether or not to include Unicode control characters.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="String" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateString(RandomNumberGenerator target, Int32 characterLengthFloor, Int32 characterLengthCeiling, Boolean permitNonLatin, Boolean permitLowercaseAlphabetic, Boolean permitUppercaseAlphabetic, Boolean permitNumeric, Boolean permitSymbolic, Boolean permitWhiteSpace, Boolean permitControl, out String randomValue)
        {
            Int32 characterLength;

            if (characterLengthFloor == characterLengthCeiling)
            {
                characterLength = characterLengthFloor;
            }
            else
            {
                GenerateInt32(target, characterLengthFloor, characterLengthCeiling, out characterLength);
            }

            var buffer = new Char[characterLength];

            for (var i = 0; i < characterLength; i++)
            {
                GenerateChar(target, permitNonLatin, permitLowercaseAlphabetic, permitUppercaseAlphabetic, permitNumeric, permitSymbolic, permitWhiteSpace, permitControl, out var randomCharacter);
                buffer[i] = randomCharacter;
            }

            randomValue = new String(buffer);
        }

        /// <summary>
        /// Generates a random <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="TimeSpan" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateTimeSpan(RandomNumberGenerator target, out TimeSpan randomValue)
        {
            GenerateInt64(target, TimeSpanTickFloor, TimeSpanTickCeiling, out var ticks);
            randomValue = new TimeSpan(ticks);
        }

        /// <summary>
        /// Generates a random <see cref="TimeSpan" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="TimeSpan" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateTimeSpan(RandomNumberGenerator target, TimeSpan floor, TimeSpan ceiling, out TimeSpan randomValue)
        {
            GenerateInt64(target, floor.Ticks, ceiling.Ticks, out var ticks);
            randomValue = new TimeSpan(ticks);
        }

        /// <summary>
        /// Generates a random <see cref="UInt16" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="UInt16" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateUInt16(RandomNumberGenerator target, out UInt16 randomValue)
        {
            var buffer = new Byte[UInt16ByteLength];
            target.GetBytes(buffer);
            randomValue = BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="UInt16" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="UInt16" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateUInt16(RandomNumberGenerator target, UInt16 floor, UInt16 ceiling, out UInt16 randomValue)
        {
            var range = (ceiling - floor);
            GenerateRangePosition(target, range, out var rangePosition);
            randomValue = Convert.ToUInt16(floor + rangePosition.RoundedTo(0));
        }

        /// <summary>
        /// Generates a random <see cref="UInt32" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="UInt32" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateUInt32(RandomNumberGenerator target, out UInt32 randomValue)
        {
            var buffer = new Byte[UInt32ByteLength];
            target.GetBytes(buffer);
            randomValue = BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="UInt32" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="UInt32" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateUInt32(RandomNumberGenerator target, UInt32 floor, UInt32 ceiling, out UInt32 randomValue)
        {
            var range = (ceiling - floor);
            GenerateRangePosition(target, range, out var rangePosition);
            randomValue = Convert.ToUInt32(floor + rangePosition.RoundedTo(0));
        }

        /// <summary>
        /// Generates a random <see cref="UInt64" />.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="UInt64" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateUInt64(RandomNumberGenerator target, out UInt64 randomValue)
        {
            var buffer = new Byte[UInt64ByteLength];
            target.GetBytes(buffer);
            randomValue = BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        /// Generates a random <see cref="UInt64" /> within the specified range of values.
        /// </summary>
        /// <param name="target">
        /// A <see cref="RandomNumberGenerator" /> that generates the random value.
        /// </param>
        /// <param name="floor">
        /// An inclusive lower boundary for the possible range of values.
        /// </param>
        /// <param name="ceiling">
        /// An inclusive upper boundary for the possible range of values.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="UInt64" />.
        /// </param>
        [DebuggerHidden]
        private static void GenerateUInt64(RandomNumberGenerator target, UInt64 floor, UInt64 ceiling, out UInt64 randomValue)
        {
            var range = BigInteger.Subtract(new BigInteger(ceiling), new BigInteger(floor));
            GenerateRangePosition(target, (Decimal)range, out var rangePosition);
            randomValue = Convert.ToUInt64(floor + rangePosition.RoundedTo(0));
        }

        /// <summary>
        /// Executes a single pass of the character generation operation.
        /// </summary>
        /// <param name="buffer">
        /// A buffer containing the randomly-generated character bytes.
        /// </param>
        /// <param name="iterationCount">
        /// The number of iterations to execute.
        /// </param>
        /// <param name="characterByteLength">
        /// The length of a single character, in bytes.
        /// </param>
        /// <param name="characterEncoding">
        /// The text encoding to use.
        /// </param>
        /// <param name="permitNonLatin">
        /// A value indicating whether or not the value can be a non-Latin Unicode character.
        /// </param>
        /// <param name="permitLowercaseAlphabetic">
        /// A value indicating whether or not the value can be a Unicode lowercase alphabetic character.
        /// </param>
        /// <param name="permitUppercaseAlphabetic">
        /// A value indicating whether or not the value can be an Unicode uppercase alphabetic character.
        /// </param>
        /// <param name="permitNumeric">
        /// A value indicating whether or not the value can be a Unicode numeric character (0 - 9).
        /// </param>
        /// <param name="permitSymbolic">
        /// A value indicating whether or not the value can be a Unicode symbolic character.
        /// </param>
        /// <param name="permitWhiteSpace">
        /// A value indicating whether or not the value can be a Unicode white space character.
        /// </param>
        /// <param name="permitControl">
        /// A value indicating whether or not the value can be a Unicode control character.
        /// </param>
        /// <param name="randomValue">
        /// The generated random <see cref="Char" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the random character was generated successfully, otherwise <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        private static Boolean PermuteCharacterGeneration(Byte[] buffer, Int32 iterationCount, Int32 characterByteLength, Encoding characterEncoding, Boolean permitNonLatin, Boolean permitLowercaseAlphabetic, Boolean permitUppercaseAlphabetic, Boolean permitNumeric, Boolean permitSymbolic, Boolean permitWhiteSpace, Boolean permitControl, out Char randomValue)
        {
            var operationIsSuccessful = false;

            for (var i = 0; i < iterationCount; i++)
            {
                if (permitNonLatin == false && buffer[i] > 0x7f)
                {
                    // 0x7f is the last valid ASCII character.
                    continue;
                }

                randomValue = characterEncoding.GetChars(buffer, i, characterByteLength).First();
                operationIsSuccessful = operationIsSuccessful || (permitLowercaseAlphabetic && randomValue.IsLowercaseAlphabetic());
                operationIsSuccessful = operationIsSuccessful || (permitUppercaseAlphabetic && randomValue.IsUppercaseAlphabetic());
                operationIsSuccessful = operationIsSuccessful || (permitNumeric && randomValue.IsNumeric());
                operationIsSuccessful = operationIsSuccessful || (permitSymbolic && randomValue.IsSymbolic());
                operationIsSuccessful = operationIsSuccessful || (permitWhiteSpace && randomValue.IsWhiteSpaceCharacter());
                operationIsSuccessful = operationIsSuccessful || (permitControl && randomValue.IsControlCharacter());

                if (operationIsSuccessful)
                {
                    return true;
                }
            }

            randomValue = default;
            return operationIsSuccessful;
        }

        /// <summary>
        /// Pushes the specified random byte collection onto the specified stack.
        /// </summary>
        /// <param name="stack">
        /// A stack onto which the random bytes should be pushed.
        /// </param>
        /// <param name="randomBytes">
        /// The random byte collection to push.
        /// </param>
        [DebuggerHidden]
        private static void PushBytes(Stack<Byte> stack, IEnumerable<Byte> randomBytes)
        {
            foreach (var randomByte in randomBytes)
            {
                stack.Push(randomByte);
            }
        }

        /// <summary>
        /// Pushes the specified random byte collection onto the specified stack.
        /// </summary>
        /// <param name="stack">
        /// A stack onto which the random bytes should be pushed.
        /// </param>
        /// <param name="randomBytes">
        /// The random byte collection to push.
        /// </param>
        [DebuggerHidden]
        private static void PushBytes(ConcurrentStack<Byte> stack, IEnumerable<Byte> randomBytes)
        {
            foreach (var randomByte in randomBytes)
            {
                stack.Push(randomByte);
            }
        }

        /// <summary>
        /// Represents the byte length of an ASCII <see cref="Char" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 AsciiCharByteLength = 1;

        /// <summary>
        /// Represents the byte length of a <see cref="Boolean" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 BooleanByteLength = 1;

        /// <summary>
        /// Represents the number of characters to generate for each pass when invoking
        /// <see cref="GenerateChar(RandomNumberGenerator, Boolean, Boolean, Boolean, Boolean, Boolean, Boolean, Boolean, out Char)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 CharacterBufferLengthMultiplier = 2;

        /// <summary>
        /// Represents the byte length of a <see cref="Decimal" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DecimalByteLength = 16;

        /// <summary>
        /// Represents the byte length of a <see cref="Double" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DoubleByteLength = 8;

        /// <summary>
        /// Represents the byte length of an <see cref="Int16" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 Int16ByteLength = 2;

        /// <summary>
        /// Represents the byte length of an <see cref="Int32" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 Int32ByteLength = 4;

        /// <summary>
        /// Represents the byte length of an <see cref="Int64" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 Int64ByteLength = 8;

        /// <summary>
        /// Represents a message for exceptions that are raised when random string or character options do not specify any character
        /// types.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String NoCharacterTypesPermittedExceptionMessageTemplate = "At least one character type must be permitted when generating a random character or string.";

        /// <summary>
        /// Represents the <see cref="DateTimeKind" /> that is assigned to new randomly generated <see cref="DateTime" /> values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const DateTimeKind RandomDateTimeKind = DateTimeKind.Utc;

        /// <summary>
        /// Represents the byte length of a <see cref="Single" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 SingleByteLength = 4;

        /// <summary>
        /// Represents the byte length of an <see cref="UInt16" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 UInt16ByteLength = 2;

        /// <summary>
        /// Represents the byte length of an <see cref="UInt32" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 UInt32ByteLength = 4;

        /// <summary>
        /// Represents the byte length of an <see cref="UInt64" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 UInt64ByteLength = 8;

        /// <summary>
        /// Represents the byte length of a Unicode <see cref="Char" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 UnicodeCharByteLength = 2;

        /// <summary>
        /// Represents the maximum boundary, in ticks, for new randomly generated <see cref="DateTime" /> values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int64 DateTimeTickCeiling = DateTime.MaxValue.Ticks;

        /// <summary>
        /// Represents the minimum boundary, in ticks, for new randomly generated <see cref="DateTime" /> values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int64 DateTimeTickFloor = DateTime.MinValue.Ticks;

        /// <summary>
        /// Represents the maximum boundary, in ticks, for new randomly generated <see cref="TimeSpan" /> values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int64 TimeSpanTickCeiling = TimeSpan.MaxValue.Ticks;

        /// <summary>
        /// Represents the minimum boundary, in ticks, for new randomly generated <see cref="TimeSpan" /> values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int64 TimeSpanTickFloor = TimeSpan.MinValue.Ticks;
    }
}