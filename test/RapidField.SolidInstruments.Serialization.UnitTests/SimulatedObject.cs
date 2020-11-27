// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.TextEncoding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Serialization.UnitTests
{
    /// <summary>
    /// Represents a serializable object that is used for testing.
    /// </summary>
    [DataContract]
    internal sealed class SimulatedObject : IEquatable<SimulatedObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedObject" /> class.
        /// </summary>
        public SimulatedObject()
        {
            DecimalValues = new List<Decimal>();
            NestedObjects = new List<SimulatedObject>();
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="SimulatedObject" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="SimulatedObject" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="SimulatedObject" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(SimulatedObject a, SimulatedObject b) => a == b == false;

        /// <summary>
        /// Determines whether or not two specified <see cref="SimulatedObject" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="SimulatedObject" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="SimulatedObject" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(SimulatedObject a, SimulatedObject b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not the current <see cref="SimulatedObject" /> is equal to the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }
            else if (obj is SimulatedObject simulatedObject)
            {
                return Equals(simulatedObject);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="SimulatedObject" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SimulatedObject" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(SimulatedObject other)
        {
            if (other is null)
            {
                return false;
            }
            else if (BooleanValue != other.BooleanValue)
            {
                return false;
            }
            else if (DateTimeValue != other.DateTimeValue)
            {
                return false;
            }
            else if (TimeSpanValue != other.TimeSpanValue)
            {
                return false;
            }
            else if (StringValue != other.StringValue)
            {
                return false;
            }
            else if (DecimalValues is null && other.DecimalValues is not null)
            {
                return false;
            }
            else if (DecimalValues is not null && other.DecimalValues is null)
            {
                return false;
            }
            else if (NestedObjects is null && other.NestedObjects is not null)
            {
                return false;
            }
            else if (NestedObjects is not null && other.NestedObjects is null)
            {
                return false;
            }

            var decimalValueCount = DecimalValues.Count;

            if (decimalValueCount != other.DecimalValues.Count)
            {
                return false;
            }

            for (var i = 0; i < decimalValueCount; i++)
            {
                if (DecimalValues.ElementAt(i) != other.DecimalValues.ElementAt(i))
                {
                    return false;
                }
            }

            var nestedObjectCount = NestedObjects.Count;

            if (nestedObjectCount != other.NestedObjects.Count)
            {
                return false;
            }

            for (var i = 0; i < nestedObjectCount; i++)
            {
                if (NestedObjects.ElementAt(i) != other.NestedObjects.ElementAt(i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode()
        {
            var hashList = new List<Int32>
            {
                BooleanValue.GetHashCode(),
                DateTimeValue.GetHashCode(),
                StringValue.GetHashCode(),
                TimeSpanValue.GetHashCode()
            };

            foreach (var decimalValue in DecimalValues)
            {
                hashList.Add(decimalValue.GetHashCode());
            }

            foreach (var nestedObject in NestedObjects)
            {
                hashList.Add(nestedObject.GetHashCode());
            }

            var foldedHash = default(Int32);

            foreach (var hash in hashList)
            {
                foldedHash ^= hash;
            }

            return foldedHash;
        }

        /// <summary>
        /// Returns a new random <see cref="SimulatedObject" /> instance.
        /// </summary>
        /// <param name="randomNumberGenerator">
        /// A random number generator that is used to hydrate the object's properties.
        /// </param>
        /// <returns>
        /// A new random <see cref="SimulatedObject" /> instance.
        /// </returns>
        [DebuggerHidden]
        internal static SimulatedObject Random() => Random(HardenedRandomNumberGenerator.Instance);

        /// <summary>
        /// Returns a new random <see cref="SimulatedObject" /> instance.
        /// </summary>
        /// <param name="randomNumberGenerator">
        /// A random number generator that is used to hydrate the object's properties.
        /// </param>
        /// <returns>
        /// A new random <see cref="SimulatedObject" /> instance.
        /// </returns>
        [DebuggerHidden]
        private static SimulatedObject Random(RandomNumberGenerator randomNumberGenerator)
        {
            var randomSimulatedObject = new SimulatedObject
            {
                BooleanValue = randomNumberGenerator.GetBoolean(),
                DateTimeValue = randomNumberGenerator.GetDateTime(),
                EnhancedReadabilityGuidValue = EnhancedReadabilityGuid.New(),
                StringValue = randomNumberGenerator.GetString(0, 32, true, true, true, true, true, true, false),
                TimeSpanValue = randomNumberGenerator.GetTimeSpan()
            };

            var decimalValueCollectionLength = randomNumberGenerator.GetInt32(0, 4);
            var decimalValueCollection = new Decimal[decimalValueCollectionLength];
            randomNumberGenerator.FillDecimalArray(decimalValueCollection);

            foreach (var value in decimalValueCollection)
            {
                randomSimulatedObject.DecimalValues.Add(value);
            }

            var nestedObjectCollectionLength = randomNumberGenerator.GetInt32(0, 2);

            if (nestedObjectCollectionLength > 1 && randomNumberGenerator.GetBoolean())
            {
                nestedObjectCollectionLength = 1;
            }

            for (var i = 0; i < nestedObjectCollectionLength; i++)
            {
                randomSimulatedObject.NestedObjects.Add(Random(randomNumberGenerator));
            }

            return randomSimulatedObject;
        }

        /// <summary>
        /// Gets or sets a <see cref="Boolean" /> value.
        /// </summary>
        [DataMember]
        public Boolean BooleanValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a <see cref="DateTime" /> value.
        /// </summary>
        [DataMember]
        public DateTime DateTimeValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="Decimal" /> values.
        /// </summary>
        [DataMember]
        public ICollection<Decimal> DecimalValues
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an <see cref="EnhancedReadabilityGuid" /> value.
        /// </summary>
        [DataMember]
        public EnhancedReadabilityGuid EnhancedReadabilityGuidValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a collection of nested <see cref="SimulatedObject" />.
        /// </summary>
        [DataMember]
        public ICollection<SimulatedObject> NestedObjects
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a <see cref="String" /> value.
        /// </summary>
        [DataMember]
        public String StringValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a <see cref="TimeSpan" /> value.
        /// </summary>
        [DataMember]
        public TimeSpan TimeSpanValue
        {
            get;
            set;
        }
    }
}