// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    /// <summary>
    /// Represents an <see cref="Model" /> derivative that is used for testing.
    /// </summary>
    [DataContract]
    internal class SimulatedModel : Model
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedModel" /> class.
        /// </summary>
        public SimulatedModel()
            : this(TimeOfDay.NowUtc)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedModel" /> class.
        /// </summary>
        /// <param name="time">
        /// A <see cref="TimeOfDay" /> value. The default value is <see cref="TimeOfDay.NowUtc" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="time" /> is <see langword="null" />.
        /// </exception>
        public SimulatedModel(TimeOfDay time)
            : this(time, Array.Empty<Int32>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedModel" /> class.
        /// </summary>
        /// <param name="time">
        /// A <see cref="TimeOfDay" /> value. The default value is <see cref="TimeOfDay.NowUtc" />.
        /// </param>
        /// <param name="integerCollection">
        /// An <see cref="Int32" /> collection. The default value is an empty collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="time" /> is <see langword="null" /> -or- <paramref name="integerCollection" /> is
        /// <see langword="null" />.
        /// </exception>
        public SimulatedModel(TimeOfDay time, IEnumerable<Int32> integerCollection)
            : this(time, integerCollection, Array.Empty<SimulatedModel>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedModel" /> class.
        /// </summary>
        /// <param name="time">
        /// A <see cref="TimeOfDay" /> value. The default value is <see cref="TimeOfDay.NowUtc" />.
        /// </param>
        /// <param name="integerCollection">
        /// An <see cref="Int32" /> collection. The default value is an empty collection.
        /// </param>
        /// <param name="modelCollection">
        /// A <see cref="SimulatedModel" /> collection. The default value is an empty collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="time" /> is <see langword="null" /> -or- <paramref name="integerCollection" /> is
        /// <see langword="null" /> -or- <paramref name="modelCollection" /> is <see langword="null" />.
        /// </exception>
        public SimulatedModel(TimeOfDay time, IEnumerable<Int32> integerCollection, IEnumerable<SimulatedModel> modelCollection)
            : this(time, integerCollection, modelCollection, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedModel" /> class.
        /// </summary>
        /// <param name="time">
        /// A <see cref="TimeOfDay" /> value. The default value is <see cref="TimeOfDay.NowUtc" />.
        /// </param>
        /// <param name="integerCollection">
        /// An <see cref="Int32" /> collection. The default value is an empty collection.
        /// </param>
        /// <param name="modelCollection">
        /// A <see cref="SimulatedModel" /> collection. The default value is an empty collection.
        /// </param>
        /// <param name="stringValue">
        /// A <see cref="String" /> value. This argument can be <see langword="null" />. The default value is
        /// <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="time" /> is <see langword="null" /> -or- <paramref name="integerCollection" /> is
        /// <see langword="null" /> -or- <paramref name="modelCollection" /> is <see langword="null" />.
        /// </exception>
        public SimulatedModel(TimeOfDay time, IEnumerable<Int32> integerCollection, IEnumerable<SimulatedModel> modelCollection, String stringValue)
            : base()
        {
            IntegerCollection = new List<Int32>(integerCollection.RejectIf().IsNull(nameof(integerCollection)).TargetArgument);
            ModelCollection = new List<SimulatedModel>(modelCollection.RejectIf().IsNull(nameof(modelCollection)).TargetArgument);
            StringValue = stringValue;
            Time = time.RejectIf().IsNull(nameof(time));
        }

        /// <summary>
        /// Generates a randomly-generated <see cref="SimulatedModel" />.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to generate the random model.
        /// </param>
        /// <returns>
        /// A randomly-generated <see cref="SimulatedModel" />.
        /// </returns>
        public static SimulatedModel Random(RandomNumberGenerator randomnessProvider)
        {
            var hour = randomnessProvider.GetInt32(0, 23);
            var minute = randomnessProvider.GetInt32(0, 59);
            var second = randomnessProvider.GetInt32(0, 59);
            var time = new TimeOfDay(TimeZoneInfo.Utc, hour, minute, second);
            var integerCollectionLength = randomnessProvider.GetInt32(0, 2);
            var integerCollection = new Int32[integerCollectionLength];
            var modelCollectionLength = randomnessProvider.GetInt32(0, 2);

            if (modelCollectionLength == 2 && randomnessProvider.GetBoolean())
            {
                // Reduce the likelihood of deep test graphs.
                modelCollectionLength = 1;
            }

            var modelCollection = new SimulatedModel[modelCollectionLength];
            var stringLength = randomnessProvider.GetInt32(3, 5);
            var stringValue = randomnessProvider.GetBoolean() ? randomnessProvider.GetString(stringLength, false, true, true, true, false, false, false) : null;
            randomnessProvider.FillInt32Array(integerCollection);

            for (var i = 0; i < modelCollectionLength; i++)
            {
                modelCollection[i] = Random(randomnessProvider);
            }

            return new SimulatedModel(time, integerCollection, modelCollection, stringValue);
        }

        /// <summary>
        /// Makes a single, random modification to the current <see cref="SimulatedModel" />.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to select and produce the modification.
        /// </param>
        public void RandomlyModify(RandomNumberGenerator randomnessProvider)
        {
            var randomMemberSelector = randomnessProvider.GetUInt16(0, 3);

            switch (randomMemberSelector)
            {
                case 0:

                    RandomlyModifyTime(randomnessProvider);
                    break;

                case 1:

                    RandomlyModifyIntegerCollection(randomnessProvider);
                    break;

                case 2:

                    RandomlyModifyModelCollection(randomnessProvider);
                    break;

                case 3:

                    RandomlyModifyStringValue(randomnessProvider);
                    break;

                default:

                    break;
            }
        }

        /// <summary>
        /// Makes a single, random modification to <see cref="IntegerCollection" />.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to select and produce the modification.
        /// </param>
        private void RandomlyModifyIntegerCollection(RandomNumberGenerator randomnessProvider) => IntegerCollection.Add(randomnessProvider.GetInt32());

        /// <summary>
        /// Makes a single, random modification to <see cref="ModelCollection" />.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to select and produce the modification.
        /// </param>
        private void RandomlyModifyModelCollection(RandomNumberGenerator randomnessProvider)
        {
            var collectionLength = ModelCollection.Count;

            if (collectionLength == 0)
            {
                ModelCollection.Add(Random(randomnessProvider));
            }
            else if (collectionLength == 1)
            {
                ModelCollection.First().RandomlyModify(randomnessProvider);
            }
            else
            {
                ModelCollection.ElementAt(randomnessProvider.GetInt32(0, (collectionLength - 1))).RandomlyModify(randomnessProvider);
            }
        }

        /// <summary>
        /// Makes a single, random modification to <see cref="StringValue" />.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to select and produce the modification.
        /// </param>
        private void RandomlyModifyStringValue(RandomNumberGenerator randomnessProvider)
        {
            var stringLength = randomnessProvider.GetInt32(3, 5);

            if (StringValue is null)
            {
                StringValue = randomnessProvider.GetString(stringLength, false, true, true, true, false, false, false);
                return;
            }

            StringValue = randomnessProvider.GetBoolean() ? randomnessProvider.GetString(stringLength, false, true, true, true, false, false, false) : null;
        }

        /// <summary>
        /// Makes a single, random modification to <see cref="Time" />.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator that is used to select and produce the modification.
        /// </param>
        private void RandomlyModifyTime(RandomNumberGenerator randomnessProvider)
        {
            var hour = randomnessProvider.GetInt32(0, 23);
            var minute = randomnessProvider.GetInt32(0, 59);
            var second = randomnessProvider.GetInt32(0, 59);
            Time = new TimeOfDay(TimeZoneInfo.Utc, hour, minute, second);
        }

        /// <summary>
        /// Gets or sets a <see cref="TimeOfDay" /> value.
        /// </summary>
        [DataMember]
        public TimeOfDay Time
        {
            get;
            set;
        }

        /// <summary>
        /// Represents an <see cref="Int32" /> collection.
        /// </summary>
        [DataMember]
        public readonly List<Int32> IntegerCollection;

        /// <summary>
        /// Represents a <see cref="SimulatedModel" /> collection.
        /// </summary>
        [DataMember]
        public readonly List<SimulatedModel> ModelCollection;

        /// <summary>
        /// Represents a <see cref="String" /> value.
        /// </summary>
        [DataMember]
        public String StringValue;
    }
}