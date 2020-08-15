// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Mathematics.Statistics;
using RapidField.SolidInstruments.Mathematics.Statistics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Cryptography.UnitTests
{
    /// <summary>
    /// Represents analytic information about a random byte array.
    /// </summary>
    internal class RandomByteArrayProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomByteArrayProfile" /> class.
        /// </summary>
        /// <param name="array">
        /// The random byte array to evaluate.
        /// </param>
        public RandomByteArrayProfile(Byte[] array)
        {
            ArrayLength = 0;
            Records = new List<ByteRecord>();

            for (var i = 0; i < 256; i++)
            {
                Records.Add(new ByteRecord((Byte)i));
            }

            foreach (var value in array)
            {
                RecordValue(value);
            }

            AverageLengthBetweenStatistics = Records.Select(record => record.AverageLengthBetween).ComputeDescriptives();
            CountStatistics = Records.Select(record => record.Count).ComputeDescriptives();
        }

        /// <summary>
        /// Records information about the current value in the associated array.
        /// </summary>
        /// <param name="value">
        /// The current byte value.
        /// </param>
        private void RecordValue(Byte value)
        {
            ArrayLength++;

            foreach (var record in Records)
            {
                var valueHasBeenRecorded = record.Count > 0;

                if (record.Value == value)
                {
                    record.Count++;

                    if (valueHasBeenRecorded)
                    {
                        record.LengthsBetween.Add(record.LengthFromLastOccurrence);
                        record.LengthFromLastOccurrence = 0;
                    }
                }
                else if (valueHasBeenRecorded)
                {
                    record.LengthFromLastOccurrence++;
                }
            }
        }

        /// <summary>
        /// Represents descriptive statistics for the average lengths between values in the associated array.
        /// </summary>
        internal readonly DescriptiveStatistics AverageLengthBetweenStatistics;

        /// <summary>
        /// Represents descriptive statistics for the counts of values in the associated array.
        /// </summary>
        internal readonly DescriptiveStatistics CountStatistics;

        /// <summary>
        /// Represents information about the full range of bytes in the associated array.
        /// </summary>
        internal readonly IList<ByteRecord> Records;

        /// <summary>
        /// Represents the total length of the associated array.
        /// </summary>
        internal Int64 ArrayLength;

        /// <summary>
        /// Represents information about a byte value within a random byte array.
        /// </summary>
        internal class ByteRecord
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ByteRecord" /> class.
            /// </summary>
            /// <param name="value">
            /// The byte value for the current statistics.
            /// </param>
            public ByteRecord(Byte value)
            {
                Count = 0;
                LengthFromLastOccurrence = 0;
                LengthsBetween = new List<Int32>();
                Value = value;
            }

            /// <summary>
            /// Gets the average length between occurrences of <see cref="Value" /> in the associated array.
            /// </summary>
            public Double AverageLengthBetween => LengthsBetween.Any() ? LengthsBetween.Average() : 0d;

            /// <summary>
            /// Gets or sets the frequency of <see cref="Value" /> in the associated array.
            /// </summary>
            public Int64 Count
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the length from the last occurrence of <see cref="Value" /> at the current index in the associated
            /// array.
            /// </summary>
            public Int32 LengthFromLastOccurrence
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the recorded lengths between occurrences of <see cref="Value" /> in the associated array.
            /// </summary>
            public IList<Int32> LengthsBetween
            {
                get;
            }

            /// <summary>
            /// Represents the byte value for the current statistics.
            /// </summary>
            public Byte Value;
        }
    }
}