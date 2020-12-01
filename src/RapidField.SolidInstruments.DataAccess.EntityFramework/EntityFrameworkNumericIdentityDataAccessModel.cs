﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Represents an object that models an Entity Framework entity and that is identified primarily by a <see cref="Int64" />
    /// value.
    /// </summary>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    [DataContract]
    public abstract class EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> : EntityFrameworkDataAccessModel<Int64, TDomainModel>, INumericIdentityDataAccessModel<TDomainModel>
        where TDomainModel : class, INumericIdentityDomainModel, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> class.
        /// </summary>
        protected EntityFrameworkNumericIdentityDataAccessModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Int64" />.
        /// </param>
        protected EntityFrameworkNumericIdentityDataAccessModel(Int64 identifier)
            : base(identifier)
        {
            return;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" />
        /// instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> a, EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> b) => (a == b) is false;

        /// <summary>
        /// Determines whether or not a specified <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" />
        /// instance is less than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> a, EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> b) => a is null ? b is Object : a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" />
        /// instance is less than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> a, EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> b) => a is null || a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" />
        /// instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> a, EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> b)
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
        /// Determines whether or not a specified <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance is greater
        /// than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> a, EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> b) => a is Object && a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance is greater
        /// than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> a, EntityFrameworkNumericIdentityDataAccessModel<TDomainModel> b) => a is null ? b is null : a.CompareTo(b) >= 0;

        /// <summary>
        /// Compares the current <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> to the specified object
        /// and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IModel{TIdentifier}" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(IModel<Int64> other) => Identifier.CompareTo(other.Identifier);

        /// <summary>
        /// Determines whether or not the current <see cref="EntityFrameworkNumericIdentityDataAccessModel{TDomainModel}" /> is
        /// equal to the specified <see cref="Object" />.
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
    }

    /// <summary>
    /// Represents an object that models an Entity Framework entity and that is identified primarily by a <see cref="Int64" />
    /// value.
    /// </summary>
    [DataContract]
    public abstract class EntityFrameworkNumericIdentityDataAccessModel : EntityFrameworkDataAccessModel<Int64>, INumericIdentityDataAccessModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> class.
        /// </summary>
        protected EntityFrameworkNumericIdentityDataAccessModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Int64" />.
        /// </param>
        protected EntityFrameworkNumericIdentityDataAccessModel(Int64 identifier)
            : base(identifier)
        {
            return;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IDataAccessModel" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IDataAccessModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IDataAccessModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(EntityFrameworkNumericIdentityDataAccessModel a, IDataAccessModel b) => (a == b) is false;

        /// <summary>
        /// Determines whether or not a specified <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance is less than
        /// another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(EntityFrameworkNumericIdentityDataAccessModel a, EntityFrameworkNumericIdentityDataAccessModel b) => a is null ? b is Object : a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance is less than
        /// or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(EntityFrameworkNumericIdentityDataAccessModel a, EntityFrameworkNumericIdentityDataAccessModel b) => a is null || a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="IDataAccessModel" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IDataAccessModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IDataAccessModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(EntityFrameworkNumericIdentityDataAccessModel a, IDataAccessModel b)
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
        /// Determines whether or not a specified <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance is greater
        /// than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(EntityFrameworkNumericIdentityDataAccessModel a, EntityFrameworkNumericIdentityDataAccessModel b) => a is Object && a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance is greater
        /// than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(EntityFrameworkNumericIdentityDataAccessModel a, EntityFrameworkNumericIdentityDataAccessModel b) => a is null ? b is null : a.CompareTo(b) >= 0;

        /// <summary>
        /// Compares the current <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> to the specified object and returns an
        /// indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IModel{TIdentifier}" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(IModel<Int64> other) => Identifier.CompareTo(other.Identifier);

        /// <summary>
        /// Determines whether or not the current <see cref="EntityFrameworkNumericIdentityDataAccessModel" /> is equal to the
        /// specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj) => base.Equals(obj);

        /// <summary>
        /// Determines whether or not two specified <see cref="INumericIdentityDataAccessModel" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="INumericIdentityDataAccessModel" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(INumericIdentityDataAccessModel other)
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
        public override Int32 GetHashCode() => base.GetHashCode();
    }
}