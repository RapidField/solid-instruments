// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents a <see cref="DataAccessTransaction" /> derivative that is used for testing.
    /// </summary>
    internal abstract class SimulatedTransaction : DataAccessTransaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedTransaction" /> class.
        /// </summary>
        protected SimulatedTransaction()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initiates the current <see cref="SimulatedTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Begin(IConcurrencyControlToken controlToken)
        {
            return;
        }

        /// <summary>
        /// Asynchronously initiates the current <see cref="SimulatedTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected override Task BeginAsync(IConcurrencyControlToken controlToken) => Task.CompletedTask;

        /// <summary>
        /// Commits all changes made within the scope of the current <see cref="SimulatedTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Commit(IConcurrencyControlToken controlToken)
        {
            return;
        }

        /// <summary>
        /// Asynchronously commits all changes made within the scope of the current <see cref="SimulatedTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected override Task CommitAsync(IConcurrencyControlToken controlToken) => Task.CompletedTask;

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DataAccessTransaction" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Rejects all changes made within the scope of the current <see cref="SimulatedTransaction" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Reject(IConcurrencyControlToken controlToken)
        {
            return;
        }

        /// <summary>
        /// Asynchronously all changes made within the scope of the current <see cref="SimulatedTransaction" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override Task RejectAsync(IConcurrencyControlToken controlToken) => Task.CompletedTask;
    }
}