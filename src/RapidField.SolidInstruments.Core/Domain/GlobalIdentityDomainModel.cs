// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core.Domain
{
    /// <summary>
    /// Represents an object that models a domain construct and that is identified primarily by a <see cref="Guid" /> value.
    /// </summary>
    /// <remarks>
    /// <see cref="GlobalIdentityDomainModel" /> is the default implementation of <see cref="IGlobalIdentityDomainModel" />.
    /// </remarks>
    [DataContract]
    public abstract class GlobalIdentityDomainModel : GlobalIdentityModel, IGlobalIdentityAggregateDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalIdentityDomainModel" /> class.
        /// </summary>
        protected GlobalIdentityDomainModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalIdentityDomainModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Guid" />.
        /// </param>
        protected GlobalIdentityDomainModel(Guid identifier)
            : base(identifier)
        {
            return;
        }
    }
}