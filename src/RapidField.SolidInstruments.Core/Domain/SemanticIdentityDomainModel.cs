// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core.Domain
{
    /// <summary>
    /// Represents an object that models a general construct and that is identified primarily by a <see cref="String" /> value.
    /// </summary>
    /// <remarks>
    /// <see cref="SemanticIdentityDomainModel" /> is the default implementation of <see cref="ISemanticIdentityDomainModel" />.
    /// </remarks>
    [DataContract]
    public abstract class SemanticIdentityDomainModel : SemanticIdentityModel, ISemanticIdentityDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticIdentityDomainModel" /> class.
        /// </summary>
        protected SemanticIdentityDomainModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticIdentityDomainModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="String" />.
        /// </param>
        protected SemanticIdentityDomainModel(String identifier)
            : base(identifier)
        {
            return;
        }
    }
}