// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that models a general construct.
    /// </summary>
    /// <remarks>
    /// <see cref="Model{TIdentifier}" /> is the default implementation of <see cref="IModel{TIdentifier}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the value that uniquely identifies the model.
    /// </typeparam>
    [DataContract]
    public abstract class Model<TIdentifier> : Model, IModel<TIdentifier>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
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
        public static Boolean operator !=(Model<TIdentifier> a, IModel<TIdentifier> b) => a == b == false;

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
        public static Boolean operator <(Model<TIdentifier> a, IModel<TIdentifier> b) => a is null ? b is Object : a.CompareTo(b) < 0;

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
        public static Boolean operator <=(Model<TIdentifier> a, IModel<TIdentifier> b) => a is null || a.CompareTo(b) <= 0;

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
        public static Boolean operator >(Model<TIdentifier> a, IModel<TIdentifier> b) => a is Object && a.CompareTo(b) > 0;

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
        public static Boolean operator >=(Model<TIdentifier> a, IModel<TIdentifier> b) => a is null ? b is null : a.CompareTo(b) >= 0;

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
        /// Converts the value of the current <see cref="Model{TIdentifier}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Model" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Identifier)}\": \"{Identifier}\" }}";

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
    /// Represents an object that models a general construct.
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
        public static Boolean operator !=(Model a, IModel b) => a == b == false;

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
        /// Creates a new <see cref="Model" /> that is an identical copy of the current <see cref="Model" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="Model" /> that is an identical copy of the current <see cref="Model" />.
        /// </returns>
        public virtual Object Clone() => this.GetSerializedClone();

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
            else if (obj is IModel model)
            {
                return Equals(model);
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
        public override String ToString() => Convert.ToBase64String(GetHashCode().ToByteArray());

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
        internal static void HydrateModel(IModel sourceModel, IModel targetModel)
        {
            var sourceModelType = sourceModel.RejectIf().IsNull(nameof(sourceModel)).TargetArgument.GetType();
            var targetModelType = targetModel.RejectIf().IsNull(nameof(targetModel)).TargetArgument.GetType();
            var sourceModelProperties = sourceModelType.GetProperties(PropertyBindingFlags).ToDictionary(property => property.Name, property => property);
            var targetModelProperties = targetModelType.GetProperties(PropertyBindingFlags).ToDictionary(property => property.Name, property => property);

            foreach (var sourcePropertyElement in sourceModelProperties)
            {
                var propertyName = sourcePropertyElement.Key;
                var sourceProperty = sourcePropertyElement.Value;
                var sourcePropertyValue = sourceProperty.CanRead ? sourceProperty.GetValue(sourceModel) : null;
                var sourcePropertyValueType = sourcePropertyValue?.GetType() ?? sourceProperty.PropertyType;

                if (targetModelProperties.ContainsKey(propertyName))
                {
                    var targetProperty = targetModelProperties[propertyName];
                    var targetPropertyValue = targetProperty.CanRead ? targetProperty.GetValue(targetModel) : null;
                    var targetPropertyValueType = targetPropertyValue?.GetType() ?? targetProperty.PropertyType;

                    if (targetProperty.CanWrite && targetPropertyValueType == sourcePropertyValueType && targetPropertyValue != sourcePropertyValue)
                    {
                        targetProperty.SetValue(targetModel, sourcePropertyValue);
                    }
                    else if (sourcePropertyValue is null)
                    {
                        continue;
                    }
                    else if (ModelInterfaceType.IsAssignableFrom(sourcePropertyValueType) && ModelInterfaceType.IsAssignableFrom(targetPropertyValueType))
                    {
                        if (targetPropertyValue is null)
                        {
                            targetPropertyValue = targetPropertyValueType.GetConstructor(Array.Empty<Type>())?.Invoke(Array.Empty<Object>());
                        }

                        if (sourcePropertyValue is IModel sourcePropertyModel && targetPropertyValue is IModel targetPropertyModel)
                        {
                            HydrateModel(sourcePropertyModel, targetPropertyModel);

                            if (targetProperty.CanWrite)
                            {
                                targetProperty.SetValue(targetModel, targetPropertyModel);
                            }
                        }
                    }
                    else if (CollectionInterfaceType.IsAssignableFrom(sourcePropertyValueType) && CollectionInterfaceType.IsAssignableFrom(targetPropertyValueType))
                    {
                        var sourceCollectionGenericInterfaceType = sourcePropertyValueType.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.Name == CollectionGenericInterfaceType.Name && interfaceType.IsGenericType);
                        var sourceCollectionElementType = sourceCollectionGenericInterfaceType?.GetGenericArguments()[0];
                        var targetCollectionGenericInterfaceType = targetPropertyValueType.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.Name == CollectionGenericInterfaceType.Name && interfaceType.IsGenericType);
                        var targetCollectionElementType = targetCollectionGenericInterfaceType?.GetGenericArguments()[0];

                        if (sourceCollectionElementType is null || targetCollectionElementType is null)
                        {
                            continue;
                        }
                        else if (ModelInterfaceType.IsAssignableFrom(sourceCollectionElementType) && ModelInterfaceType.IsAssignableFrom(targetCollectionElementType))
                        {
                            var sourceCollection = sourcePropertyValue as ICollection<IModel>;
                            var targetCollection = targetPropertyValue as ICollection<IModel> ?? targetPropertyValueType.GetConstructor(Array.Empty<Type>())?.Invoke(Array.Empty<Object>()) as ICollection<IModel>;

                            if (sourceCollection.IsNullOrEmpty() || targetCollection is null)
                            {
                                continue;
                            }

                            foreach (var sourceCollectionElement in sourceCollection)
                            {
                                if (sourceCollectionElement is null)
                                {
                                    continue;
                                }
                                else if (targetCollectionElementType.GetConstructor(Array.Empty<Type>())?.Invoke(Array.Empty<Object>()) is IModel targetCollectionElement)
                                {
                                    HydrateModel(sourceCollectionElement, targetCollectionElement);
                                    targetCollection.Add(targetCollectionElement);
                                }
                            }

                            if (targetProperty.CanWrite)
                            {
                                targetProperty.SetValue(targetModel, targetCollection);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Represents binding flags that are used to find public instance properties of a model.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const BindingFlags PropertyBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// Represents the <see cref="ICollection{T}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CollectionGenericInterfaceType = typeof(ICollection<>);

        /// <summary>
        /// Represents the <see cref="ICollection" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CollectionInterfaceType = typeof(ICollection);

        /// <summary>
        /// Represents the <see cref="IModel" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ModelInterfaceType = typeof(IModel);
    }
}