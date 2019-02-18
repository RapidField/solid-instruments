// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Example.Contracts.Models;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.Domain.Models
{
    /// <summary>
    /// Represents an integer number.
    /// </summary>
    public sealed class Number : INumber
    {
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
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        internal Number(Int64 value)
            : this(Guid.NewGuid(), value)
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
        /// Gets a unique identifier for the entity.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the value of the number.
        /// </summary>
        public Int64 Value
        {
            get;
        }
    }
}