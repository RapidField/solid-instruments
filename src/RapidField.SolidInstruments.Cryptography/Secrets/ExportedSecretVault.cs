// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a serializable collection of <see cref="ISecret" /> objects that were exported from an
    /// <see cref="ISecretVault" />.
    /// </summary>
    /// <remarks>
    /// <see cref="ExportedSecretVault" /> is the default implementation of <see cref="IExportedSecretVault" />.
    /// </remarks>
    [DataContract]
    public sealed class ExportedSecretVault : Model, IExportedSecretVault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportedSecret" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The unique semantic identifier for the associated <see cref="ISecretVault" />.
        /// </param>
        /// <param name="secrets">
        /// The secrets that were exported from the associated <see cref="ISecretVault" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="secrets" /> contains one or more <see langword="null" /> elements.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="identifier" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="identifier" /> is <see langword="null" /> -or- <paramref name="secrets" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal ExportedSecretVault(String identifier, IEnumerable<ExportedSecret> secrets)
            : base()
        {
            Identifier = identifier.RejectIf().IsNullOrEmpty(nameof(identifier));
            Secrets = new(secrets.RejectIf().IsNull(nameof(secrets)).OrIf(collection => collection.Any(secret => secret is null), nameof(secrets), "The specified secret collection contains one or more null elements.").TargetArgument);
        }

        /// <summary>
        /// Returns the collection of secrets that were exported from the associated <see cref="ISecretVault" />.
        /// </summary>
        /// <returns>
        /// The secrets that were exported from the associated <see cref="ISecretVault" />.
        /// </returns>
        public IEnumerable<IExportedSecret> GetExportedSecrets() => Secrets ?? new List<ExportedSecret>();

        /// <summary>
        /// Gets or sets the unique semantic identifier for the associated <see cref="ISecretVault" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// The specified string is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The specified string is <see langword="null" />.
        /// </exception>
        [DataMember]
        public String Identifier
        {
            get => IdentifierReference;
            set => IdentifierReference = value.RejectIf().IsNullOrEmpty(nameof(Identifier));
        }

        /// <summary>
        /// Gets the number of secrets that were exported from the associated <see cref="ISecretVault" />.
        /// </summary>
        [IgnoreDataMember]
        public Int32 SecretCount => Secrets?.Count ?? 0;

        /// <summary>
        /// Gets or sets the secrets that were exported from the associated <see cref="ISecretVault" />.
        /// </summary>
        [DataMember]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<ExportedSecret> Secrets
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the unique semantic identifier for the associated <see cref="ISecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String IdentifierReference;
    }
}