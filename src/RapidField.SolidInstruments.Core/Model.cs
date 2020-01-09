// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a serializable data/domain model.
    /// </summary>
    /// <remarks>
    /// <see cref="Model{TIdentifier}" /> is the default implementation of <see cref="IModel{TIdentifier}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the value that uniquely identifies the model.
    /// </typeparam>
    [DataContract]
    public abstract class Model<TIdentifier> : Model, IModel<TIdentifier>
        where TIdentifier : IComparable<TIdentifier>, IEquatable<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model{TIdentifier}" /> class.
        /// </summary>
        protected Model()
            : this(default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model{TIdentifier}" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of
        /// <typeparamref name="TIdentifier" />.
        /// </param>
        protected Model(TIdentifier identifier)
            : base()
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IModel{TIdentifier}" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Model<TIdentifier> a, IModel<TIdentifier> b) => (a == b) == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="IModel{TIdentifier}" /> instance is less than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Model<TIdentifier> a, IModel<TIdentifier> b) => a.CompareTo(b) == -1;

        /// <summary>
        /// Determines whether or not a specified <see cref="IModel{TIdentifier}" /> instance is less than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Model<TIdentifier> a, IModel<TIdentifier> b) => a.CompareTo(b) < 1;

        /// <summary>
        /// Determines whether or not two specified <see cref="IModel{TIdentifier}" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Model<TIdentifier> a, IModel<TIdentifier> b)
        {
            if ((Object)a is null && (Object)b is null)
            {
                return true;
            }
            else if ((Object)a is null || (Object)b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not a specified <see cref="IModel{TIdentifier}" /> instance is greater than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Model<TIdentifier> a, IModel<TIdentifier> b) => a.CompareTo(b) == 1;

        /// <summary>
        /// Determines whether or not a specified <see cref="IModel{TIdentifier}" /> instance is greater than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Model<TIdentifier> a, IModel<TIdentifier> b) => a.CompareTo(b) > -1;

        /// <summary>
        /// Compares the current <see cref="Model{TIdentifier}" /> to the specified object and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IModel{TIdentifier}" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(IModel<TIdentifier> other) => Identifier.CompareTo(other.Identifier);

        /// <summary>
        /// Determines whether or not the current <see cref="Model{TIdentifier}" /> is equal to the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj) => base.Equals(obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Gets or sets a value that uniquely identifies the current <see cref="Model{TIdentifier}" />.
        /// </summary>
        [DataMember]
        public TIdentifier Identifier
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a serializable data/domain model.
    /// </summary>
    /// <remarks>
    /// <see cref="Model" /> is the default implementation of <see cref="IModel" />.
    /// </remarks>
    [DataContract]
    public abstract class Model : IModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model" /> class.
        /// </summary>
        protected Model()
        {
            return;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IModel" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Model a, IModel b) => (a == b) == false;

        /// <summary>
        /// Determines whether or not two specified <see cref="IModel" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Model a, IModel b)
        {
            if ((Object)a is null && (Object)b is null)
            {
                return true;
            }
            else if ((Object)a is null || (Object)b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not the current <see cref="Model" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is IModel)
            {
                return Equals((IModel)obj);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IModel" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IModel" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(IModel other)
        {
            if ((Object)other is null)
            {
                return false;
            }
            else if (GetHashCode() != other.GetHashCode())
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
        public override Int32 GetHashCode() => this.GetImpliedHashCode();

        /// <summary>
        /// Converts the value of the current <see cref="Model" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Model" />.
        /// </returns>
        public override String ToString() => GetHashCode().ToString();
    }
}