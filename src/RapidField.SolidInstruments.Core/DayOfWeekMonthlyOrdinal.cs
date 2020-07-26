// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Indicates the ordinal position of a <see cref="DayOfWeek" /> within a month.
    /// </summary>
    [DataContract]
    public enum DayOfWeekMonthlyOrdinal : Int32
    {
        /// <summary>
        /// The ordinal position of the <see cref="DayOfWeek" /> is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// Indicates that the <see cref="DayOfWeek" /> represents the first occurrence in the month.
        /// </summary>
        [EnumMember]
        First = 1,

        /// <summary>
        /// Indicates that the <see cref="DayOfWeek" /> represents the second occurrence in the month.
        /// </summary>
        [EnumMember]
        Second = 2,

        /// <summary>
        /// Indicates that the <see cref="DayOfWeek" /> represents the third occurrence in the month.
        /// </summary>
        [EnumMember]
        Third = 3,

        /// <summary>
        /// Indicates that the <see cref="DayOfWeek" /> represents the fourth but not the last occurrence in the month. Month and
        /// day pairs for which there are only four occurrences in a month use <see cref="Last" /> to identify the fourth and last
        /// occurrence.
        /// </summary>
        [EnumMember]
        Fourth = 4,

        /// <summary>
        /// Indicates that the <see cref="DayOfWeek" /> represents the last occurrence in the month. Month and day pairs for which
        /// there are only four occurrences in a month use <see cref="Last" /> to identify the fourth and last occurrence.
        /// </summary>
        [EnumMember]
        Last = 5
    }
}