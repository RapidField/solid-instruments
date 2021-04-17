// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents a command to perform an action related to a data access model.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessModelCommand{TIdentifier, TDataAccessModel}" /> is the default implementation of
    /// <see cref="IDataAccessModelCommand{TIdentifier, TDataAccessModel}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    [DataContract]
    public abstract class DataAccessModelCommand<TIdentifier, TDataAccessModel> : DataAccessModelCommand<TIdentifier, TDataAccessModel, Nix>, IDataAccessModelCommand<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessModelCommand{TIdentifier, TDataAccessModel}" /> class.
        /// </summary>
        protected DataAccessModelCommand()
            : base()
        {
            Model = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessModelCommand{TIdentifier, TDataAccessModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The associated data access model.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        protected DataAccessModelCommand(TDataAccessModel model)
            : base(model.RejectIf().IsNull(nameof(model)).TargetArgument.Identifier)
        {
            Model = model;
        }

        /// <summary>
        /// Gets or sets the associated <see cref="IDataAccessModel{TIdentifier}" />.
        /// </summary>
        [DataMember]
        public TDataAccessModel Model
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a command to perform an action related to a data access model.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessModelCommand{TIdentifier, TDataAccessModel, TResult}" /> is the default implementation of
    /// <see cref="IDataAccessModelCommand{TIdentifier, TDataAccessModel, TResult}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is produced by handling the data access model command.
    /// </typeparam>
    [DataContract]
    public abstract class DataAccessModelCommand<TIdentifier, TDataAccessModel, TResult> : DataAccessCommand<TResult>, IDataAccessModelCommand<TIdentifier, TDataAccessModel, TResult>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessModelCommand{TIdentifier, TDataAccessModel, TResult}" /> class.
        /// </summary>
        protected DataAccessModelCommand()
            : this(default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessModelCommand{TIdentifier, TDataAccessModel, TResult}" /> class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated data access model.
        /// </param>
        protected DataAccessModelCommand(TIdentifier modelIdentifier)
            : base()
        {
            ModelIdentifier = modelIdentifier;
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the associated <see cref="IDataAccessModel{TIdentifier}" />.
        /// </summary>
        [DataMember]
        public TIdentifier ModelIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the type of the associated data access model.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [IgnoreDataMember]
        public Type ModelType => ModelTypeReference;

        /// <summary>
        /// Represents the type of the associated data access model.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ModelTypeReference = typeof(TDataAccessModel);
    }
}