// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.IO;
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
        /// Represents the <see cref="DateTime" /> format string that is used by <see cref="GetImpliedHashCode(Object)" /> to
        /// serialize objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DateTimeSerializationFormatString = "o";

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
    }
}