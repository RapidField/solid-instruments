// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command.Configuration
{
    /// <summary>
    /// Represents a command that retrieves and deserializes a configuration value or section.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the configuration object that is produced by interrogating the specified configuration key and target.
    /// </typeparam>
    public interface IGetConfigurationObjectCommand<TResult> : ICommand<TResult>
    {
        /// <summary>
        /// Gets or sets a textual key for the configuration value or section to retrieve.
        /// </summary>
        [DataMember]
        public String Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target type of the current <see cref="IGetConfigurationObjectCommand{TResult}" />.
        /// </summary>
        [DataMember]
        public GetConfigurationObjectCommandTarget Target
        {
            get;
            set;
        }
    }
}