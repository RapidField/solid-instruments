// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command.Configuration
{
    /// <summary>
    /// Specifies the target type of an <see cref="IGetConfigurationObjectCommand{TResult}" />.
    /// </summary>
    [DataContract]
    public enum GetConfigurationObjectCommandTarget : Int32
    {
        /// <summary>
        /// The target type is not specified.
        /// </summary>
        [EnumMember]
        Unspecified = 0,

        /// <summary>
        /// The associated command targets a connection string with a specified key.
        /// </summary>
        [EnumMember]
        ConnectionString = 1,

        /// <summary>
        /// The associated command targets a configuration section with a specified name.
        /// </summary>
        [EnumMember]
        Section = 2,

        /// <summary>
        /// The associated command targets a configuration value with a specified key.
        /// </summary>
        [EnumMember]
        Value = 3
    }
}