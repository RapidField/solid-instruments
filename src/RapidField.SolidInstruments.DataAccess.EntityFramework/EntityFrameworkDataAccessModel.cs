// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Represents an object that models an Entity Framework entity.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the value that uniquely identifies the model.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    [DataContract]
    public abstract class EntityFrameworkDataAccessModel<TIdentifier, TDomainModel> : EntityFrameworkDataAccessModel<TIdentifier>, IDataAccessModel<TIdentifier, TDomainModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDataAccessModel{TIdentifier, TDomainModel}" /> class.
        /// </summary>
        protected EntityFrameworkDataAccessModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDataAccessModel{TIdentifier, TDomainModel}" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of
        /// <typeparamref name="TIdentifier" />.
        /// </param>
        protected EntityFrameworkDataAccessModel(TIdentifier identifier)
            : base(identifier)
        {
            return;
        }

        /// <summary>
        /// Copies the state of the specified domain model to the current
        /// <see cref="EntityFrameworkDataAccessModel{TIdentifier, TDomainModel}" />.
        /// </summary>
        /// <param name="domainModel">
        /// A domain model from which to hydrate the state of the data access model.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The state of <paramref name="domainModel" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainModel" /> is <see langword="null" />.
        /// </exception>
        public void HydrateFromDomainModel(TDomainModel domainModel)
        {
            _ = domainModel.RejectIf().IsNull(nameof(domainModel));

            try
            {
                HydrateModel(domainModel, this);
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"Failed to hydrate a data access model (type: {GetType().FullName}) from the specified domain model (type: {typeof(TDomainModel).FullName}). See inner exception.", nameof(domainModel), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="EntityFrameworkDataAccessModel{TIdentifier, TDomainModel}" /> to its equivalent domain
        /// model representation.
        /// </summary>
        /// <returns>
        /// An <see cref="IDomainModel{TIdentifier}" /> that is equivalent to the current data access model.
        /// </returns>
        /// <exception cref="TypeInitializationException">
        /// An exception was raised while converting the data access model to its equivalent domain model.
        /// </exception>
        public TDomainModel ToDomainModel()
        {
            try
            {
                var domainModel = new TDomainModel();
                HydrateModel(this, domainModel);
                return domainModel;
            }
            catch (Exception exception)
            {
                throw new TypeInitializationException(typeof(TDomainModel).FullName, exception);
            }
        }

        /// <summary>
        /// Copies the state of the specified source model to the specified target model.
        /// </summary>
        /// <param name="sourceModel">
        /// The model from which state is derived.
        /// </param>
        /// <param name="targetModel">
        /// The model to which state is copied.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceModel" /> is <see langword="null" /> -or- <paramref name="targetModel" /> is
        /// <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static void HydrateModel(IModel sourceModel, IModel targetModel)
        {
            var sourceModelType = sourceModel.RejectIf().IsNull(nameof(sourceModel)).TargetArgument.GetType();
            var targetModelType = targetModel.RejectIf().IsNull(nameof(targetModel)).TargetArgument.GetType();
            var sourceModelProperties = sourceModelType.GetProperties(PropertyBindingFlags).ToDictionary(property => property.Name, property => property);
            var targetModelProperties = targetModelType.GetProperties(PropertyBindingFlags).ToDictionary(property => property.Name, property => property);

            foreach (var sourcePropertyElement in sourceModelProperties)
            {
                var propertyName = sourcePropertyElement.Key;
                var sourcePropertyValue = sourcePropertyElement.Value.CanRead ? sourcePropertyElement.Value.GetValue(sourceModel) : null;

                if (targetModelProperties.ContainsKey(propertyName) && (sourcePropertyValue is null) == false)
                {
                    var targetProperty = targetModelProperties[propertyName];
                    var sourcePropertyValueType = sourcePropertyValue.GetType();

                    if (targetProperty.CanWrite && targetProperty.PropertyType == sourcePropertyValueType)
                    {
                        targetProperty.SetValue(targetModel, sourcePropertyValue);
                    }
                }
            }
        }

        /// <summary>
        /// Represents binding flags that are used to find public instance properties of a model.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const BindingFlags PropertyBindingFlags = BindingFlags.Instance | BindingFlags.Public;
    }

    /// <summary>
    /// Represents an object that models an Entity Framework entity.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the value that uniquely identifies the model.
    /// </typeparam>
    [DataContract]
    public abstract class EntityFrameworkDataAccessModel<TIdentifier> : EntityFrameworkDataAccessModel, IDataAccessModel<TIdentifier>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDataAccessModel{TIdentifier}" /> class.
        /// </summary>
        protected EntityFrameworkDataAccessModel()
            : this(default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDataAccessModel{TIdentifier}" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of
        /// <typeparamref name="TIdentifier" />.
        /// </param>
        protected EntityFrameworkDataAccessModel(TIdentifier identifier)
            : base()
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IDataAccessModel{TIdentifier}" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(EntityFrameworkDataAccessModel<TIdentifier> a, IDataAccessModel<TIdentifier> b) => a == b == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="IDataAccessModel{TIdentifier}" /> instance is less than another
        /// specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(EntityFrameworkDataAccessModel<TIdentifier> a, IDataAccessModel<TIdentifier> b) => a is null ? b is Object : a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="IDataAccessModel{TIdentifier}" /> instance is less than or equal to
        /// another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(EntityFrameworkDataAccessModel<TIdentifier> a, IDataAccessModel<TIdentifier> b) => a is null || a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="IDataAccessModel{TIdentifier}" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(EntityFrameworkDataAccessModel<TIdentifier> a, IDataAccessModel<TIdentifier> b)
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
        /// Determines whether or not a specified <see cref="IDataAccessModel{TIdentifier}" /> instance is greater than another
        /// specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(EntityFrameworkDataAccessModel<TIdentifier> a, IDataAccessModel<TIdentifier> b) => a is Object && a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="IDataAccessModel{TIdentifier}" /> instance is greater than or equal to
        /// another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IDataAccessModel{TIdentifier}" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(EntityFrameworkDataAccessModel<TIdentifier> a, IDataAccessModel<TIdentifier> b) => a is null ? b is null : a.CompareTo(b) >= 0;

        /// <summary>
        /// Compares the current <see cref="EntityFrameworkDataAccessModel{TIdentifier}" /> to the specified object and returns an
        /// indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IDataAccessModel{TIdentifier}" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(IDataAccessModel<TIdentifier> other) => Identifier.CompareTo(other.Identifier);

        /// <summary>
        /// Determines whether or not the current <see cref="EntityFrameworkDataAccessModel{TIdentifier}" /> is equal to the
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
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Converts the value of the current <see cref="EntityFrameworkDataAccessModel{TIdentifier}" /> to its equivalent string
        /// representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="EntityFrameworkDataAccessModel{TIdentifier}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Identifier)}\": \"{Identifier}\" }}";

        /// <summary>
        /// Gets or sets a value that uniquely identifies the current <see cref="EntityFrameworkDataAccessModel{TIdentifier}" />.
        /// </summary>
        [Column(Order = 0)]
        [DataMember]
        [Key]
        public TIdentifier Identifier
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents an object that models an Entity Framework entity.
    /// </summary>
    [DataContract]
    public abstract class EntityFrameworkDataAccessModel : Model, IDataAccessModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDataAccessModel" /> class.
        /// </summary>
        protected EntityFrameworkDataAccessModel()
            : base()
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
        public static Boolean operator !=(EntityFrameworkDataAccessModel a, IDataAccessModel b) => a == b == false;

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
        public static Boolean operator ==(EntityFrameworkDataAccessModel a, IDataAccessModel b)
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
        /// Determines whether or not the current <see cref="EntityFrameworkDataAccessModel" /> is equal to the specified
        /// <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj) => base.Equals(obj);

        /// <summary>
        /// Determines whether or not two specified <see cref="IDataAccessModel" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IDataAccessModel" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(IDataAccessModel other)
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