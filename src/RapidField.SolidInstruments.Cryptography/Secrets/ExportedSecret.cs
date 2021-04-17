// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a serializable <see cref="ISecret" /> that was exported from an <see cref="ISecretManager" />.
    /// </summary>
    /// <remarks>
    /// <see cref="ExportedSecret" /> is the default implementation of <see cref="IExportedSecret" />.
    /// </remarks>
    [DataContract]
    public sealed class ExportedSecret : Model, IExportedSecret
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportedSecret" /> class.
        /// </summary>
        /// <param name="secret">
        /// The associated secret.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="secret" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        internal ExportedSecret(IReadOnlySecret secret)
            : base()
        {
            Name = secret.RejectIf().IsNull(nameof(secret)).TargetArgument.Name;
            ValueTypeAssemblyQualifiedName = secret.ValueType.AssemblyQualifiedName;
            secret.Read(secretMemory =>
            {
                Value = new Byte[secretMemory.LengthInBytes];
                secretMemory.ReadOnlySpan.CopyTo(Value);
            });
        }

        /// <summary>
        /// Converts the current <see cref="ExportedSecret" /> to its <see cref="IReadOnlySecret" /> representation.
        /// </summary>
        /// <returns>
        /// The <see cref="IReadOnlySecret" /> representation of the current <see cref="ExportedSecret" />.
        /// </returns>
        public IReadOnlySecret ToSecret()
        {
            if (ValueType == typeof(CascadingSymmetricKey))
            {
                return HydrateSecret(new CascadingSymmetricKeySecret(Name));
            }
            else if (ValueType == typeof(Guid))
            {
                return HydrateSecret(new GuidSecret(Name));
            }
            else if (ValueType == typeof(Double))
            {
                return HydrateSecret(new NumericSecret(Name));
            }
            else if (ValueType == typeof(String))
            {
                return HydrateSecret(new StringSecret(Name));
            }
            else if (ValueType == typeof(SymmetricKey))
            {
                return HydrateSecret(new SymmetricKeySecret(Name));
            }
            else if (ValueType == typeof(X509Certificate2))
            {
                return HydrateSecret(new X509CertificateSecret(Name));
            }

            return HydrateSecret(new Secret(Name));
        }

        /// <summary>
        /// Overwrites the value bytes with zeros, if any, and sets <see cref="Value" /> equal to a <see langword="null" />
        /// reference as an idempotent operation.
        /// </summary>
        [DebuggerHidden]
        internal void ClearValue()
        {
            if (Value is null)
            {
                return;
            }

            var valueLength = Value?.Length ?? 0;

            for (var i = 0; i < valueLength; i++)
            {
                Value[i] = 0x00;
            }

            Value = null;
        }

        /// <summary>
        /// Writes <see cref="Value" /> to the specified secret and returns it.
        /// </summary>
        /// <typeparam name="TValue">
        /// The secret value type.
        /// </typeparam>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <returns>
        /// The specified, hydrated secret.
        /// </returns>
        [DebuggerHidden]
        private IReadOnlySecret<TValue> HydrateSecret<TValue>(Secret<TValue> secret)
        {
            if (HasValue)
            {
                secret?.Write(Value);
            }

            return secret;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the secret has a value.
        /// </summary>
        [DataMember]
        public Boolean HasValue
        {
            get => Value is not null;
            set
            {
                if (value && Value is null)
                {
                    Value = Array.Empty<Byte>();
                    return;
                }

                ClearValue();
            }
        }

        /// <summary>
        /// Gets or sets a textual name that uniquely identifies the secret.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// The specified string is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The specified string is <see langword="null" />.
        /// </exception>
        [DataMember]
        public String Name
        {
            get => NameReference;
            set => NameReference = value.RejectIf().IsNullOrEmpty(nameof(Name));
        }

        /// <summary>
        /// Gets or sets the secret value as a Base64 string, or <see langword="null" /> if <see cref="HasValue" /> is
        /// <see langword="false" />.
        /// </summary>
        /// <exception cref="FormatException">
        /// The specified string is not a valid Base64 string.
        /// </exception>
        [DataMember]
        public String PlaintextValue
        {
            get => HasValue ? Convert.ToBase64String(Value) : null;
            set => Value = value is null ? null : Convert.FromBase64String(value);
        }

        /// <summary>
        /// Gets the type of the secret value, or <see langword="null" /> if <see cref="ValueTypeAssemblyQualifiedName" /> is not a
        /// valid, assembly-qualified type name.
        /// </summary>
        /// <exception cref="BadImageFormatException">
        /// The type assembly or one of its dependencies is invalid.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// The type assembly could not be loaded.
        /// </exception>
        [IgnoreDataMember]
        public Type ValueType => ValueTypeAssemblyQualifiedName.IsNullOrEmpty() ? null : Type.GetType(ValueTypeAssemblyQualifiedName, false, true);

        /// <summary>
        /// Gets or sets the assembly-qualified name of the type of the secret value.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// The specified string is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The specified string is <see langword="null" />.
        /// </exception>
        [DataMember]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public String ValueTypeAssemblyQualifiedName
        {
            [DebuggerHidden]
            get => ValueTypeAssemblyQualifiedNameReference;
            [DebuggerHidden]
            set => ValueTypeAssemblyQualifiedNameReference = value.RejectIf().IsNullOrEmpty(nameof(ValueTypeAssemblyQualifiedName));
        }

        /// <summary>
        /// Represents a textual name that uniquely identifies the secret.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String NameReference;

        /// <summary>
        /// Represents the secret value, or an empty array if <see cref="HasValue" /> is <see langword="null" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Byte[] Value = null;

        /// <summary>
        /// Represents the assembly-qualified name of the type of the secret value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String ValueTypeAssemblyQualifiedNameReference;
    }
}