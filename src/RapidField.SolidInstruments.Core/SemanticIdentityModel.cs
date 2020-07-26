// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that models a general construct and that is identified primarily by a <see cref="String" /> value.
    /// </summary>
    /// <remarks>
    /// <see cref="SemanticIdentityModel" /> is the default implementation of <see cref="ISemanticIdentityModel" />.
    /// </remarks>
    [DataContract]
    public abstract class SemanticIdentityModel : Model<String>, ISemanticIdentityModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticIdentityModel" /> class.
        /// </summary>
        protected SemanticIdentityModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticIdentityModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="String" />.
        /// </param>
        protected SemanticIdentityModel(String identifier)
            : base(identifier)
        {
            return;
        }
    }
}