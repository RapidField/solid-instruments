// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command that is enriched with metadata information.
    /// </summary>
    public interface IMetadataEnrichedCommand : ICommand, IMetadataEnrichedObject
    {
    }

    /// <summary>
    /// Represents a command that emits a result when processed and that is enriched with metadata information.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted when processing the command.
    /// </typeparam>
    public interface IMetadataEnrichedCommand<out TResult> : ICommand<TResult>, IMetadataEnrichedObject
    {
    }
}