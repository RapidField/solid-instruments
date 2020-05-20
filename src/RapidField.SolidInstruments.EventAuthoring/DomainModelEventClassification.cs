// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Specifies a classification that describes the effect of an event upon an <see cref="IDomainModel" />.
    /// </summary>
    [DataContract]
    public enum DomainModelEventClassification : Int32
    {
        /// <summary>
        /// The domain model event classification is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The event relates to the model, but does not otherwise represent a change to its characteristics.
        /// </summary>
        [EnumMember]
        Associated = 1,

        /// <summary>
        /// The event represents creation of the model.
        /// </summary>
        [EnumMember]
        Created = 2,

        /// <summary>
        /// The event represents deletion of the model.
        /// </summary>
        [EnumMember]
        Deleted = 3,

        /// <summary>
        /// The event represents an update to the model.
        /// </summary>
        [EnumMember]
        Updated = 4
    }
}