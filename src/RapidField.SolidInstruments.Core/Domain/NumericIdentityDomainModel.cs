// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core.Domain
{
    /// <summary>
    /// Represents an object that models a domain construct and that is identified primarily by a <see cref="Int64" /> value.
    /// </summary>
    /// <remarks>
    /// <see cref="NumericIdentityDomainModel" /> is the default implementation of <see cref="INumericIdentityDomainModel" />.
    /// </remarks>
    [DataContract]
    public abstract class NumericIdentityDomainModel : NumericIdentityModel, INumericIdentityDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericIdentityDomainModel" /> class.
        /// </summary>
        protected NumericIdentityDomainModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericIdentityDomainModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Int64" />.
        /// </param>
        protected NumericIdentityDomainModel(Int64 identifier)
            : base(identifier)
        {
            return;
        }
    }
}