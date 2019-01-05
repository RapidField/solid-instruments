// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Example.Contracts.Models;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.Domain.Models
{
    /// <summary>
    /// Represents a sequential series of integer numbers.
    /// </summary>
    public sealed class NumberSeries : INumberSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberSeries" /> class.
        /// </summary>
        /// <param name="model">
        /// A model that is used to hydrate the new object.
        /// </param>
        [DebuggerHidden]
        internal NumberSeries(INumberSeries model)
            : this(model.Identifier, model.Name)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberSeries" /> class.
        /// </summary>
        /// <param name="name">
        /// The name of the series.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal NumberSeries(String name)
            : this(Guid.NewGuid(), name)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberSeries" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <param name="name">
        /// The name of the series.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        private NumberSeries(Guid identifier, String name)
        {
            Identifier = identifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(identifier));
            Name = name.RejectIf().IsNullOrEmpty(nameof(name));
        }

        /// <summary>
        /// Gets a unique identifier for the entity.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the name of the series.
        /// </summary>
        public String Name
        {
            get;
        }
    }
}