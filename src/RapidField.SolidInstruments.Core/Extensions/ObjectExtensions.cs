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
        /// Derives a hash code from the current object's type name, public field values and public property values.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Object" />.
        /// </param>
        /// <returns>
        /// A hash code that is derived from the current object's type name, public field values and public property values.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="target" /> could not be serialized.
        /// </exception>
        [DebuggerHidden]
        internal static Int32 GetImpliedHashCode(this Object target)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    try
                    {
                        var objectType = target.GetType();
                        var serializer = new DataContractJsonSerializer(objectType, SerializerSettings);
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

                    return stream.ToArray().ComputeThirtyTwoBitHash();
                }
            }
            catch (Exception exception)
            {
                throw new ArgumentException("A hash code cannot be computed for the specified object. See inner exception.", exception);
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
        private static readonly DataContractJsonSerializerSettings SerializerSettings = new DataContractJsonSerializerSettings
        {
            DateTimeFormat = new DateTimeFormat(DateTimeSerializationFormatString),
            EmitTypeInformation = EmitTypeInformation.AsNeeded,
            SerializeReadOnlyTypes = false
        };
    }
}