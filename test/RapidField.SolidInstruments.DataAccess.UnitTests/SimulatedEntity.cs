// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents an entity that is used for testing.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the entity's value.
    /// </typeparam>
    internal class SimulatedEntity<TValue> : SimulatedEntity, IEquatable<SimulatedEntity<TValue>>
        where TValue : IEquatable<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The entity's identifier.
        /// </param>
        /// <param name="value">
        /// The entity's value.
        /// </param>
        protected SimulatedEntity(TValue value)
            : this(Guid.NewGuid(), value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The entity's identifier.
        /// </param>
        /// <param name="value">
        /// The entity's value.
        /// </param>
        protected SimulatedEntity(Guid identifier, TValue value)
            : base(identifier, typeof(TValue))
        {
            Value = value;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="SimulatedEntity{TValue}" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="SimulatedEntity{TValue}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="SimulatedEntity{TValue}" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(SimulatedEntity<TValue> a, SimulatedEntity<TValue> b) => a == b == false;

        /// <summary>
        /// Determines whether or not two specified <see cref="SimulatedEntity{TValue}" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="SimulatedEntity{TValue}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="SimulatedEntity{TValue}" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(SimulatedEntity<TValue> a, SimulatedEntity<TValue> b)
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
        /// Creates a copy of the current <see cref="SimulatedEntity{TValue}" />.
        /// </summary>
        /// <returns>
        /// A copy of the current <see cref="SimulatedEntity{TValue}" />.
        /// </returns>
        public sealed override SimulatedEntity Copy() => new SimulatedEntity<TValue>(Identifier, Value);

        /// <summary>
        /// Determines whether or not the current <see cref="SimulatedEntity{TValue}" /> is equal to the specified
        /// <see cref="Object" />.
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
            else if (obj is SimulatedEntity<TValue> entity)
            {
                return Equals(entity);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="SimulatedEntity{TValue}" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SimulatedEntity{TValue}" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(SimulatedEntity<TValue> other)
        {
            if (other is null)
            {
                return false;
            }
            else if (Identifier != other.Identifier)
            {
                return false;
            }
            else if (Value.Equals(other.Value) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => Identifier.GetHashCode() ^ Value.ToString().GetHashCode();

        /// <summary>
        /// Represents the entity's value.
        /// </summary>
        public readonly TValue Value;
    }

    /// <summary>
    /// Represents an entity that is used for testing.
    /// </summary>
    internal abstract class SimulatedEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The entity's identifier.
        /// </param>
        /// <param name="valueType">
        /// The type of the entity's value.
        /// </param>
        protected SimulatedEntity(Guid identifier, Type valueType)
        {
            Identifier = identifier;
            ValueType = valueType;
        }

        /// <summary>
        /// Creates a copy of the current <see cref="SimulatedEntity" />.
        /// </summary>
        /// <returns>
        /// A copy of the current <see cref="SimulatedEntity" />.
        /// </returns>
        public abstract SimulatedEntity Copy();

        /// <summary>
        /// Represents the entity's identifier.
        /// </summary>
        public readonly Guid Identifier;

        /// <summary>
        /// Represents the type of the entity's value.
        /// </summary>
        public readonly Type ValueType;
    }
}