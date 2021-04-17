// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents a command to delete a data access model.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    [DataContract]
    public class DeleteDataAccessModelCommand<TIdentifier, TDataAccessModel> : DataAccessModelCommand<TIdentifier, TDataAccessModel, Nix>, IDeleteDataAccessModelCommand<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDataAccessModelCommand{TIdentifier, TDataAccessModel}" /> class.
        /// </summary>
        /// <param name="modelIdentifier">
        /// A value that uniquely identifies the associated data access model.
        /// </param>
        public DeleteDataAccessModelCommand(TIdentifier modelIdentifier)
            : base(modelIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDataAccessModelCommand{TIdentifier, TDataAccessModel}" /> class.
        /// </summary>
        [DebuggerHidden]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected internal DeleteDataAccessModelCommand()
            : base()
        {
            return;
        }
    }
}