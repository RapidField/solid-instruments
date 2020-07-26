// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that models a general construct and that is identified primarily by a <see cref="Int64" /> value.
    /// </summary>
    /// <remarks>
    /// <see cref="NumericIdentityModel" /> is the default implementation of <see cref="INumericIdentityModel" />.
    /// </remarks>
    [DataContract]
    public abstract class NumericIdentityModel : Model<Int64>, INumericIdentityModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericIdentityModel" /> class.
        /// </summary>
        protected NumericIdentityModel()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericIdentityModel" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A value that uniquely identifies the model. The default value is equal to the default instance of <see cref="Int64" />.
        /// </param>
        protected NumericIdentityModel(Int64 identifier)
            : base(identifier)
        {
            return;
        }
    }
}