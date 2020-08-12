// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.DataAccess.EntityFramework;
using System;
using System.Data;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests
{
    /// <summary>
    /// Represents a prototypical, in-memory database transaction.
    /// </summary>
    public sealed class SimulatedTransaction : EntityFrameworkTransaction<SimulatedContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedTransaction" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the transaction.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="context" /> has outstanding changes tracked against it.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public SimulatedTransaction(SimulatedContext context)
            : base(context)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedTransaction" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the transaction.
        /// </param>
        /// <param name="isolationLevel">
        /// The isolation level for the transaction, or <see cref="IsolationLevel.Unspecified" /> to use the database default. The
        /// default value is <see cref="IsolationLevel.Unspecified" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="context" /> has outstanding changes tracked against it.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public SimulatedTransaction(SimulatedContext context, IsolationLevel isolationLevel)
            : base(context, isolationLevel)
        {
            return;
        }
    }
}