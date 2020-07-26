// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command to perform an action related to an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelCommand{TModel}" /> is the default implementation of <see cref="IDomainModelCommand{TModel}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    [DataContract]
    public class DomainModelCommand<TModel> : DomainCommand, IDomainModelCommand<TModel>
        where TModel : class, IDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommand{TModel}" /> class.
        /// </summary>
        public DomainModelCommand()
            : base()
        {
            Model = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommand{TModel}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainModelCommand(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            Model = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommand{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The desired state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelCommandClassification.Unspecified" />.
        /// </exception>
        public DomainModelCommand(TModel model, DomainModelCommandClassification classification)
            : this(model, classification, Array.Empty<String>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommand{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The desired state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelCommandClassification.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainModelCommand(TModel model, DomainModelCommandClassification classification, Guid correlationIdentifier)
            : this(model, classification, Array.Empty<String>(), correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommand{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The desired state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelCommandClassification.Unspecified" />.
        /// </exception>
        public DomainModelCommand(TModel model, DomainModelCommandClassification classification, IEnumerable<String> labels)
            : base(labels)
        {
            Classification = classification.RejectIf().IsEqualToValue(DomainModelCommandClassification.Unspecified, nameof(classification));
            Model = model.RejectIf().IsNull(nameof(model));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommand{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The desired state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelCommandClassification.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainModelCommand(TModel model, DomainModelCommandClassification classification, IEnumerable<String> labels, Guid correlationIdentifier)
            : base(labels, correlationIdentifier)
        {
            Classification = classification.RejectIf().IsEqualToValue(DomainModelCommandClassification.Unspecified, nameof(classification));
            Model = model.RejectIf().IsNull(nameof(model));
        }

        /// <summary>
        /// Gets or sets a classification that describes the effect of a the current <see cref="DomainModelCommand{TModel}" /> upon
        /// <see cref="Model" />.
        /// </summary>
        [DataMember]
        public DomainModelCommandClassification Classification
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the desired state of the associated domain model.
        /// </summary>
        [DataMember]
        public TModel Model
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the type of the associated domain model.
        /// </summary>
        [IgnoreDataMember]
        public Type ModelType => typeof(TModel);
    }
}