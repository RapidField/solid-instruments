// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command.Configuration
{
    /// <summary>
    /// Represents a command that retrieves and deserializes a configuration value or section.
    /// </summary>
    /// <remarks>
    /// <see cref="GetConfigurationObjectCommand{TResult}" /> is the default implementation of
    /// <see cref="IGetConfigurationObjectCommand{TResult}" />.
    /// </remarks>
    /// <typeparam name="TResult">
    /// The type of the configuration object that is produced by interrogating the specified configuration key and target.
    /// </typeparam>
    [DataContract]
    public abstract class GetConfigurationObjectCommand<TResult> : Command<TResult>, IGetConfigurationObjectCommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfigurationObjectCommand{TResult}" /> class.
        /// </summary>
        /// <param name="key">
        /// A textual key for the configuration value or section to retrieve.
        /// </param>
        /// <param name="target">
        /// The target type of the command.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="key" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="target" /> is equal to <see cref="GetConfigurationObjectCommandTarget.Unspecified" />.
        /// </exception>
        protected GetConfigurationObjectCommand(String key, GetConfigurationObjectCommandTarget target)
            : base()
        {
            Key = key.RejectIf().IsNullOrEmpty(nameof(key));
            Target = target.RejectIf().IsEqualToValue(GetConfigurationObjectCommandTarget.Unspecified, nameof(target));
        }

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
        /// Gets or sets the target type of the current <see cref="GetConfigurationObjectCommand{TResult}" />.
        /// </summary>
        [DataMember]
        public GetConfigurationObjectCommandTarget Target
        {
            get;
            set;
        }
    }
}