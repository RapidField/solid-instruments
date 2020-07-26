// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using SolidInstrumentsCommand = RapidField.SolidInstruments.Command.Command;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents a data access command that emits a result when processed.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessCommand{TResult}" /> is the default implementation of <see cref="IDataAccessCommand{TResult}" />.
    /// </remarks>
    /// <typeparam name="TResult">
    /// The type of the result that is produced by handling the data access command.
    /// </typeparam>
    [DataContract]
    public abstract class DataAccessCommand<TResult> : DataAccessCommand, IDataAccessCommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessCommand{TResult}" /> class.
        /// </summary>
        protected DataAccessCommand()
            : base()
        {
            return;
        }

        /// <summary>
        /// Gets the type of the result that is produced by handling the data access command.
        /// </summary>
        [IgnoreDataMember]
        public sealed override Type ResultType => ResultTypeReference;

        /// <summary>
        /// Represents the type of the result that is produced by handling the data access command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResultTypeReference = typeof(TResult);
    }

    /// <summary>
    /// Represents a data access command.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessCommand" /> is the default implementation of <see cref="IDataAccessCommand" />.
    /// </remarks>
    [DataContract]
    public abstract class DataAccessCommand : SolidInstrumentsCommand, IDataAccessCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessCommand" /> class.
        /// </summary>
        protected DataAccessCommand()
            : base()
        {
            return;
        }

        /// <summary>
        /// Gets the type of the result that is produced by processing the data access command.
        /// </summary>
        [IgnoreDataMember]
        public override Type ResultType => Nix.Type;
    }
}