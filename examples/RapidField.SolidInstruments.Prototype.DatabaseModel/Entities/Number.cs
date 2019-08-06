// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Example.Contracts.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.DatabaseModel.Entities
{
    /// <summary>
    /// Represents an integer number.
    /// </summary>
    public sealed class Number : INumber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> class.
        /// </summary>
        [DebuggerHidden]
        internal Number()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> class.
        /// </summary>
        /// <param name="model">
        /// A model that is used to hydrate the new object.
        /// </param>
        [DebuggerHidden]
        internal Number(INumber model)
            : this(model.Identifier, model.Value)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        private Number(Guid identifier, Int64 value)
        {
            Identifier = identifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(identifier));
            Value = value;
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
        /// Gets or sets the value of the number.
        /// </summary>
        [Required]
        public Int64 Value
        {
            get;
            set;
        }
    }
}