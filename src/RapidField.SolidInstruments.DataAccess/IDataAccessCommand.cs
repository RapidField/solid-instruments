// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents a data access command.
    /// </summary>
    public interface IDataAccessCommand : IDataAccessCommand<Nix>, ICommand
    {
    }

    /// <summary>
    /// Represents a data access command that emits a result when processed.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the result that is produced by handling the data access command.
    /// </typeparam>
    public interface IDataAccessCommand<out TResult> : ICommand<TResult>
    {
    }
}