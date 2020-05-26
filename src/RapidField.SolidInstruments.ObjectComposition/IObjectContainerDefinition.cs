// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents information about how an <see cref="ObjectContainer" /> resolves requests for objects.
    /// </summary>
    internal interface IObjectContainerDefinition : IComparable<IObjectContainerDefinition>, IEquatable<IObjectContainerDefinition>
    {
        /// <summary>
        /// Converts the current <see cref="IObjectContainerDefinition" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="IObjectContainerDefinition" />.
        /// </returns>
        public Byte[] ToByteArray();

        /// <summary>
        /// Gets the type that is produced as a result of a request for <see cref="RequestType" />.
        /// </summary>
        public Type ProductType
        {
            get;
        }

        /// <summary>
        /// Gets the request type that identifies the current <see cref="IObjectContainerDefinition" />.
        /// </summary>
        public Type RequestType
        {
            get;
        }
    }
}