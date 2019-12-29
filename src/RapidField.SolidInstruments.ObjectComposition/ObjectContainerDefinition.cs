// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents information about how an <see cref="ObjectContainer" /> resolves requests for objects.
    /// </summary>
    internal class ObjectContainerDefinition
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