﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command that is described by textual information.
    /// </summary>
    /// <remarks>
    /// <see cref="TextualCommand" /> is the default implementation of <see cref="ITextualCommand" />.
    /// </remarks>
    [DataContract]
    public sealed class TextualCommand : Command, ITextualCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextualCommand" /> class.
        /// </summary>
        public TextualCommand()
            : this(String.Empty)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextualCommand" /> class.
        /// </summary>
        /// <param name="value">
        /// The textual command value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        public TextualCommand(String value)
            : this(value, Array.Empty<String>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextualCommand" /> class.
        /// </summary>
        /// <param name="value">
        /// The textual command value.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public TextualCommand(String value, IEnumerable<String> labels)
            : base()
        {
            Labels = new List<String>((labels.RejectIf().IsNull(nameof(labels)).TargetArgument));
            Metadata = new Dictionary<String, String>();
            Value = value.RejectIf().IsNull(nameof(value));
        }

        /// <summary>
        /// Gets or sets a collection of textual labels that provide categorical and/or contextual information about the current
        /// <see cref="TextualCommand" />.
        /// </summary>
        [DataMember]
        public ICollection<String> Labels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a dictionary of metadata for the current <see cref="TextualCommand" />.
        /// </summary>
        [DataMember]
        public IDictionary<String, String> Metadata
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the textual command value.
        /// </summary>
        [DataMember]
        public String Value
        {
            get;
            set;
        }
    }
}