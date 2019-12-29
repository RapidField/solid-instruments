// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents information about how an <see cref="ObjectContainer" /> resolves requests for objects.
    /// </summary>
    internal class ObjectContainerDefinition : IComparable<ObjectContainerDefinition>, IEquatable<ObjectContainerDefinition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainerDefinition" /> class.
        /// </summary>
        /// <param name="requestType">
        /// The request type that identifies the definition.
        /// </param>
        /// <param name="productType">
        /// The type that is produced as a result of a request for <paramref name="requestType" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestType" /> is <see langword="null" /> -or- <paramref name="productType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="productType" /> is not a subclass or implementation of <paramref name="requestType" />.
        /// </exception>
        public ObjectContainerDefinition(Type requestType, Type productType)
        {
            RequestType = requestType.RejectIf().IsNull(nameof(requestType)).OrIf().IsNotSupportedType(requestType);
            ProductType = productType.RejectIf().IsNull(nameof(productType));
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="ObjectContainerDefinition" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(ObjectContainerDefinition a, ObjectContainerDefinition b) => (a == b) == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="ObjectContainerDefinition" /> instance is less than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(ObjectContainerDefinition a, ObjectContainerDefinition b) => a.CompareTo(b) == -1;

        /// <summary>
        /// Determines whether or not a specified <see cref="ObjectContainerDefinition" /> instance is less than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(ObjectContainerDefinition a, ObjectContainerDefinition b) => a.CompareTo(b) < 1;

        /// <summary>
        /// Determines whether or not two specified <see cref="ObjectContainerDefinition" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(ObjectContainerDefinition a, ObjectContainerDefinition b)
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
        /// Determines whether or not a specified <see cref="ObjectContainerDefinition" /> instance is greater than another
        /// specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(ObjectContainerDefinition a, ObjectContainerDefinition b) => a.CompareTo(b) == 1;

        /// <summary>
        /// Determines whether or not a specified <see cref="ObjectContainerDefinition" /> instance is greater than or equal to
        /// another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ObjectContainerDefinition" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(ObjectContainerDefinition a, ObjectContainerDefinition b) => a.CompareTo(b) > -1;

        /// <summary>
        /// Compares the current <see cref="ObjectContainerDefinition" /> to the specified object and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="ObjectContainerDefinition" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(ObjectContainerDefinition other)
        {
            var thisInstanceHashCode = GetHashCode();
            var otherInstancHashCode = other.GetHashCode();

            if (thisInstanceHashCode < otherInstancHashCode)
            {
                return -1;
            }
            else if (thisInstanceHashCode > otherInstancHashCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Determines whether or not the current <see cref="ObjectContainerDefinition" /> is equal to the specified
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
            else if (obj is ObjectContainerDefinition)
            {
                return Equals((ObjectContainerDefinition)obj);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="ObjectContainerDefinition" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="ObjectContainerDefinition" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(ObjectContainerDefinition other)
        {
            if ((Object)other is null)
            {
                return false;
            }
            else if (ProductType != other.ProductType)
            {
                return false;
            }
            else if (RequestType != other.RequestType)
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
        public override Int32 GetHashCode() => ToByteArray().ComputeThirtyTwoBitHash();

        /// <summary>
        /// Converts the current <see cref="ObjectContainerDefinition" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="ObjectContainerDefinition" />.
        /// </returns>
        public Byte[] ToByteArray()
        {
            var requestTypeBytes = RequestType.ToByteArray();
            var productTypeBytes = ProductType.ToByteArray();
            var requestTypeByteLength = requestTypeBytes.Length;
            var productTypeByteLength = productTypeBytes.Length;
            var result = new Byte[requestTypeByteLength + productTypeByteLength];
            Array.Copy(requestTypeBytes, 0, result, 0, requestTypeByteLength);
            Array.Copy(productTypeBytes, 0, result, requestTypeByteLength, productTypeByteLength);
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="ObjectContainerDefinition" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="ObjectContainerDefinition" />.
        /// </returns>
        public override String ToString() => $"RequestType: {RequestType.FullName} | ProductType: {ProductType.FullName}";

        /// <summary>
        /// Gets the type that is produced as a result of a request for <see cref="RequestType" />.
        /// </summary>
        public Type ProductType
        {
            get;
        }

        /// <summary>
        /// Gets the request type that identifies the current <see cref="ObjectContainerDefinition" />.
        /// </summary>
        public Type RequestType
        {
            get;
        }
    }
}