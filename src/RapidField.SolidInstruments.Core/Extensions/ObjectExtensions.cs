// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Object" /> class with general purpose features.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Reflectively interrogates the current object to determine the total number of bytes that it occupies in memory.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Object" />.
        /// </param>
        /// <returns>
        /// The total size of the current object, in bytes.
        /// </returns>
        public static Int32 CalculateSizeInBytes(this Object target)
        {
            if (target?.GetType().IsValueType ?? false)
            {
                return Marshal.SizeOf(target);
            }

            var pointerSizeInBytes = IntPtr.Size;
            var targetSizeInBytes = pointerSizeInBytes;

            if (target is not null)
            {
                var values = new List<Object>();

                {
                    var collections = new List<IEnumerable>();
                    var references = new List<Object>();
                    target.FlattenObjectGraph(collections, references, values);
                    targetSizeInBytes += (collections.Count + references.Count) * pointerSizeInBytes;
                }

                foreach (var value in values)
                {
                    targetSizeInBytes += Marshal.SizeOf(value);
                }
            }

            return targetSizeInBytes;
        }

        /// <summary>
        /// Derives a hash code from the current object's type name and serialized representation.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Object" />.
        /// </param>
        /// <returns>
        /// A hash code that is derived from the current object's type name, public field values and public property values.
        /// </returns>
        /// <exception cref="UnsupportedSpecificationException">
        /// The object could not be serialized.
        /// </exception>
        [DebuggerHidden]
        internal static Int32 GetImpliedHashCode(this Object target)
        {
            var objectType = target.GetType();
            var typeFullName = objectType.FullName;
            var hashCode = typeFullName.GetHashCode();

            try
            {
                var serializer = new DataContractJsonSerializer(objectType, GetImpliedHashCodeSerializerSettings);
                var serializedObject = Serialize(serializer, target);
                hashCode ^= serializedObject.ComputeThirtyTwoBitHash();
            }
            catch (Exception exception)
            {
                throw new UnsupportedSpecificationException($"A hash code cannot be computed for the specified object of type {typeFullName}. Object serialization failed. This can happen when an equality comparer attempts to calculate a hash code for a type that is not configured for serialization. See inner exception.", exception);
            }

            return hashCode;
        }

        /// <summary>
        /// Produces a clone of the current object using serialization and deserialization.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Object" />.
        /// </param>
        /// <returns>
        /// A clone of the current object.
        /// </returns>
        /// <exception cref="UnsupportedSpecificationException">
        /// The object could not be serialized.
        /// </exception>
        [DebuggerHidden]
        internal static Object GetSerializedClone(this Object target)
        {
            var objectType = target.GetType();
            var typeFullName = objectType.FullName;

            try
            {
                var serializer = new DataContractJsonSerializer(objectType, GetSerializedCloneSerializerSettings);
                var serializedObject = Serialize(serializer, target);
                return Deserialize(serializer, serializedObject);
            }
            catch (Exception exception)
            {
                throw new UnsupportedSpecificationException($"A clone could not be produced for the specified object of type {typeFullName}. Object serialization failed. This can happen when attempting to clone a type that is not configured for serialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Converts the specified serialized object to its typed equivalent.
        /// </summary>
        /// <param name="serializer">
        /// A serializer that deserializes <paramref name="serializedObject" />.
        /// </param>
        /// <param name="serializedObject">
        /// A serialized object.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="serializedObject" /> is invalid or an error occurred during deserialization.
        /// </exception>
        [DebuggerHidden]
        private static Object Deserialize(DataContractJsonSerializer serializer, Byte[] serializedObject)
        {
            using (var stream = new MemoryStream(serializedObject))
            {
                try
                {
                    return serializer.ReadObject(stream) ?? throw new SerializationException("The specified serialized object is invalid.");
                }
                catch (SerializationException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new SerializationException("An error occurred during deserialization. See inner exception.", exception);
                }
            }
        }

        /// <summary>
        /// Recursively interrogates the specified collection object's graph and hydrates the specified collections with unique
        /// instances of its child objects.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Object" />.
        /// </param>
        /// <param name="collections">
        /// A flattened collection of collection object references to which new, unique instances are added.
        /// </param>
        /// <param name="references">
        /// A flattened collection of non-collection object references to which new, unique instances are added.
        /// </param>
        /// <param name="values">
        /// A collection of values to which new instances are added.
        /// </param>
        [DebuggerHidden]
        private static void FlattenCollectionGraph(this Object target, IList<IEnumerable> collections, IList<Object> references, IList<Object> values)
        {
            if (target is IEnumerable collection && collections.Contains(collection) is false)
            {
                collections.Add(collection);

                foreach (var element in collection)
                {
                    element?.FlattenObjectGraph(collections, references, values);
                }
            }
        }

        /// <summary>
        /// Recursively interrogates the specified object's graph and hydrates the specified collections with unique instances of
        /// its child objects.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Object" />.
        /// </param>
        /// <param name="collections">
        /// A flattened collection of collection object references to which new, unique instances are added.
        /// </param>
        /// <param name="references">
        /// A flattened collection of non-collection object references to which new, unique instances are added.
        /// </param>
        /// <param name="values">
        /// A collection of values to which new instances are added.
        /// </param>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FlattenObjectGraph(this Object target, IList<IEnumerable> collections, IList<Object> references, IList<Object> values)
        {
            var targetType = target.GetType();
            var targetTypeCategory = targetType.GetTypeCategory();

            switch (targetTypeCategory)
            {
                case ObjectTypeCategory.Collection:

                    target.FlattenCollectionGraph(collections, references, values);
                    break;

                case ObjectTypeCategory.Reference:

                    target.FlattenReferenceGraph(targetType, collections, references, values);
                    break;

                case ObjectTypeCategory.Value:

                    values.Add(target);
                    break;

                default:

                    throw new UnsupportedSpecificationException($"The specified object type category, {targetTypeCategory}, is not supported.");
            }
        }

        /// <summary>
        /// Recursively interrogates the specified reference object's graph and hydrates the specified collections with unique
        /// instances of its child objects.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Object" />.
        /// </param>
        /// <param name="targetType">
        /// The type of <paramref name="target" />.
        /// </param>
        /// <param name="collections">
        /// A flattened collection of collection object references to which new, unique instances are added.
        /// </param>
        /// <param name="references">
        /// A flattened collection of non-collection object references to which new, unique instances are added.
        /// </param>
        /// <param name="values">
        /// A collection of values to which new instances are added.
        /// </param>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FlattenReferenceGraph(this Object target, Type targetType, IList<IEnumerable> collections, IList<Object> references, IList<Object> values)
        {
            if (references.Contains(target) is false)
            {
                references.Add(target);

                foreach (var field in targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    field.GetValue(target)?.FlattenObjectGraph(collections, references, values);
                }

                foreach (var property in targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (property.CanRead)
                    {
                        property.GetValue(target)?.FlattenObjectGraph(collections, references, values);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether an object type is a collection type, non-collection reference type or value type.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Type" />.
        /// </param>
        /// <returns>
        /// The resulting type category.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ObjectTypeCategory GetTypeCategory(this Type target) => target.IsValueType ? ObjectTypeCategory.Value : (CollectionInterfaceType.IsAssignableFrom(target) ? ObjectTypeCategory.Collection : ObjectTypeCategory.Reference);

        /// <summary>
        /// Converts the specified object to a serialized byte array.
        /// </summary>
        /// <param name="serializer">
        /// A serializer that serializes <paramref name="target" />.
        /// </param>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// The serialized byte array.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        [DebuggerHidden]
        private static Byte[] Serialize(DataContractJsonSerializer serializer, Object target)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    serializer.WriteObject(stream, target);
                }
                catch (SerializationException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new SerializationException("An error occurred during serialization. See inner exception.", exception);
                }

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Represents the <see cref="DateTime" /> format string that is used by <see cref="GetImpliedHashCode(Object)" /> and
        /// <see cref="GetSerializedClone(Object)" /> to serialize objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DateTimeSerializationFormatString = "o";

        /// <summary>
        /// Represents the <see cref="IEnumerable" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CollectionInterfaceType = typeof(IEnumerable);

        /// <summary>
        /// Represents settings used by <see cref="GetImpliedHashCode(Object)" /> to serialize objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly DataContractJsonSerializerSettings GetImpliedHashCodeSerializerSettings = new DataContractJsonSerializerSettings
        {
            DateTimeFormat = new DateTimeFormat(DateTimeSerializationFormatString),
            EmitTypeInformation = EmitTypeInformation.Always,
            SerializeReadOnlyTypes = true
        };

        /// <summary>
        /// Represents settings used by <see cref="GetSerializedClone(Object)" /> to serialize objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly DataContractJsonSerializerSettings GetSerializedCloneSerializerSettings = new DataContractJsonSerializerSettings
        {
            DateTimeFormat = new DateTimeFormat(DateTimeSerializationFormatString),
            EmitTypeInformation = EmitTypeInformation.AsNeeded,
            SerializeReadOnlyTypes = false
        };

        /// <summary>
        /// Specifies whether an object is a collection type, non-collection reference type or value type.
        /// </summary>
        private enum ObjectTypeCategory : Byte
        {
            /// <summary>
            /// The object type category is not specified.
            /// </summary>
            Unspecified = 0,

            /// <summary>
            /// The object type is a collection type.
            /// </summary>
            Collection = 1,

            /// <summary>
            /// The object type is a non-collection reference type.
            /// </summary>
            Reference = 2,

            /// <summary>
            /// The object type is a value type.
            /// </summary>
            Value = 3
        }
    }
}