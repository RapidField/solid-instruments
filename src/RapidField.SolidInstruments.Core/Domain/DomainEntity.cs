// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace RapidField.SolidInstruments.Core.Domain
{
    /// <summary>
    /// Represents an object that models a domain entity.
    /// </summary>
    [DataContract]
    public abstract class DomainEntity : Model, IDomainEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEntity" /> class.
        /// </summary>
        protected DomainEntity()
        {
            return;
        }
    }
}