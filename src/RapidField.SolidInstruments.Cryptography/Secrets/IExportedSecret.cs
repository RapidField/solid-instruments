// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.IO;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a serializable <see cref="ISecret" /> that was exported from an <see cref="ISecretManager" />.
    /// </summary>
    public interface IExportedSecret : IModel
    {
        /// <summary>
        /// Converts the current <see cref="IExportedSecret" /> to its <see cref="IReadOnlySecret" /> representation.
        /// </summary>
        /// <returns>
        /// The <see cref="IReadOnlySecret" /> representation of the current <see cref="IExportedSecret" />.
        /// </returns>
        public IReadOnlySecret ToSecret();

        /// <summary>
        /// Gets a value indicating whether or not the secret has a value.
        /// </summary>
        public Boolean HasValue
        {
            get;
        }

        /// <summary>
        /// Gets a textual name that uniquely identifies the secret.
        /// </summary>
        public String Name
        {
            get;
        }

        /// <summary>
        /// Gets the secret value as a Base64 string, or <see langword="null" /> if <see cref="HasValue" /> is
        /// <see langword="false" />.
        /// </summary>
        public String PlaintextValue
        {
            get;
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
        public Type ValueType
        {
            get;
        }

        /// <summary>
        /// Gets the assembly-qualified name of the type of the secret value.
        /// </summary>
        public String ValueTypeAssemblyQualifiedName
        {
            get;
        }
    }
}