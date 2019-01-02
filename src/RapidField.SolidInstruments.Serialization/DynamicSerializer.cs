// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Performs binary, JSON or XML serialization and deserialization for a given type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the serializable object.
    /// </typeparam>
    public class DynamicSerializer<T> : ISerializer<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializer{T}" /> class.
        /// </summary>
        public DynamicSerializer()
            : this(DefaultFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializer{T}" /> class.
        /// </summary>
        /// <param name="format">
        /// The format to use for serialization and deserialization.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="format" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        public DynamicSerializer(SerializationFormat format)
        {
            Format = format.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(format));
            LazyJsonSerializer = new Lazy<DataContractJsonSerializer>(InitializeJsonSerializer, LazyThreadSafetyMode.PublicationOnly);
            LazyXmlSerializer = new Lazy<DataContractSerializer>(InitializeXmlSerializer, LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Converts the specified binary, JSON or XML buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SerializationException">
        /// <paramref name="buffer" /> is invalid or an error occurred during deserialization.
        /// </exception>
        public T Deserialize(Byte[] buffer)
        {
            switch (Format)
            {
                case SerializationFormat.Binary:

                    return DeserializeFromBinary(buffer.RejectIf().IsNull(nameof(buffer)));

                case SerializationFormat.Json:

                    return DeserializeFromJson(buffer.RejectIf().IsNull(nameof(buffer)));

                case SerializationFormat.Xml:

                    return DeserializeFromXml(buffer.RejectIf().IsNull(nameof(buffer)));

                default:

                    throw new InvalidOperationException($"The specified serialization format, {Format}, is not supported.");
            }
        }

        /// <summary>
        /// Converts the specified object to a serialized binary, JSON or XML buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// The serialized buffer.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        public Byte[] Serialize(T target)
        {
            switch (Format)
            {
                case SerializationFormat.Binary:

                    return SerializeToBinary(target.RejectIf().IsNull(nameof(target)));

                case SerializationFormat.Json:

                    return SerializeToJson(target.RejectIf().IsNull(nameof(target)));

                case SerializationFormat.Xml:

                    return SerializeToXml(target.RejectIf().IsNull(nameof(target)));

                default:

                    throw new InvalidOperationException($"The specified serialization format, {Format}, is not supported.");
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="DynamicSerializer{T}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="DynamicSerializer{T}" />.
        /// </returns>
        public override String ToString() => $"Format: {Format.ToString()}";

        /// <summary>
        /// Converts the specified binary buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="buffer" /> is invalid or an error occurred during deserialization.
        /// </exception>
        protected virtual T DeserializeFromBinary(Byte[] buffer) => DeserializeFromJson(buffer.Decompress());

        /// <summary>
        /// Converts the specified JSON buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="buffer" /> is invalid or an error occurred during deserialization.
        /// </exception>
        protected virtual T DeserializeFromJson(Byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                try
                {
                    return JsonSerializer.ReadObject(stream) as T ?? throw new SerializationException("The specified buffer is invalid.");
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
        /// Converts the specified XML buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="buffer" /> is invalid or an error occurred during deserialization.
        /// </exception>
        protected virtual T DeserializeFromXml(Byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                try
                {
                    return XmlSerializer.ReadObject(stream) as T ?? throw new SerializationException("The specified buffer is invalid.");
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
        /// Converts the specified object to a binary buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// The serialized buffer.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected virtual Byte[] SerializeToBinary(T target) => SerializeToJson(target).Compress();

        /// <summary>
        /// Converts the specified object to a JSON buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// The serialized buffer.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected virtual Byte[] SerializeToJson(T target)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    JsonSerializer.WriteObject(stream, target);
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
        /// Converts the specified object to a XML buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// The serialized buffer.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected virtual Byte[] SerializeToXml(T target)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    XmlSerializer.WriteObject(stream, target);
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
        /// Initializes an XML serializer.
        /// </summary>
        /// <returns>
        /// An XML serializer.
        /// </returns>
        [DebuggerHidden]
        private static DataContractJsonSerializer InitializeJsonSerializer() => new DataContractJsonSerializer(ContractType, JsonSerializerSettings);

        /// <summary>
        /// Initializes an XML serializer.
        /// </summary>
        /// <returns>
        /// An XML serializer.
        /// </returns>
        [DebuggerHidden]
        private static DataContractSerializer InitializeXmlSerializer() => new DataContractSerializer(ContractType, XmlSerializerSettings);

        /// <summary>
        /// Gets the format to use for serialization and deserialization.
        /// </summary>
        public SerializationFormat Format
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a JSON serializer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DataContractJsonSerializer JsonSerializer => LazyJsonSerializer.Value;

        /// <summary>
        /// Gets an XML serializer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DataContractSerializer XmlSerializer => LazyXmlSerializer.Value;

        /// <summary>
        /// Represents the type of the serializable object.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected static readonly Type ContractType = typeof(T);

        /// <summary>
        /// Represents the <see cref="DateTime" /> format string that is used for serialization.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DateTimeSerializationFormatString = "o";

        /// <summary>
        /// Represents the default format to use for serialization and deserialization.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SerializationFormat DefaultFormat = SerializationFormat.Json;

        /// <summary>
        /// Represents settings used by the JSON serializer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly DataContractJsonSerializerSettings JsonSerializerSettings = new DataContractJsonSerializerSettings
        {
            DateTimeFormat = new DateTimeFormat(DateTimeSerializationFormatString),
            EmitTypeInformation = EmitTypeInformation.AsNeeded,
            SerializeReadOnlyTypes = false
        };

        /// <summary>
        /// Represents settings used by the XML serializer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly DataContractSerializerSettings XmlSerializerSettings = new DataContractSerializerSettings
        {
            PreserveObjectReferences = true,
            SerializeReadOnlyTypes = false
        };

        /// <summary>
        /// Represents a lazily-initialized JSON serializer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<DataContractJsonSerializer> LazyJsonSerializer;

        /// <summary>
        /// Represents a lazily-initialized XML serializer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<DataContractSerializer> LazyXmlSerializer;
    }
}