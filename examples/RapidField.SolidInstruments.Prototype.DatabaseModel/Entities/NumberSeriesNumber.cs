// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Example.Contracts.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.DatabaseModel.Entities
{
    /// <summary>
    /// Represents an integer number belonging to a specific number series.
    /// </summary>
    public sealed class NumberSeriesNumber : INumberSeriesNumber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberSeriesNumber" /> class.
        /// </summary>
        [DebuggerHidden]
        internal NumberSeriesNumber()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberSeriesNumber" /> class.
        /// </summary>
        /// <param name="model">
        /// A model that is used to hydrate the new object.
        /// </param>
        [DebuggerHidden]
        internal NumberSeriesNumber(INumberSeriesNumber model)
            : this(model.Identifier, model.NumberIdentifier, model.NumberSeriesIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberSeriesNumber" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <param name="numberIdentifier">
        /// A unique identifier for the associated number.
        /// </param>
        /// <param name="numberSeriesIdentifier">
        /// A unique identifier for the associated number series.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="numberIdentifier" /> is equal
        /// to <see cref="Guid.Empty" /> -or- <paramref name="numberSeriesIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        private NumberSeriesNumber(Guid identifier, Guid numberIdentifier, Guid numberSeriesIdentifier)
        {
            Identifier = identifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(identifier));
            NumberIdentifier = numberIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(numberIdentifier));
            NumberSeriesIdentifier = numberSeriesIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(numberSeriesIdentifier));
        }

        /// <summary>
        /// Gets or sets a unique identifier for the entity.
        /// </summary>
        [Key]
        public Guid Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the associated number.
        /// </summary>
        [ForeignKey(nameof(NumberIdentifier))]
        public Number Number
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a unique identifier for the associated number.
        /// </summary>
        [Required]
        public Guid NumberIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the associated number series.
        /// </summary>
        [ForeignKey(nameof(NumberSeriesIdentifier))]
        public NumberSeries NumberSeries
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a unique identifier for the associated number series.
        /// </summary>
        [Required]
        public Guid NumberSeriesIdentifier
        {
            get;
            set;
        }
    }
}