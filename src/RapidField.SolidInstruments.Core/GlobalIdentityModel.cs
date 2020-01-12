// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that models a general construct and that is identified primarily by a <see cref="Guid" /> value.
    /// </summary>
    /// <remarks>
    /// <see cref="GlobalIdentityModel" /> is the default implementation of <see cref="IGlobalIdentityModel" />.
    /// </remarks>
    [DataContract]
    public abstract class GlobalIdentityModel : Model<Guid>, IGlobalIdentityModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalIdentityModel" /> class.
        /// </summary>
        protected GlobalIdentityModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalIdentityModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Guid" />.
        /// </param>
        protected GlobalIdentityModel(Guid identifier)
            : base(identifier)
        {
            return;
        }
    }
}