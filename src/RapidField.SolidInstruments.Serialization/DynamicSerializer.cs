// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace RapidField.SolidInstruments.Serialization
{
    /// <summary>
    /// Performs serialization and deserialization for a given type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the serializable object.
    /// </typeparam>
    public class DynamicSerializer<T> : DynamicSerializer, ISerializer<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializer{T}" /> class.
        /// </summary>
        public DynamicSerializer()
            : base()
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
            : base(format)
        {
            LazyJsonSerializer = new Lazy<DataContractJsonSerializer>(InitializeJsonSerializer, LazyThreadSafetyMode.PublicationOnly);
            LazyXmlSerializer = new Lazy<DataContractSerializer>(InitializeXmlSerializer, LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Converts the specified buffer to its typed equivalent.
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
        public T Deserialize(Byte[] buffer) => Deserialize(buffer.RejectIf().IsNull(nameof(buffer)), Format);

        /// <summary>
        /// Converts the specified object to a serialized buffer.
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
        public Byte[] Serialize(T target) => Serialize(target.RejectIf().IsNull(nameof(target)), Format);

        /// <summary>
        /// Converts the specified buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <param name="format">
        /// The format to use for deserialization.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="buffer" /> is invalid or an error occurred during deserialization.
        /// </exception>
        protected virtual T Deserialize(Byte[] buffer, SerializationFormat format) => format switch
        {
            SerializationFormat.Binary => Deserialize(buffer, DefaultFormat),
            SerializationFormat.CompressedJson => DeserializeFromJson(buffer.Decompress()),
            SerializationFormat.CompressedXml => DeserializeFromXml(buffer.Decompress()),
            SerializationFormat.Json => DeserializeFromJson(buffer),
            SerializationFormat.Xml => DeserializeFromXml(buffer),
            _ => throw new UnsupportedSpecificationException($"The specified serialization format, {Format}, is not supported.")
        };

        /// <summary>
        /// Converts the specified object to a buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <param name="format">
        /// The format to use for serialization.
        /// </param>
        /// <returns>
        /// The serialized buffer.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected virtual Byte[] Serialize(T target, SerializationFormat format) => format switch
        {
            SerializationFormat.Binary => Serialize(target, DefaultFormat),
            SerializationFormat.CompressedJson => SerializeToJson(target).Compress(),
            SerializationFormat.CompressedXml => SerializeToXml(target).Compress(),
            SerializationFormat.Json => SerializeToJson(target),
            SerializationFormat.Xml => SerializeToXml(target),
            _ => throw new UnsupportedSpecificationException($"The specified serialization format, {format}, is not supported.")
        };

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
        [DebuggerHidden]
        private T DeserializeFromJson(Byte[] buffer)
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
        [DebuggerHidden]
        private T DeserializeFromXml(Byte[] buffer)
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
        [DebuggerHidden]
        private Byte[] SerializeToJson(T target)
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
        /// Converts the specified object to an XML buffer.
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
        [DebuggerHidden]
        private Byte[] SerializeToXml(T target)
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
            PreserveObjectReferences = false,
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

    /// <summary>
    /// Performs serialization and deserialization for a given type.
    /// </summary>
    public abstract class DynamicSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializer" /> class.
        /// </summary>
        protected DynamicSerializer()
            : this(DefaultFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializer" /> class.
        /// </summary>
        /// <param name="format">
        /// The format to use for serialization and deserialization.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="format" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        protected DynamicSerializer(SerializationFormat format)
        {
            Format = format.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(format));
        }

        /// <summary>
        /// Converts the value of the current <see cref="DynamicSerializer" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="DynamicSerializer" />.
        /// </returns>
        public override String ToString() => $"Format: {Format}";

        /// <summary>
        /// Converts the specified buffer to its typed equivalent.
        /// </summary>
        /// <param name="buffer">
        /// A serialized object.
        /// </param>
        /// <param name="targetType">
        /// The type of the object to be deserialized.
        /// </param>
        /// <param name="format">
        /// The format to use for deserialization.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="buffer" /> is invalid or an error occurred during deserialization.
        /// </exception>
        [DebuggerHidden]
        internal static Object Deserialize(Byte[] buffer, Type targetType, SerializationFormat format)
        {
            try
            {
                var serializer = Create(targetType.RejectIf().IsNull(nameof(targetType)), format.RejectIf().IsEqualToValue(SerializationFormat.Unspecified));
                var deserializeMethod = serializer.GetType().GetMethod(nameof(ISerializer<Object>.Deserialize));
                return deserializeMethod.Invoke(serializer, new Object[] { buffer.RejectIf().IsNull(nameof(buffer)).TargetArgument });
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

        /// <summary>
        /// Converts the specified object to a buffer.
        /// </summary>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <param name="targetType">
        /// The type of the object to be serialized.
        /// </param>
        /// <param name="format">
        /// The format to use for serialization.
        /// </param>
        /// <returns>
        /// The serialized buffer.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        [DebuggerHidden]
        internal static Byte[] Serialize(Object target, Type targetType, SerializationFormat format)
        {
            try
            {
                var serializer = Create(targetType.RejectIf().IsNull(nameof(targetType)), format.RejectIf().IsEqualToValue(SerializationFormat.Unspecified));
                var serializeMethod = serializer.GetType().GetMethod(nameof(ISerializer<Object>.Serialize));
                return serializeMethod.Invoke(serializer, new Object[] { target.RejectIf().IsNull(nameof(target)).TargetArgument }) as Byte[];
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during serialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Converts the specified object to an encoded JSON string.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to be serialized.
        /// </typeparam>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// An encoded JSON string.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        [DebuggerHidden]
        internal static String SerializeToJsonString<T>(T target)
            where T : class => Encoding.UTF8.GetString(CreateGeneric<T>(SerializationFormat.Json).Serialize(target));

        /// <summary>
        /// Converts the specified object to an encoded XML string.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to be serialized.
        /// </typeparam>
        /// <param name="target">
        /// An object to be serialized.
        /// </param>
        /// <returns>
        /// An encoded XML string.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="target" /> is invalid or an error occurred during serialization.
        /// </exception>
        [DebuggerHidden]
        internal static String SerializeToXmlString<T>(T target)
            where T : class => Encoding.UTF8.GetString(CreateGeneric<T>(SerializationFormat.Xml).Serialize(target));

        /// <summary>
        /// Creates a new serializer for the specified contract type and format.
        /// </summary>
        /// <param name="targetType">
        /// The type of the object to be serialized.
        /// </param>
        /// <param name="format">
        /// The format to use for serialization and deserialization.
        /// </param>
        /// <returns>
        /// A new serializer for the specified contract type and format.
        /// </returns>
        [DebuggerHidden]
        private static Object Create(Type targetType, SerializationFormat format)
        {
            var createMethod = typeof(DynamicSerializer).GetMethod(nameof(CreateGeneric), BindingFlags.NonPublic | BindingFlags.Static);
            var genericCreateMethod = createMethod.MakeGenericMethod(targetType);
            return genericCreateMethod.Invoke(null, new Object[] { format });
        }

        /// <summary>
        /// Creates a new serializer for the specified contract type and format.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the serializable object.
        /// </typeparam>
        /// <param name="format">
        /// The format to use for serialization and deserialization.
        /// </param>
        /// <returns>
        /// A new serializer for the specified contract type and format.
        /// </returns>
        [DebuggerHidden]
        private static ISerializer<T> CreateGeneric<T>(SerializationFormat format)
            where T : class => format switch
            {
                SerializationFormat.Binary => new Serializer<T>(),
                SerializationFormat.CompressedJson => new CompressedJsonSerializer<T>(),
                SerializationFormat.CompressedXml => new CompressedXmlSerializer<T>(),
                SerializationFormat.Json => new JsonSerializer<T>(),
                SerializationFormat.Xml => new XmlSerializer<T>(),
                _ => throw new UnsupportedSpecificationException($"The specified serialization format, {format}, is not supported.")
            };

        /// <summary>
        /// Gets the format to use for serialization and deserialization.
        /// </summary>
        public SerializationFormat Format
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents the default format to use for serialization and deserialization.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const SerializationFormat DefaultFormat = SerializationFormat.CompressedJson;
    }
}